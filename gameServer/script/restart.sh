#!/bin/bash
  
echo ">> restart begin ..."
cur_dir=$(cd "$(dirname "$0")"; pwd)

$cur_dir/stop.sh
$cur_dir/start.sh

echo ">> restart end"
