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
using Google.Protobuf;

public class Gvalue
{
    // CmdHeartbeat 心跳
    public static uint CmdHeartbeat = 99999999;
}

public class RegConfig
{
    public bool Enable { get; set; }
    public uint GameID { get; set; }
    public string SvcName { get; set; }
    public string PodName { get; set; }
    public string RemoteHost { get; set; }
    public uint RemotePort { get; set; }
    public string LocalHost { get; set; }
    public uint LocalPort { get; set; }
}
public class RoomConfig
{
    public bool Enable { get; set; }
    public string SvcName { get; set; }
    public string PodName { get; set; }
    public string RemoteHost { get; set; }
    public uint RemotePort { get; set; }
}
/// <summary>
/// gs配置文件
/// </summary>
public class Gsconfig
{
    public int LogLevel { get; set; }
    public string HostIp { get; set; }
    public uint HostPort { get; set; }
    public RegConfig RegConf { get; set; }
    public RoomConfig RoomConf { get; set; }
}

public class FrameSyncRate
{
    public UInt32 GameID { get; set; }
    public UInt64 RoomID { get; set; }
    public UInt32 FrameRate { get; set; }
    public UInt32 FrameIndex { get; set; }
    public UInt64 Timestamp { get; set; }
    public UInt32 EnableGS { get; set; }
}

public class FrameItem
{
    public UInt32 SrcUserID { get; set; }
    public ByteString CpProto { get; set; }
    public UInt64 Timestamp { get; set; }
}

public class FrameData
{
    public UInt32 GameID { get; set; }
    public UInt64 RoomID { get; set; }
    public UInt32 FrameIndex { get; set;}
    public List<FrameItem> FrameItems { get; set; }
    public int FrameWaitCount { get; set;}
}
