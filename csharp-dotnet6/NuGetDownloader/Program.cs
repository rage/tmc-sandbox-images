using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGetDownloader.Core;

namespace NuGetDownloader
{
    public static class Program
    {
        public const string DefaultDownloadDir = "packages";

        public static async Task Main(string[] args)
        {
            RootCommand rootCommand = Program.GenerateCommands();
            rootCommand.Handler = CommandHandler.Create(async (FileInfo? configFile, DirectoryInfo? outputDir) =>
            {
                Downloader downloader = Program.BuildDownloader(outputDir?.FullName ?? Path.Combine(Environment.CurrentDirectory, Program.DefaultDownloadDir));

                await Program.ReadConfig(downloader, configFile?.FullName ?? "config.json");

                await downloader.ConsumeQueueAsync();
            });

            await rootCommand.InvokeAsync(args);
        }

        private static RootCommand GenerateCommands()
        {
            return new RootCommand(description: "NuGet helper for downloading selected packages and all of their dependencies.")
            {
                new Option<FileInfo>(aliases: new[]
                {
                    "--config-file",
                    "-c",
                }, description: "Config file location.")
                {
                    Argument = new Argument<FileInfo>().ExistingOnly()
                },

                new Option<DirectoryInfo>(aliases: new[]
                {
                    "--output-dir",
                    "-o"
                }, description: "Sets the directory where the NuGet packages are downloaded to.")
                {
                    Argument = new Argument<DirectoryInfo>()
                }
            };
        }

        private static Downloader BuildDownloader(string downloadDir)
        {
            ImmutableList<SourceRepository> repositories = Program.GetSourceRepositories();

            Downloader downloader = new Downloader(downloadDir, repositories);

            return downloader;
        }

        private static ImmutableList<SourceRepository> GetSourceRepositories()
        {
            ISettings settings = Settings.LoadDefaultSettings(Environment.CurrentDirectory);

            PackageSourceProvider source = new PackageSourceProvider(settings);
            SourceRepositoryProvider provider = new SourceRepositoryProvider(source, Repository.Provider.GetCoreV3());

            IEnumerable<PackageSource> sources = PackageSourceProvider.LoadPackageSources(settings);
            IEnumerable<SourceRepository> repositories = sources.Select(provider.CreateRepository);

            return repositories.ToImmutableList();
        }

        private static async Task ReadConfig(Downloader downloader, string configLocation)
        {
            Dictionary<string, HashSet<string>> config = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(await File.ReadAllTextAsync(configLocation))!;

            foreach ((string packageId, HashSet<string> packageVersions) in config)
            {
                foreach (string packageVersion in packageVersions)
                {
                    PackageIdentity package = new PackageIdentity(packageId, NuGetVersion.Parse(packageVersion));

                    downloader.TryQueue(package);
                }
            }
        }
    }
}
