/*******************************************************************
** 文件名:	Gvalue
** 版  权:	(C)  2017 - 掌玩
** 创建人:	ZJ
** 日  期:	2017/09/06
** 版  本:	1.0
** 描  述:	
** 应  用:  全局定义常量

**************************** 修改记录 ******************************
** 修改人: ZJ
** 日  期: 2018/01/29
** 描  述: 删除无用信息
********************************************************************/
using System;
using System.Collections.Generic;

public class Gvalue
{
    // CmdHeartbeat 心跳
    public static uint CmdHeartbeat = 99999999;
}
/// <summary>
/// gs配置文件
/// </summary>
public class Gsconfig
{
    public int LogLevel { get; set; }
    public string HostIp { get; set; }
    public string HostPort { get; set; }
}


