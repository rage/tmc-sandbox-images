FROM debian:buster

ARG RUST_CLI_URL
RUN test -n "$RUST_CLI_URL"

RUN \
    apt update && \
    # common utilities
    apt install -y unzip curl build-essential libssl1.1 wget locales locales-all zstd && \
    # Java packages
    apt install -y openjdk-11-jdk maven ant && \
    useradd --create-home user && \
    rm -rf /var/lib/apt/lists/*

ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8
ENV LANGUAGE en_US.UTF-8

WORKDIR /app
RUN chown user /app
USER user

COPY maven-templates/maven-template /app/

RUN mvn -q clean compile test-compile
RUN mvn -q -U dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.6:test || true

COPY maven-templates/maven-template-2 /app/

RUN mvn -q clean compile test-compile
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.6:test || true

COPY maven-templates/maven-template-3 /app/

RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.6:test || true

COPY maven-templates/maven-template-4 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-5 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-6 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-7 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-8 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-9 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-10 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-11 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-12 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-13 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-14 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-15 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-16 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-17 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-18 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-19 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

COPY maven-templates/maven-template-20 /app
RUN mvn -q clean compile test-compile || true
RUN mvn -q dependency:resolve-plugins dependency:go-offline compile dependency:resolve-plugins
RUN mvn -q test || true
RUN mvn -q fi.helsinki.cs.tmc:tmc-maven-plugin:1.12:test || true

ENV M2_HOME="/usr/share/maven"

COPY Dockerfile /Dockerfile

ADD --chown=user ${RUST_CLI_URL} /tmc-langs-cli
RUN chmod +x /tmc-langs-cli && /tmc-langs-cli --version
ADD --chown=user tmc-run /tmc-run
