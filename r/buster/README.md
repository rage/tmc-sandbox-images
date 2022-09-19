## Dockerfile for R 4.2.1 on Buster

This is nearly trivial, since R 4.2.1 is backported to Buster. Only the
new libraries (libharfbuzz-dev libfribidi-dev) that are needed for
`textshaping` with R 4.2.1 needs to be added.

