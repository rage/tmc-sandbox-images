FROM debian:buster

ARG RUST_CLI_URL
RUN test -n "$RUST_CLI_URL"

RUN apt-get update && \
    apt-get install zstd

RUN mkdir -p /rbuild
COPY installer1.sh              /rbuild/
RUN bash /rbuild/installer1.sh

RUN useradd --create-home user

COPY installer2.sh installer.R  /rbuild/
RUN bash /rbuild/installer2.sh

ENV LC_ALL      en_US.UTF-8
ENV LANG        en_US.UTF-8
ENV LANGUAGE    en_US.UTF-8

COPY Dockerfile /Dockerfile

ADD --chown=user ${RUST_CLI_URL} /tmc-langs-cli
RUN chmod +x /tmc-langs-cli && /tmc-langs-cli --version
ADD --chown=user tmc-run /tmc-run

WORKDIR /app
RUN chown user /app
USER user
