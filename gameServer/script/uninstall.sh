#!/bin/sh

cur_dir=$(cd "$(dirname "$0")"; pwd)
root_dir=`dirname ${cur_dir}/`
service_name="${root_dir##*/}"
service_data_path="/data/zhangwan/${service_name}"
service_path="/usr/local/zhangwan/${service_name}"

#判断执行目录是否是/data/zhangwan/{service_name}
if [ ${root_dir} != ${service_data_path} ];then
	echo "script should be executed in /data/zhangwan/{service_name}"
	exit
fi


# 去除crontab中的定时任务
for bin in `ls $service_path/bin` 
do 
    num=`sudo crontab -l | grep "check_process.*$bin" | grep -v grep | wc -l`
    if [ $num -ge 1 ]
    then
        sudo crontab -l > /tmp/crontab.$$
        sudo sed -i "/check_process.*$bin/d" /tmp/crontab.$$
        sudo crontab /tmp/crontab.$$
    fi
done
