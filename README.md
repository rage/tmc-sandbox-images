# tmc-sandbox-images

## Creating a new version for a specific image

Create a new tag `DIR-MAJOR.MINOR` to create a new version for the `tmc-sandbox-DIR` image. For example, to create a new version `1.2` for the image in the `python` directory, create the tag `python-1.2`.

The new version should have the `MINOR` version incremented. For example, if the current version of `tmc-sandbox-python` is `1.2`, the new version should be `1.3`.

## Creating a new version for all images

Create a tag with the format `all-MAJOR.0` where the `MAJOR` is incremented by one from the current versions.

## Updating Python

1. Change the `SANDBOX_PYTHON_VERSION` variable to a new version in the `env` file
2. Create a new tag(s) as instructed in the section above.

## Updating tmc-langs-rust

1. Change the `RUST_CLI_VER` variable to a new version in the `env` file
2. Create a new `all-MAJOR.0` tag as instructed in the section above.

## After creating a new version

You can check the progress at https://github.com/rage/tmc-sandbox-images/actions. Creating the images may take up to 40 minutes. When finished sandboxes will pull the images within 15 minutes.
