git -C tmc-langs pull || git clone https://github.com/testmycode/tmc-langs tmc-langs
mvn -DskipTests -f tmc-langs clean package

git -C tmc-csharp-runner pull --recurse-submodules || git clone --recursive https://github.com/TMC-CSharp/tmc-csharp-runner tmc-csharp-runner
dotnet publish tmc-csharp-runner/Bootstrap/ -c Release -o tmc-csharp-runner/Bootstrap/bin/Publish

dotnet run --project NuGetDownloader/NuGetDownloader.csproj --config-file NuGetDownloader/config.json --output-dir NuGet/packages

docker build .