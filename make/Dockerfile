FROM debian:buster

ARG RUST_CLI_URL
RUN test -n "$RUST_CLI_URL"

RUN \
    apt update && \
    apt install -y make g++ check pkg-config valgrind && \
    useradd --create-home user && \
    rm -rf /var/lib/apt/lists/*

COPY Dockerfile /Dockerfile

ADD --chown=user ${RUST_CLI_URL} /tmc-langs-cli
RUN chmod +x /tmc-langs-cli && /tmc-langs-cli --version
ADD --chown=user tmc-run /tmc-run

WORKDIR /app
RUN chown user /app
USER user
