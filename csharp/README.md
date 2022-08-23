# NuGet cache
Due to the container not having access to the internet, all required packages need to be avaiable inside the container. The required packages can be defined on `NuGetDownloader/config.json` which is processed during image build. The packages listed and their dependencies are downloaded automatically and copied to `/NuGet/cache`.

Finally `_nuget.config` is copied to `/nuget.config` in order to force package lookups to `/NuGet/cache`.

# Upgrading .NET SDK version
In order to support new major version of the SDK, you first need to bump the installed version inside the docker image. Additionally to preserve the ability to buid and run older versions, you need to add the old version reference assemblies to be downloaded to the NuGet cache. These are the `Microsoft.NETCore.App.Ref` and `Microsoft.AspNetCore.App.Ref`.

Changes to the `tmc-csharp-runner` are not required as it's configured to roll forward to the latest major version available on the machine with the `<RollForward>LatestMajor</RollForward>` switch. By bumping up the major version, it's requires that at least that version of the .NET is available on the machine. In the future there might be breaking changes to .NET that require code changes.

For example, upgrading from .NET 6 to .NET 8 would mean installing `dotnet-sdk-8.0` instead of `dotnet-sdk-6.0` and adding the following to `NuGetDownloader/config.json`:
```json
"Microsoft.NETCore.App.Ref": [
	"6.0.0"
],
"Microsoft.AspNetCore.App.Ref": [
	"6.0.0"
]
```