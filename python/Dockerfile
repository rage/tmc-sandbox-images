FROM debian:buster

ARG RUST_CLI_URL
RUN test -n "$RUST_CLI_URL"

RUN \
    apt update && \
    # required for building Python
    apt install -y wget make g++ zlib1g-dev libffi-dev libssl-dev && \
    # optional build dependencies for Python
    apt install -y libbz2-dev libncurses-dev libgdbm-dev liblzma-dev libsqlite3-dev tk-dev uuid-dev libreadline-dev && \
    # other Python packages
    apt install -y python3-pip python3-numpy python3-scipy python3-matplotlib python3-sklearn python3-skimage python3-tk python3-pandas && \
    useradd --create-home user && \
    rm -rf /var/lib/apt/lists/*

RUN bash -c "mkdir -p /pythonbuild && cd /pythonbuild && wget https://www.python.org/ftp/python/3.8.8/Python-3.8.8.tgz && tar -xzvf Python-3.8.8.tgz && cd Python-3.8.8/ && ./configure && make && make install"

RUN pip3 install spacy numpy scipy matplotlib sklearn scikit-image pandas seaborn lxml matplotlib statsmodels nbformat beautifulsoup4 pillow spacy pygame

COPY Dockerfile /Dockerfile

ADD --chown=user ${RUST_CLI_URL} /tmc-langs-cli
RUN chmod +x /tmc-langs-cli && /tmc-langs-cli --version
ADD --chown=user tmc-run /tmc-run

WORKDIR /app
RUN chown user /app
USER user
