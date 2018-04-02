#!/bin/sh

set -x

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

cd ${service_data_path}/script

# uninstall
./uninstall.sh

#修改配置文件

#测试配置文件有效性
./testconfig.sh

#备份数据
./backup.sh

if [ ! -d ${service_path} ];then
    sudo mkdir -p ${service_path}
    sudo mkdir ${service_path}/script
    sudo mkdir ${service_path}/bin
    sudo mkdir ${service_path}/conf
fi

#更新工作目录
for dir in log data backup; do 
    if [ ! -L ${service_path}/$dir ];then
        sudo mkdir ${service_data_path}/$dir
        sudo rm -f ${service_path}/${dir}
        sudo ln -s ${service_data_path}/${dir} ${service_path}/${dir}
    fi
done

sudo cp -r ${service_data_path}/script/ ${service_path}/
sudo cp -r ${service_data_path}/conf/ ${service_path}/
if [ -f ${service_data_path}/README.md ]; then
    sudo cp ${service_data_path}/README.md ${service_path}/README.md
fi

sudo ${service_path}/script/stop.sh
sudo cp -r ${service_data_path}/bin ${service_path}/
sudo ${service_path}/script/start.sh

#更新logrotate
sed -i "s,matchvs,${service_name}," ${service_data_path}/script/logrotate.example
sudo cp ${service_data_path}/script/logrotate.example /etc/logrotate.d/${service_name}

bin_name="gameServer.dll"

# 更新crontab

num=`sudo crontab -l | grep "check_process.*${bin_name}" | grep -v grep | wc -l`
if [ $num -lt 1 ]
then
    sudo crontab -l > /tmp/crontab.$$
    sudo echo "* * * * * ${service_path}/script/check_process.sh ${bin_name} \"$service_path/bin/${bin_name}\"" >> /tmp/crontab.$$
    sudo crontab /tmp/crontab.$$
fi

