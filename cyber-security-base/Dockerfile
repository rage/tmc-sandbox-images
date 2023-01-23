FROM debian:buster

ARG RUST_CLI_URL
RUN test -n "$RUST_CLI_URL"

# latest chrome download url
ENV CHROME_URL https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb

RUN apt-get update && \
    apt-get install -y wget build-essential python3.7 python3-pip unzip curl libsqlite3-dev sqlite3 && \
    wget ${CHROME_URL} && \
    apt install -y ./google-chrome*.deb && \
    # zstd for sandbox
    apt install -y zstd && \
    useradd --create-home user && \
    rm -rf /var/lib/apt/lists/*

RUN pip3 install django selenium beautifulsoup4 requests

# Gets latest chromedriver, the latest chromedriver should match to the latest chrome
RUN wget http://chromedriver.storage.googleapis.com/`curl -sS chromedriver.storage.googleapis.com/LATEST_RELEASE`/chromedriver_linux64.zip
RUN unzip chromedriver_linux64.zip
RUN mv chromedriver /usr/bin/

WORKDIR /app
RUN chown user /app
USER user

COPY Dockerfile /Dockerfile

ADD --chown=user ${RUST_CLI_URL} /tmc-langs-cli
RUN chmod +x /tmc-langs-cli
ADD --chown=user tmc-run /tmc-run

USER user
