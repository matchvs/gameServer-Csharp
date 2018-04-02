#!/bin/sh

private_token=BU1qLzSjvsRKHqFNyfA9
group=matchvs
project=robot
version=v1.0.5
zip=${project}_${version}.zip

url=http://115.231.9.78/$group/$project/builds/artifacts/$version/download?job=release

curl --header "PRIVATE-TOKEN: $private_token" -L $url -o $zip
