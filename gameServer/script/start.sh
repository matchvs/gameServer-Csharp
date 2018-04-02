#!/bin/sh

set -x
# you need exe this script in /usr/local/zhangwan/service_name/script/ dir""
cur_dir=$(cd "$(dirname "$0")"; pwd)
root_dir=`dirname ${cur_dir}/`
service_name="${root_dir##*/}"
bin_name="gameServer.dll"

$root_dir/script/check_process.sh $bin_name "$root_dir/bin/$bin_name"
