FROM debian:buster

ARG RUST_CLI_URL
RUN test -n "$RUST_CLI_URL"

RUN \
    apt update && \
    # common utilities
    apt install -y unzip curl build-essential libssl1.1 wget locales locales-all && \
    # required for building Python
    apt install -y wget make g++ zlib1g-dev libffi-dev libssl-dev && \
    # optional build dependencies for Python
    apt install -y libbz2-dev libncurses-dev libgdbm-dev liblzma-dev libsqlite3-dev tk-dev uuid-dev libreadline-dev && \
    # other Python packages
    apt install -y python3-pip python3-numpy python3-scipy python3-matplotlib python3-sklearn python3-skimage python3-tk python3-pandas && \
    useradd --create-home user && \
    rm -rf /var/lib/apt/lists/*

ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8
ENV LANGUAGE en_US.UTF-8

RUN bash -c "mkdir -p /pythonbuild && cd /pythonbuild && wget https://www.python.org/ftp/python/3.8.8/Python-3.8.8.tgz && tar -xzvf Python-3.8.8.tgz && cd Python-3.8.8/ && ./configure && make && make install"

COPY requirements.txt /requirements.txt

RUN pip3 install -r ./requirements.txt

COPY Dockerfile /Dockerfile

ADD --chown=user ${RUST_CLI_URL} /tmc-langs-cli
RUN chmod +x /tmc-langs-cli && /tmc-langs-cli --version
ADD --chown=user tmc-run /tmc-run

WORKDIR /app
RUN chown user /app
USER user
