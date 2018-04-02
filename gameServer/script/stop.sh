#!/bin/sh

set -x
cur_dir=$(cd "$(dirname "$0")"; pwd)
root_dir=`dirname ${cur_dir}/`
service_name="${root_dir##*/}"
bin_name="gameServer.dll"

pid=`cat $root_dir/${bin_name}_pid`
kill -9 $pid
