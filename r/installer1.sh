debianversion=$(grep VERSION_CODENAME /etc/os-release | cut -f2 -d=)
debian_cran_key='95C0FAF38DB3CCAD0C080A7BDC78B2DDEABC47B7'
debian_cran_signature='/etc/apt/keyrings/cran_debian_key.pgp'
signature="[signed-by=${debian_cran_signature}]"
debian_cran_host_server='https://cloud.r-project.org/bin/linux/debian'
R_version_tested="latest R (4.4.1)"

echo
echo "-------------------------------------------"                          &&
echo "Installing packages for ${R_version_tested}"                          &&
echo "-------------------------------------------"                          &&
#
if [[ "${debianversion}" == "buster" ]];
then
  echo "using Debian Buster"
  libsslpkg=libssl1.1
elif [[ "${debianversion}" == "bullseye" ]];
then
  echo "using Debian Bullseye"
  libsslpkg=libssl1.1
elif [[ "${debianversion}" == "bookworm" ]];
then
  echo "using Debian Bookworm"
  libsslpkg=libssl3
else
  echo "---------------"                                                    &&
  echo "    NOTE       "                                                    &&
  echo "---------------"                                                    &&
  echo "Version '${debianversion}' is unknown, assuming Debian Buster."     &&
  echo "This should not happen when updating to, say, Debian Bookworm."     &&
  echo "A handler for new version needs to added for he later Debian"       &&
  echo "versions."                                                          &&
  echo "---------------"
  debianversion=buster
  libsslpkg=libssl1.1
fi
backports="deb ${signature} ${debian_cran_host_server} ${debianversion}-cran40/"
#
apt-get install -y gnupg2 apt-transport-https ca-certificates               &&
#
mkdir -p /etc/apt/keyrings                                                  &&
gpg --keyserver keyserver.ubuntu.com --recv-key ${debian_cran_key}          &&
gpg --export ${debian_cran_key} > ${debian_cran_signature}                  &&
echo ${backports} >> /etc/apt/sources.list                                  &&
#
apt-get update                                                              &&
# common utilities
apt-get install -y unzip curl build-essential wget locales locales-all      &&
apt-get install -y ${libsslpkg}                                             &&
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
apt-get install -y libharfbuzz-dev libfribidi-dev libgit2-dev               &&
# cleaning up the apt lists
rm -rf /var/lib/apt/lists/*                                                 &&
#
echo "-------------------------------------------------"                    &&
echo "Installing packages for ${R_version_tested}: done"                    &&
echo "-------------------------------------------------"
#
