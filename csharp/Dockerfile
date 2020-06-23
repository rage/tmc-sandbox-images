FROM debian:jessie as package-cache

RUN useradd --create-home user && \
    apt-get update && \
    apt-get install -y apt-transport-https wget git libssl-dev libssl1.0.0 && \
    wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-3.1 aspnetcore-runtime-3.1 dotnet-runtime-3.1 && \
    rm -rf /var/lib/apt/lists/*

COPY NuGetDownloader /build/NuGetDownloader

WORKDIR /build

RUN git clone --recursive https://github.com/TMC-CSharp/tmc-csharp-runner tmc-csharp-runner && \
    dotnet publish tmc-csharp-runner/Bootstrap/ -c Release -o tmc-csharp-runner/Bootstrap/bin/Publish && \
    dotnet run --project NuGetDownloader/NuGetDownloader.csproj --config-file NuGetDownloader/config.json --output-dir NuGet/packages

FROM debian:stretch

RUN useradd --create-home user && \
    apt-get update && \
    apt-get install -y apt-transport-https wget openjdk-8-jdk openjfx libcglib3-java && \
    wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-3.1 aspnetcore-runtime-3.1 dotnet-runtime-3.1 && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app
RUN chown user /app
USER user

COPY --from=package-cache /build/tmc-csharp-runner/Bootstrap/bin/Publish /tmc-csharp-runner

ENV TMC_CSHARP_BOOTSTRAP_PATH /tmc-csharp-runner/Bootstrap.dll

COPY --from=package-cache /build/NuGet/packages /NuGet/cache
COPY nuget.config /nuget.config

ADD https://download.mooc.fi/tmc-langs/tmc-langs-cli-0.8.2-SNAPSHOT.jar /tmc-langs.jar
