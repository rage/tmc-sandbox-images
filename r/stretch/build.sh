source ../../env
docker build --progress=plain . -t ${RUST_CLI_TAG}_testmain_r421  \
             --build-arg "RUST_CLI_URL=${RUST_CLI_URL}"     2>&1 |    \
             tee testing-main-dockerfile-build-R421.log
