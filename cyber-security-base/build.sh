source ../env
docker build . -t cybersecuritybase:0.2 --build-arg "RUST_CLI_URL=${RUST_CLI_URL}"
