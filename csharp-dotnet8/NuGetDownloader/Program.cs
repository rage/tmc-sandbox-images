using System.Collections.Immutable;
using System.CommandLine;
using System.Text.Json;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGetDownloader.Core;

namespace NuGetDownloader;

public static class Program
{
	public static async Task<int> Main(string[] args)
	{
		Option<FileInfo> configFile = new Option<FileInfo>(
			aliases: new[]
			{
				"--config-file",
				"-c",
			},
			description: "The location of the config file.",
			getDefaultValue: () => new FileInfo("config.json")
		).ExistingOnly();

		Option<DirectoryInfo> outputDirectory = new Option<DirectoryInfo>(
			aliases: new[]
			{
				"--output-dir",
				"-o"
			},
			description: "The directory to store the downloaded NuGet packages to.",
			getDefaultValue: () => new DirectoryInfo("packages")
		).LegalFilePathsOnly();

		RootCommand rootCommand = new(description: "Command line tool to download selected NuGet packages and all of their dependencies.")
		{
			configFile,
			outputDirectory
		};

		rootCommand.SetHandler(CommandHandler, configFile, outputDirectory);

		return await rootCommand.InvokeAsync(args);
	}

	private static async Task CommandHandler(FileInfo configFile, DirectoryInfo outputDir)
	{
		Downloader downloader = new(Program.GetSourceRepositories(), outputDir);

		await Program.ReadConfig(downloader, configFile);

		await downloader.Completion;
	}

	private static ImmutableList<SourceRepository> GetSourceRepositories()
	{
		ISettings settings = Settings.LoadDefaultSettings(Environment.CurrentDirectory);

		PackageSourceProvider source = new(settings);
		SourceRepositoryProvider provider = new(source, Repository.Provider.GetCoreV3());

		IEnumerable<PackageSource> sources = source.LoadPackageSources();
		IEnumerable<SourceRepository> repositories = sources.Select(provider.CreateRepository);

		return repositories.ToImmutableList();
	}

	private static async Task ReadConfig(Downloader downloader, FileInfo configFile)
	{
		await using FileStream stream = configFile.OpenRead();

		Dictionary<string, HashSet<string>>? config = await JsonSerializer.DeserializeAsync<Dictionary<string, HashSet<string>>>(stream);
		if (config is null)
		{
			return;
		}

		foreach ((string packageId, HashSet<string> packageVersions) in config)
		{
			foreach (string packageVersion in packageVersions)
			{
				PackageIdentity package = new(packageId, NuGetVersion.Parse(packageVersion));

				downloader.TryQueue(package);
			}
		}
	}
}