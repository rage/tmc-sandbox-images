## Dockerfile for R 4.2.1

```
#5 [ 2/13] RUN test -n "https://download.mooc.fi/tmc-langs-rust/tmc-langs-cli-x86_64-unknown-linux-gnu-0.10.0.4"
#5 sha256:75576dd64e6f9ddff2e09187160bf25fca2196a0c6cb0ad81937af833ddf6c28
#5 DONE 0.3s
------
 > [11/13] ADD --chown=user tmc-run /tmc-run:
------
```

Now we have a new missing part:

```
#11 1776.6 * installing *source* package 'textshaping' ...
#11 1776.6 ** package 'textshaping' successfully unpacked and MD5 sums checked
#11 1776.6 ** using staged installation
#11 1776.7 Package fribidi was not found in the pkg-config search path.
#11 1776.7 Perhaps you should add the directory containing `fribidi.pc'
#11 1776.7 to the PKG_CONFIG_PATH environment variable
#11 1776.7 No package 'fribidi' found
#11 1776.7 Using PKG_CFLAGS=
#11 1776.7 Using PKG_LIBS=-lfreetype -lharfbuzz -lfribidi -lpng
#11 1776.8 --------------------------- [ANTICONF] --------------------------------
#11 1776.8 Configuration failed to find the harfbuzz freetype2 fribidi library. Try installing:
#11 1776.8  * deb: libharfbuzz-dev libfribidi-dev (Debian, Ubuntu, etc)
#11 1776.8  * rpm: harfbuzz-devel fribidi-devel (Fedora, EPEL)
#11 1776.8  * csw: libharfbuzz_dev libfribidi_dev (Solaris)
#11 1776.8  * brew: harfbuzz fribidi (OSX)
#11 1776.8 If harfbuzz freetype2 fribidi is already installed, check that 'pkg-config' is in your
#11 1776.8 PATH and PKG_CONFIG_PATH contains a harfbuzz freetype2 fribidi.pc file. If pkg-config
#11 1776.8 is unavailable you can set INCLUDE_DIR and LIB_DIR manually via:
#11 1776.8 R CMD INSTALL --configure-vars='INCLUDE_DIR=... LIB_DIR=...'
#11 1776.8 -------------------------- [ERROR MESSAGE] ---------------------------
#11 1776.8 <stdin>:1:19: fatal error: hb-ft.h: No such file or directory
#11 1776.8 compilation terminated.
#11 1776.8 --------------------------------------------------------------------
#11 1776.8 ERROR: configuration failed for package 'textshaping'
```




Before this, read the README for the R 3.6.3 version.  Let's turn this
into a R4.2.0 for comparison. In order to get R 4.2.0 building cleanly
we are missing the following libraries libraries

    RUN apt-get update && \
        apt-get install -y bison nano automake autoconf libx11-dev \
                           texinfo libxt-dev x11proto-core-dev libx11-xcb-dev \
                           libxkbcommon-x11-0 less file libpango1.0-dev \
                           libcairo2-dev tcl8.6-dev tk8.6-dev libtiff5-dev

and with these we can build the R4.2.0. Without cleanng we see that it
is 600MB larger.

    $ docker images -a
    REPOSITORY      TAG                        IMAGE ID       SIZE
    tmc-langs-cli   0.10.0.4_testmain_r420     ece16c2e86e5   1.62GB
    tmc-langs-cli   0.10.0.4_testmain_r363     925ab78c46a7   1.03GB

And that works. In order to make this smaller we can first clean
`/rbuild`. The `docker history tmc-langs-cli:0.10.0.4_testmain_r420`
shows two layers that are extra

    <missing>      RUN |1 RUST_CLI_URL=https://download.mooc.fi…   418MB
    <missing>      RUN |1 RUST_CLI_URL=https://download.mooc.fi…   169MB

The one on the top is the layer that builds R-4.2.0 so that would get shaved by
320MB (roughly) since `du -k` shows that

    327976  rbuild/

Making a "monolithic" image which removes the `rbuild/R-4.2.0` is 1.32GB

    REPOSITORY      TAG                        IMAGE ID       SIZE
    tmc-langs-cli   0.10.0.4_testmain_r420     6377b5191216   1.32GB

This is the smallest possible that still has both R 3.6.3 and R 4.2.0
that does not use a multistage image.  If we compare this monolithic
version with the R-3.6.3 version, we see the 300MB increase from 3.6.3
and the 300MB decrease from the previous.

    CREATED BY          SIZE         CREATED BY          SIZE
                                     RUN |1 RUST_CLI_U   71.2MB
    RUN |1 RUST_CLI_U   1.13GB       RUN |1 RUST_CLI_U   770MB
    COPY installer.R    186B
    COPY installer.sh   2.47kB
    RUN |1 RUST_CLI_U   0B           RUN |1 RUST_CLI_U   0B

The drawback with a monolithic Dockerfile is that it is very
uncacheable. By separating two layers (preparation and building) we get

    REPOSITORY      TAG                        IMAGE ID       SIZE
    tmc-langs-cli   0.10.0.4_testmain_r420     6642c5d16227   1.32GB
    tmc-langs-cli   0.10.0.4_testmain_r420b    6377b5191216   1.32GB

so no apparent difference (on this size).

      CREATED BY          SIZE         CREATED BY          SIZE
      RUN |1 RUST_CLI_U   209MB        RUN |1 RUST_CLI_U   1.13GB
      RUN |1 RUST_CLI_U   919MB

There really is no real difference. This way, if we make a change to the
latter, we don't trigger the former.

## Moving these to tmc_langs_rust

The first version did not clean clean the /rbuild directory. It also
included the maven (which took so much time and so many layers) so I
wanted to see the size effects.

    REPOSITORY      TAG                        SIZE
    tmc-langs-cli   0.10.0.4_testmain          4.64GB

The size is huge 4.6GB, so let's see the size without R 4.2.0 and the
tmc R packages at all. This was the time I found the bug (just by adding
the R 4.2.0 in the middle eliminated the bug by obsoleting the need for
backporting R 3.6.3). This version contained also R 3.3.3.

After fixing the bug (see README.md for R 3.6.3 version), the version
with R 3.6.3. Comparing the sizes of the R-3.6.3 and R-4.2.0 branches
show that we should expect that the image would be of size 4.0GB.

The original without Maven parts and without R 4.2.0 is 3.8GB, which
suggests that the Maven parts contribute 200 MB (not tested).

    REPOSITORY      TAG                             SIZE
    tmc-langs-cli   0.10.0.4_testmain_wo_maven_v1   3.83GB

The major contributing layers are

    RUN |1 RUST_CLI_URL=...     44.6MB   running /tmc-langc-cli
    ADD https://...             44.6MB   adding /tmc-langs-cli
    RUN |1 RUST_CLI_URL=...     1.03GB   pip3 install ...
    RUN |1 RUST_CLI_URL=...     71MB     devtools and tmcRtestrunner
    RUN |1 RUST_CLI_URL=...     467MB    Python build
    RUN |1 RUST_CLI_URL=...     133MB    R 3.6.3
    RUN |1 RUST_CLI_URL=...     1.94GB   apt-install -y makedev ...
    /bin/sh -c ADD file:...     101MB    base-image for Strech

The cleaning of Python could trim of 200MB (based on the `du -k
/pythonbuild`). The Maven is estimated to add 200MB. Now we add R-4.2.0
and that added 200MB to the size. The image with

* R-4.2.0 and R-3.6.3 (unnecessary)
* with many potentially unnecessary `-dev` libraries (multistage)

    REPOSITORY      TAG                             SIZE
    tmc-langs-cli   0.10.0.4_testmain_wo_maven_v2   4.03GB
    tmc-langs-cli   0.10.0.4_testmain_wo_maven_v1   3.83GB

The major layers here are

    RUN |1 RUST_CLI_URL=...     44.6MB   running /tmc-langc-cli
    ADD https://...             44.6MB   adding /tmc-langs-cli
    RUN |1 RUST_CLI_URL=...     1.03GB   pip3 install ...
    RUN |1 RUST_CLI_URL=...     468MB    Python build
    RUN |1 RUST_CLI_URL=...     209MB    R-4.2.0 and tmcRtestrunner
    RUN |1 RUST_CLI_URL=...     195MB    R-3.6.3 and -dev libraries
    RUN |1 RUST_CLI_URL=...     1.94GB   apt-install -y makedev ...
    /bin/sh -c ADD file:...     101MB    base-image for Strech

Adding these (using 1 GB = 1000 MB) gives size estimates for v2 and v1 of
4032.20 MB = 4.03 GB and 3831.20 MB = 3.83 GB respectively, which match
the sizes reported by Docker. This, however, would mean that the layers
don't contribute much (or at all), which does not make too much sense.

Using 1 GB = 1024 MB gives estimates 4103.48 MB = 4.00 GB and 3902.48 MB
= 3.81 GB, that means that approximately 0.03 GB = 30.7 MB is coming
from the layers.  The v2 has 24 layers and v1 has 21 layers. The range
that would be reported with 0.03 GB is [0.025, 0.035) GB = [25.60,
35.84) MB. Using these limits give estimates for the layer sizes.

In the v1 the estimates for the layer size are [1.21, 1.70) MB and for
v2 [1.06, 1.49) MB respectively. Thus, the contribution of the layer
should be between 1.21 MB and 1.49 MB.

The number of layers the Maven parts add these are 99 new layers, so
that would contribute between 119.79 MB = 0.11 GB and 147.51 MB = 0.14
GB. So roughly 50 MB is missing from the estimates. One needs to do the
full build to obtain the actual estimates.

The full build with Maven tests is now 4.37GB (so 300 MB down from the
4.64GB), but more than I anticipated.

    REPOSITORY      TAG                             SIZE
    tmc-langs-cli   0.10.0.4                        4.37GB

The layers that contribute are now

    RUN |1 RUST_CLI_URL=...     44.6MB   running /tmc-langc-cli
    ADD https://...             44.6MB   adding /tmc-langs-cli

    RUN |1 RUST_CLI_URL=...     6.88MB
    # maven-template-20
    RUN |1 RUST_CLI_URL=...     1.07MB
    # maven-template-19
    RUN |1 RUST_CLI_URL=...     2.96MB
    RUN |1 RUST_CLI_URL=...     2.14MB
    # maven-template-18
    RUN |1 RUST_CLI_URL=...     4.81MB
    RUN |1 RUST_CLI_URL=...     1.49MB
    # maven-template-17
    RUN |1 RUST_CLI_URL=...     48.3MB
    RUN |1 RUST_CLI_URL=...     60.3MB
    # maven-template-16
    # maven-template-15
    RUN |1 RUST_CLI_URL=...     9.71MB
    RUN |1 RUST_CLI_URL=...     8.19MB
    # maven-template-14
    RUN |1 RUST_CLI_URL=...     5.69MB
    # maven-template-13
    RUN |1 RUST_CLI_URL=...     10.3MB
    # maven-template-12
    RUN |1 RUST_CLI_URL=...     1MB
    # maven-template-11
    RUN |1 RUST_CLI_URL=...     6.37MB
    # maven-template-10
    # maven-template-9
    RUN |1 RUST_CLI_URL=...     1.79MB
    # maven-template-8
    RUN |1 RUST_CLI_URL=...     22.1MB
    # maven-template-7
    RUN |1 RUST_CLI_URL=...     20.8MB
    # maven-template-6
    RUN |1 RUST_CLI_URL=...     2.21MB
    # maven-template-5
    # maven-template-4
    # maven-template-3
    RUN |1 RUST_CLI_URL=...     5.7MB
    RUN |1 RUST_CLI_URL=...     5.06MB
    # maven-template-2
    RUN |1 RUST_CLI_URL=...     4.89MB
    RUN |1 RUST_CLI_URL=...     47.8MB
    RUN |1 RUST_CLI_URL=...     55.7MB
    # maven-template

    RUN |1 RUST_CLI_URL=...     1.03GB   pip3 install ...
    RUN |1 RUST_CLI_URL=...     468MB    Python build
    RUN |1 RUST_CLI_URL=...     209MB    R-4.2.0 and tmcRtestrunner
    RUN |1 RUST_CLI_URL=...     195MB    R-3.6.3 and -dev libraries
    RUN |1 RUST_CLI_URL=...     1.94GB   apt-install -y makedev ...
    /bin/sh -c ADD file:...     101MB    base-image for Strech

The total from the maven layers is 335.26 MB, so using the previous 4103.48 MB
gives the total of 4438.73 MB = 4.33 GB. Since now we have 123 layers,
and we are missing only 0.04 GB (which has the supremum 0.045 GB) which
means that at maximum the overhead of a layer in average is 0.37 MB.

Thus, the idea of having multiple layers contributing a lot only applies
to tiny projects. Most of the cruft is coming from uncleaned build
directories and unnecessary development libraries.

The other explanation is that the overhead depends on the size of the
layer, so using cutpoint 1MB and 30MB we get

            >=30    >=1     <1  oh_low  oh_high
    all     12      19      92  35.84   46.08
    v2      8       0       16  25.60   35.84
    v1      8       0       13  25.60   35.84

By fitting linear models to this, we see that the small layers do not
seem to contribute much to these and it is mostly the large layers.
Using the large layer numbers we get that approximately 3.61 MB overhead
is coming from each large layer by fitting a linear model without
intercept. This is consistent with the estimates that the overhead (if
only large layers contribute) is in range 3.2 MB to 3.84 MB.
