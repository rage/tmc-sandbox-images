# tmc-sandbox-images


## Bump TMC Langs Rust version in images
### Bump tmc-langs-rust

1. Change `RUST_CLI_VER` to new version in `env` file
2. `git add -p` & `git commit`
3. `git push`
4. Check latest tag https://github.com/rage/tmc-sandbox-images/tags
5. `git tag -a rust-vX.Y.Z`
6. `git push --tags`
7. Check GitHub Actions tab (may take upto 40 minutes), when finished sandboxes will pull the image within 15 minutes

### Bump cyber-security-base

1. Do above to step 4 (if you didn't already), use tag `git tag -a cyber-security-base-vXY`
2. `git push --tags`