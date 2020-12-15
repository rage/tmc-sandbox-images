source ../env
docker build . -t csharp:0.1 --build-arg "RUST_CLI_URL=${RUST_CLI_URL}"