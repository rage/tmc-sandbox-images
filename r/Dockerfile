FROM debian:buster

ARG RUST_CLI_URL
RUN test -n "$RUST_CLI_URL"

RUN \
    # see https://cran.r-project.org/bin/linux/debian/#debian-buster-stable
    echo "deb http://cloud.r-project.org/bin/linux/debian buster-cran40/ >> /etc/apt/sources.list" && \
    apt update && \
    # common utilities
    apt install -y unzip curl build-essential libssl wget locale locale-all \
    # required by devtools
    apt install -y libssl-dev libcurl4-openssl-dev libxml2-dev && \
    # install R
    apt install -y r-base r-base-dev && \
    useradd --create-home user && \
    rm -rf /var/lib/apt/lists/*

ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8
ENV LANGUAGE en_US.UTF-8

RUN Rscript -e 'install.packages(c("devtools","testthat", "httr", "curl"),repos="https://ftp.eenet.ee/pub/cran/")' && Rscript -e 'devtools::install_github("RTMC/tmc-r-tester/tmcRtestrunner")'

COPY Dockerfile /Dockerfile

ADD --chown=user ${RUST_CLI_URL} /tmc-langs-cli
RUN chmod +x /tmc-langs-cli && /tmc-langs-cli --version
ADD --chown=user tmc-run /tmc-run

WORKDIR /app
RUN chown user /app
USER user
