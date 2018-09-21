#!/bin/bash

# protoc -I ./ --csharp_out=plugins=grpc:. *.proto

PROTOC=~/.nuget/packages/grpc.tools/1.4.1/tools/macosx_x64/protoc
PLUGIN=~/.nuget/packages/grpc.tools/1.4.1/tools/macosx_x64/grpc_csharp_plugin


$PROTOC  -I ./  --csharp_out ../ *.proto --grpc_out ../ --plugin=protoc-gen-grpc=$PLUGIN