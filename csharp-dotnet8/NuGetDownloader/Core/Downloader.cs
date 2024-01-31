using System.Collections.Immutable;
using System.Threading.Tasks.Dataflow;
using NuGet.Common;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace NuGetDownloader.Core;

internal sealed class Downloader
{
	private readonly ActionBlock<PackageIdentity> downloadQueue;
	private readonly HashSet<PackageIdentity> downloadQueued;

	private readonly PackageDownloadContext downloadCache;

	private int downloadCount;

	public ImmutableList<SourceRepository> Sources { get; }

	public string DownloadDirectory { get; }

	internal Downloader(ImmutableList<SourceRepository> sources, DirectoryInfo outputDir)
	{
		this.downloadQueue = new ActionBlock<PackageIdentity>(this.ConsumeItemAsync, new ExecutionDataflowBlockOptions
		{
			MaxDegreeOfParallelism = Environment.ProcessorCount
		});
		this.downloadQueued = new HashSet<PackageIdentity>();

		this.downloadCache = new PackageDownloadContext(NullSourceCacheContext.Instance);

		this.Sources = sources;

		this.DownloadDirectory = outputDir.FullName;
	}

	public Task Completion => this.downloadQueue.Completion;

	public void TryQueue(PackageIdentity package)
	{
		lock (this.downloadQueued)
		{
			if (!this.downloadQueued.Add(package))
			{
				return;
			}
		}

		this.downloadQueue.Post(package);
	}

	private async Task ConsumeItemAsync(PackageIdentity package)
	{
		Console.WriteLine($"Processing {package}");

		DownloadResourceResult result = await PackageDownloader.GetDownloadResourceResultAsync(this.Sources, package, this.downloadCache, this.DownloadDirectory, DebugLogger.instance, CancellationToken.None);

		foreach (PackageDependencyGroup dependencyGroup in await result.PackageReader.GetPackageDependenciesAsync(CancellationToken.None))
		{
			foreach (PackageDependency dependency in dependencyGroup.Packages)
			{
				PackageIdentity dependencyPackage = new(dependency.Id, dependency.VersionRange.MaxVersion ?? dependency.VersionRange.MinVersion);

				this.TryQueue(dependencyPackage);
			}
		}

		int count = Interlocked.Increment(ref this.downloadCount);
		if (count == this.downloadQueued.Count)
		{
			this.downloadQueue.Complete();
		}
	}

	private sealed class DebugLogger : LoggerBase
	{
		internal static readonly DebugLogger instance = new();

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