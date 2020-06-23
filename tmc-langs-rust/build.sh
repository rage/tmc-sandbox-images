source ../env
docker build -t tmc-langs-rust:${RUST_CLI_VER} --build-arg cli_url=${RUST_CLI_URL} .
