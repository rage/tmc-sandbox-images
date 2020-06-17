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

COPY tmc-langs/target/tmc-langs-cli-0.8.0-SNAPSHOT.jar /tmc-langs.jar
COPY tmc-csharp-runner/Bootstrap/bin/Publish /tmc-csharp-runner

ENV TMC_CSHARP_BOOTSTRAP_PATH /tmc-csharp-runner/Bootstrap.dll

COPY NuGet/packages /NuGet/cache
COPY NuGet/nuget.config /nuget.config
