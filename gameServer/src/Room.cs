using Google.Protobuf;
using Grpc.Core;
using Stream;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RoomManager
{
    static uint reserved = 0;
    readonly uint maxReserved = 1000;

    readonly UInt32 userID;
    readonly string podName;
    readonly string svcName;
    readonly string remoteHost;
    readonly string remotePort;

    SimpleService.SimpleServiceClient client;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="filePath"></param>
    public RoomManager(string filePath)
    {
        var confData = JsonUtils.DecodeCps(filePath);
        svcName = Environment.GetEnvironmentVariable("DIRECTORY_SVC_NAME");
        if(svcName != null && svcName.Length != 0) 
        {
            podName = Environment.GetEnvironmentVariable("DIRECTORY_POD_NAME");
            remoteHost = Environment.GetEnvironmentVariable("DIRECTORY_REMOTE_HOST");
            remotePort = Environment.GetEnvironmentVariable("DIRECTORY_REMOTE_PORT");
        }
        else if (confData.RoomConf.Enable)
        {
            svcName = confData.RoomConf.SvcName;
            podName = confData.RoomConf.PodName;
            remoteHost = confData.RoomConf.RemoteHost;
            remotePort = confData.RoomConf.RemotePort.ToString();
        }
        else
        {
            return;
        }
        string serverTag = svcName + podName;
        userID = CRC32Utils.GetCRC32(serverTag);
        Logger.Debug("svcName: {0}, podName: {1}, remoteHost: {2}, remotePort: {3}, userID: {4}", svcName, podName, remoteHost, remotePort, userID);
        Connect();
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    private void Connect()
    {
        string remoteAddr = remoteHost + ":" + remotePort;
        Channel channel = new Channel(remoteAddr, ChannelCredentials.Insecure);
        client = new SimpleService.SimpleServiceClient(channel);
    }
    
    /// <summary>
    /// 生成序列号
    /// </summary>
    /// <returns></returns>
    private uint GetReverse()
    {
        reserved++;
        if (reserved > maxReserved)
        {
            reserved = 1;
        }
        return reserved;
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cmd"></param>
    /// <returns></returns>
    private Package.Types.Frame SendMsg(ByteString message, UInt32 cmd)
    {
        var request = new Package.Types.Frame()
        {
            Type = Package.Types.FrameType.LeagueMessage,
            Version = 2,
            CmdId = cmd,
            UserId = userID,
            Reserved = GetReverse(),
            Message = message
        };
        Package.Types.Frame reply;
        try
        {
            reply = client.SimpleRequest(request);
        }
        catch (RpcException e)
        {
            Logger.Warn("Send message error: {0}", e.Status);
            return null;
        }
        return reply;
    }

    public CreateRoomAck CreateRoom(CreateRoom msg)
    {
        CreateRoomAck rsp = new CreateRoomAck();
        if (client == null)
        {
            Logger.Warn("Not connected to matchvs server");
            rsp.Status = 400;
            return rsp;
        }
        msg.SvcName = svcName;
        msg.PodName = podName;

        var reply = SendMsg(ByteUtils.ObjectToByteString(msg), (UInt32)GSDirectoryCmdID.GscreateRoomCmd);
        if (reply == null)
        {
            rsp.Status = 500;
        }
        else
        {
            rsp = new CreateRoomAck();
            ByteUtils.ByteStringToObject(rsp, reply.Message);
        }
        return rsp;
    }

    public TouchRoomAck TouchRoom(TouchRoom msg)
    {
        TouchRoomAck rsp = new TouchRoomAck();
        if (client == null)
        {
            Logger.Warn("Not connected to matchvs server");
            rsp.Status = 400;
            return rsp;
        }
        msg.SvcName = svcName;
        msg.PodName = podName;

        var reply = SendMsg(ByteUtils.ObjectToByteString(msg), (UInt32)GSDirectoryCmdID.GstouchRoomCmd);
        if (reply == null)
        {
            rsp.Status = 500;
        }
        else
        {
            rsp = new TouchRoomAck();
            ByteUtils.ByteStringToObject(rsp, reply.Message);
        }
        return rsp;
    }

    public DestroyRoomAck DestroyRoom(DestroyRoom msg)
    {
        DestroyRoomAck rsp = new DestroyRoomAck();
        if (client == null)
        {
            Logger.Warn("Not connected to matchvs server");
            rsp.Status = 400;
            return rsp;
        }
        msg.SvcName = svcName;
        msg.PodName = podName;

        var reply = SendMsg(ByteUtils.ObjectToByteString(msg), (UInt32)GSDirectoryCmdID.GsdestroyRoomCmd);
        if (reply == null)
        {
            rsp.Status = 500;
        }
        else
        {
            rsp = new DestroyRoomAck();
            ByteUtils.ByteStringToObject(rsp, reply.Message);
        }
        return rsp;
    }
}
