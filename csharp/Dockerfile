FROM debian:jessie as package-cache

RUN useradd --create-home user && \
    apt-get update && \
    apt-get install -y apt-transport-https wget git libssl-dev libssl1.0.0 && \
    wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-5.0 aspnetcore-runtime-5.0 dotnet-runtime-5.0 && \
    rm -rf /var/lib/apt/lists/*

COPY NuGetDownloader /build/NuGetDownloader

WORKDIR /build

RUN git clone --recursive https://github.com/TMC-CSharp/tmc-csharp-runner tmc-csharp-runner && \
    dotnet publish tmc-csharp-runner/Bootstrap/ -c Release -o tmc-csharp-runner/Bootstrap/bin/Publish && \
    dotnet run --project NuGetDownloader/NuGetDownloader.csproj --config-file NuGetDownloader/config.json --output-dir NuGet/packages

ADD --chown=user ${RUST_CLI_URL} /tmc-langs-cli
RUN chmod +x /tmc-langs-cli

FROM debian:stretch

ARG RUST_CLI_URL

ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8
ENV LANGUAGE en_US.UTF-8

RUN useradd --create-home user && \
    apt-get update && \
    apt-get install -y apt-transport-https wget openjdk-8-jdk openjfx libcglib3-java && \
    wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-5.0 aspnetcore-runtime-5.0 dotnet-runtime-5.0 && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app
RUN chown user /app
USER user

COPY --from=package-cache /build/tmc-csharp-runner/Bootstrap/bin/Publish /tmc-csharp-runner

ENV TMC_CSHARP_BOOTSTRAP_PATH /tmc-csharp-runner/TestMyCode.CSharp.Bootstrap.dll

COPY --from=package-cache /build/NuGet/packages /NuGet/cache
COPY _nuget.config /nuget.config

COPY Dockerfile /Dockerfile

COPY --from=package-cache /tmc-langs-cli /tmc-langs-cli
RUN /tmc-langs-cli --version
ADD --chown=user tmc-run /tmc-run

USER user
