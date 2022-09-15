#
echo "--------------------------------------"                               &&
echo "Installing R 4.2.1                    "                               &&
echo "--------------------------------------"                               &&
#
# download R 4.2.1
cd /rbuild                                                                  &&
wget https://cran.r-project.org/src/base/R-4/R-4.2.1.tar.gz                 &&
tar -xzvf R-4.2.1.tar.gz                                                    &&
cd R-4.2.1                                                                  &&
# configure, build and install R 4.2.1
./configure                                                                 &&
make                                                                        &&
make install                                                                &&
cd /rbuild                                                                  &&
# clean up after building
rm -rf R-4.2.1                                                              &&
# install tmcRtestrunner and required libraries
#
echo "--------------------------------------"                               &&
echo "Installing tmcRtestrunner             "                               &&
echo "--------------------------------------"                               &&
#
Rscript /rbuild/installer.R
