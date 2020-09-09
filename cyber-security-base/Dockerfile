FROM debian:stretch

ARG RUST_CLI_URL

# latest chrome download url
ENV CHROME_URL https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb

RUN apt-get update && \
    apt-get install -y gnupg2 && \
    ( apt-key adv --keyserver keys.gnupg.net --no-tty --recv-keys 'E19F5F87128899B192B1A2C2AD5F960A256A04AF' || \
    apt-key adv --keyserver keys.openpgp.org --no-tty --recv-keys 'E19F5F87128899B192B1A2C2AD5F960A256A04AF' || \
    apt-key adv --keyserver keyserver.ubuntu.com --no-tty --recv-keys 'E19F5F87128899B192B1A2C2AD5F960A256A04AF' ) && \
    apt-get install -y makedev locales build-essential g++ nano procps xvfb iproute2 net-tools iputils-ping curl wget rsync libxrender-dev libxtst-dev check pkg-config valgrind libxslt-dev libxml2-dev libreadline-dev curl git-core zlib1g zlib1g-dev libssl-dev libyaml-dev libsqlite3-dev sqlite3 libxml2-dev libxslt-dev autoconf libc6-dev libgdbm-dev ncurses-dev automake libtool bison subversion libffi-dev openssl libxrender-dev libxtst-dev xmlstarlet python3 python3-pip moreutils libcurl4-openssl-dev libfreetype6 libfreetype6-dev libgl1-mesa-dev libfontconfig1-dev libx11-dev libx11-dev libxext-dev libxfixes-dev libxi-dev libxrender-dev libxcb1-dev libx11-xcb-dev libxcb-glx0-dev software-properties-common libncurses5-dev libgdbm-dev libnss3-dev libssl-dev libreadline-dev libffi-dev zlib1g libdouble-conversion1 libxcb-xkb1 libxcb-render-util0 libxcb-xinerama0 libxcb-image0 libxcb-keysyms1 libxcb-icccm4 libxkbcommon-x11-0 libxcb-xkb1 unzip && \
    wget ${CHROME_URL} && \
    apt install -y ./google-chrome*.deb && \
    useradd --create-home user && \
    rm -rf /var/lib/apt/lists/*

RUN python3 -m pip install django selenium beautifulsoup4 requests

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
