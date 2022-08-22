FROM debian:bullseye as package-cache

ARG RUST_CLI_URL
RUN test -n "$RUST_CLI_URL"

RUN useradd --create-home user && \
    apt-get update && \
    apt-get install -y apt-transport-https wget git locales locales-all && \
    wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-6.0 && \
    rm -rf /var/lib/apt/lists/*

# Make sure to install packages locales and locales-all in addition to these
ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8
ENV LANGUAGE en_US.UTF-8

COPY NuGetDownloader /build/NuGetDownloader

WORKDIR /build

RUN git clone --recursive https://github.com/testmycode/tmc-csharp-runner tmc-csharp-runner && \
    dotnet publish tmc-csharp-runner/Bootstrap/ -c Release -o tmc-csharp-runner/Bootstrap/bin/Publish && \
    dotnet run --project NuGetDownloader/NuGetDownloader.csproj --config-file NuGetDownloader/config.json --output-dir NuGet/packages

ADD --chown=user ${RUST_CLI_URL} /tmc-langs-cli
RUN chmod +x /tmc-langs-cli

FROM debian:bullseye

RUN useradd --create-home user && \
    apt-get update && \
    apt-get install -y apt-transport-https wget locales locales-all && \
    wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-6.0 && \
    rm -rf /var/lib/apt/lists/*

# Make sure to install packages locales and locales-all in addition to these
ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8
ENV LANGUAGE en_US.UTF-8

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
