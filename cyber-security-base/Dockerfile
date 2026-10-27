# The venv's interpreter symlinks into /usr/local, so both stages must use the same image.
ARG PYTHON_IMAGE=python:3.13-slim-trixie

FROM ${PYTHON_IMAGE} AS builder

COPY --from=ghcr.io/astral-sh/uv:0.12.2 /uv /bin/uv

ENV UV_PROJECT_ENVIRONMENT=/opt/venv \
    UV_PYTHON_DOWNLOADS=never \
    UV_COMPILE_BYTECODE=1

WORKDIR /build
COPY pyproject.toml uv.lock ./
RUN uv sync --locked --no-dev

FROM ${PYTHON_IMAGE}

RUN apt-get update && \
    apt-get install -y --no-install-recommends unzip curl wget locales sqlite3 zstd chromium chromium-driver fonts-liberation && \
    sed -i 's/^# *en_US.UTF-8/en_US.UTF-8/' /etc/locale.gen && \
    locale-gen && \
    ln -s /usr/bin/chromium /usr/bin/google-chrome && \
    ln -s /usr/bin/chromium /usr/bin/google-chrome-stable && \
    useradd --create-home user && \
    rm -rf /var/lib/apt/lists/*

ENV LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8 \
    LANGUAGE=en_US.UTF-8 \
    PATH=/opt/venv/bin:$PATH \
    SE_OFFLINE=true

COPY --from=builder /opt/venv /opt/venv

WORKDIR /app
RUN chown user /app

ARG RUST_CLI_URL
RUN test -n "$RUST_CLI_URL"
ADD --chown=user --chmod=755 ${RUST_CLI_URL} /tmc-langs-cli
RUN /tmc-langs-cli --version
ADD --chown=user tmc-run /tmc-run

COPY Dockerfile /
# Kept out of / so tools searching upward from /app for project config do not find them.
COPY pyproject.toml uv.lock /image/

USER user
