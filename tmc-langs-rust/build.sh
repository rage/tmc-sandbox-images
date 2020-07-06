source ../env
docker build . -t ${RUST_CLI_TAG} --build-arg "RUST_CLI_URL=${RUST_CLI_URL}"
