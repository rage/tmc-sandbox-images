echo "--------------------------------------"                               &&
echo "Installing packages for R 4.2.1       "                               &&
echo "--------------------------------------"                               &&
#
# from: <https://cran.r-project.org/bin/linux/debian/>
# <quote>
#    Since 16th of November 2021, the bullseye-cran40, buster-cran40, and
#    since 17th of November also the buster-cran35 and stretch-cran35
#    repositories are signed with a new key with key ID 0xB8F25A8A73EACF41,
#    key fingerprint 95C0FAF38DB3CCAD0C080A7BDC78B2DDEABC47B7 and user ID
#    Johannes Ranke <johannes.ranke@jrwb.de>
# </quote>
#
# from: <https://cran.r-project.org/bin/linux/debian/>
# <quote>
#    Since the release of R 3.4.0 in April 2017, the repositories are
#    signed with the current signing key of Johannes Ranke
#    <jranke@uni-bremen.de> (alternative uid is Johannes Ranke
#    (wissenschaftlicher Berater) <johannes.ranke@jrwb.de>) with key
#    fingerprint E19F 5F87 1288 99B1 92B1  A2C2 AD5F 960A 256A 04AF
# </quote>
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
# common utilities (most already added)
apt-get install -y unzip curl build-essential libssl1.1 wget locales        \
                   locales-all                                              &&
# required by devtools
apt-get install -y libssl-dev libcurl4-openssl-dev libxml2-dev              &&
# install R old
apt-get install -y r-base r-base-dev                                        &&
#
apt-get update                                                              &&
# required with building R 4.2.1 (in addition to the long list)
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
