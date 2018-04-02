#!/bin/sh

set -x

cur_dir=$(cd "$(dirname "$0")"; pwd)
root_dir=`dirname ${cur_dir}/`
service_name="${root_dir##*/}"
service_data_path="/data/zhangwan/${service_name}"
backup_path="${service_data_path}/backup/bin"

#判断执行目录是否是/data/zhangwan/{service_name}
if [ ${root_dir} != ${service_data_path} ];then
	echo "script should be executed in /data/zhangwan/{service_name}"
	exit
fi

#备份目录不存在时创建目录
if [ ! -d ${backup_path} ];then
	sudo mkdir -p ${backup_path}
fi

sudo mv ${service_data_path}/*.zip $backup_path

#删除老备份，仅保留上2次的，即一共3份
cd ${backup_path}
sudo ls -t | awk '{if(NR>3){print $0}}' |sudo xargs rm -f
cd -
