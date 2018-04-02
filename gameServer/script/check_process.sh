#!/bin/sh
set -x
#filename: checkProcess.sh
#示例：每分钟检测httpd是否在运行，不在运行则重启
#crontab -e
# 加入：* * * * * /usr/local/zhangwan/${service_name}/script/check_process.sh "elastalert" "/usr/local/bin/elastalert --verbose --config /etc/elastalert.yaml"
#-------------------------------
# 函数: checkProcess
# 功能: 检查一个进程是否存在
# 参数: $1 --- 要检查的进程名称（可以加入路径，增强唯一性）
# 返回: 如果存在返回1, 否则返回0.
#-------------------------------
checkProcess()
{
    # 检查输入的参数是否有效
    if [ "$1" = "" -o ! -f $1 ]
    then
        return 0
    fi

    # get pid
    pid=`cat $1`

    #实际进程数PROCESS_NUM小于判断为运行中的数IS_RUNNING_NUM，表示有错误，需要重新启动
    if [ ! -d /proc/$pid  ];
    then
        return 0
    else
        return 1
    fi

}

#如果参数1或者参数2为空，提示用法
if [ -z "$1" -o -z "$2"  ]
then
    echo "Usage: check_process.sh process execCommand"
    echo 'Example: check_process.sh "httpd" "/etc/init.d/httpd start"'
    exit
fi

# get service_name
cur_dir=$(cd "$(dirname "$0")"; pwd)
root_dir=`dirname ${cur_dir}/`
service_name="${root_dir##*/}"

# 检查test实例是否已经存在
checkProcess $root_dir/$1_pid
checkResult=$?
t=`date +"%Y-%m-%d %H:%M:%S"` 
mkdir -p $root_dir/log/
if [ $checkResult -eq 0  ];
then
    # 杀死所有test进程，可换任意你需要执行的操作
    echo "[$t] [ERRO] process($1) restart" >> $root_dir/log/launch_$1.log
    # 在后台执行程序
    echo "nohup dotnet $2& 1>>$root_dir/log/launch_$1.log 2>&1 &"
    nohup dotnet $2& 1>>$root_dir/log/launch_$1.log 2>&1 &
else
    echo "[$t] [INFO] process($1) is running" >> $root_dir/log/launch_$1.log
fi
