echo "--------------------------------------"                               &&
echo "Installing packages for R 4.2.1       "                               &&
echo "--------------------------------------"                               &&
#
apt-get update                                                              &&
apt-get install -y gnupg2 apt-transport-https                               &&
( apt-key adv --keyserver keyserver.ubuntu.com --no-tty                     \
              --recv-keys '95C0FAF38DB3CCAD0C080A7BDC78B2DDEABC47B7'   ||   \
  apt-key adv --keyserver keyserver.ubuntu.com --no-tty                     \
              --recv-keys 'E19F5F87128899B192B1A2C2AD5F960A256A04AF' )      &&
echo "deb https://cran.r-project.org/bin/linux/debian stretch-cran35/" >>   \
    /etc/apt/sources.list                                                   &&
apt-get update                                                              &&
# common utilities
apt-get install -y unzip curl build-essential libssl1.1 wget locales        \
                   locales-all                                              &&
# required by devtools
apt-get install -y libssl-dev libcurl4-openssl-dev libxml2-dev              &&
# install R
apt-get install -y r-base r-base-dev                                        &&
useradd --create-home user

apt-get update                                                              &&
apt-get install -y bison nano automake autoconf libx11-dev                  \
                   texinfo libxt-dev x11proto-core-dev libx11-xcb-dev       \
                   libxkbcommon-x11-0 less file libpango1.0-dev             \
                   libcairo2-dev tcl8.6-dev tk8.6-dev libtiff5-dev          &&
# required with newer R packages
apt-get install -y libharfbuzz-dev libfribidi-dev                           &&
# cleaning up the apt lists
rm -rf /var/lib/apt/lists/*                                                 &&
#
echo "--------------------------------------"                               &&
echo "Installing packages for R 4.2.1: done "                               &&
echo "--------------------------------------"
#
