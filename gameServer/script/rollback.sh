#!/bin/sh

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

cd ${backup_path}

#删除最新的安装包
sudo ls -t | awk '{if(NR<2){print $0}}' |xargs rm -f

#恢复上一版本
pname=`ls -t | awk '{if(NR<2){print $0}}'`
if [ -z "${pname}" ];then
	echo "pkg not exist!"
	exit
fi

sudo unzip -o ${pname} -d ${service_data_path}
cd ${service_data_path}
sudo cp -r build/* .

sudo ./script/install.sh
