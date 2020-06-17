using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGetDownloader.Core
{
    internal sealed class Downloader
    {
        private ConcurrentDictionary<PackageIdentity, bool> Installed;
        private ConcurrentQueue<PackageIdentity> DownloadQueue;

        private PackageDownloadContext DownloadCache;

        public string DownloadDirectory { get; }

        public ImmutableList<SourceRepository> Sources { get; }

        internal Downloader(string downloadDirectory, ImmutableList<SourceRepository> sources)
        {
            this.Installed = new ConcurrentDictionary<PackageIdentity, bool>();
            this.DownloadQueue = new ConcurrentQueue<PackageIdentity>();

            this.DownloadCache = new PackageDownloadContext(NullSourceCacheContext.Instance);

            this.DownloadDirectory = downloadDirectory;

            this.Sources = sources;
        }

        public void TryQueue(PackageIdentity package)
        {
            //If we are unaware of the new package and we haven't installed it yet
            if (this.Installed.TryAdd(package, false))
            {
                this.DownloadQueue.Enqueue(package);
            }
        }

        public async Task ConsumeQueueAsync()
        {
            List<Task> tasks = new List<Task>(Environment.ProcessorCount * 2);
            while (this.DownloadQueue.Count > 0 || tasks.Count > 0)
            {
                while (this.DownloadQueue.TryDequeue(out PackageIdentity? package))
                {
                    //If we haven't installed it yet then process it
                    if (!this.Installed.TryUpdate(package, true, false))
                    {
                        continue;
                    }

                    Task task = Task.Run(async () => await this.ConsumeItemAsync(package));

                    tasks.Add(task);

                    if (tasks.Count == tasks.Capacity)
                    {
                        break;
                    }
                }

                Task completedTask = await Task.WhenAny(tasks);

                tasks.Remove(completedTask);
            }
        }

        private async Task ConsumeItemAsync(PackageIdentity package)
        {
            Console.WriteLine($"Processing {package}");

            DownloadResourceResult result = await PackageDownloader.GetDownloadResourceResultAsync(this.Sources, package, this.DownloadCache, this.DownloadDirectory, DebugLogger.Instance, CancellationToken.None);

            foreach (PackageDependencyGroup dependencyGroup in await result.PackageReader.GetPackageDependenciesAsync(CancellationToken.None))
            {
                foreach (PackageDependency dependency in dependencyGroup.Packages)
                {
                    PackageIdentity dependencyPackage = new PackageIdentity(dependency.Id, dependency.VersionRange.MaxVersion ?? dependency.VersionRange.MinVersion);

                    this.TryQueue(dependencyPackage);
                }
            }
        }

        private sealed class DebugLogger : LoggerBase
        {
            internal static readonly DebugLogger Instance = new DebugLogger();

            private DebugLogger() : base(LogLevel.Debug)
            {

            }

            public override void Log(ILogMessage message) => Console.WriteLine(message);

            public override Task LogAsync(ILogMessage message)
            {
                this.Log(message);

                return Task.CompletedTask;
            }
        }
    }
}
