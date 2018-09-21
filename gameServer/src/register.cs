using Google.Protobuf;
using Grpc.Core;
using Stream;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Register
{
    static uint reserved = 0;
    readonly uint maxReserved = 1000;

    readonly RegConfig reg;
    public uint Load
    {
        get;
        private set;
    }
    public bool Stopped {
        get;
        private set;
    }
    readonly UInt32 userID;

    CSStream.CSStreamClient client;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="regconf"></param>
    public Register(RegConfig regconf)
    {
        reg = regconf;
        string serverTag = regconf.SvcName + regconf.PodName;
        userID = CRC32Utils.GetCRC32(serverTag);
    }


    /// <summary>
    /// 连接服务器
    /// </summary>
    private void Connect()
    {
        string remoteAddr = reg.RemoteHost + ":" + reg.RemotePort;
        Channel channel = new Channel(remoteAddr, ChannelCredentials.Insecure);
        client = new CSStream.CSStreamClient(channel);
    }
    
    /// <summary>
    /// 启动注册
    /// </summary>
    public void Run()
    {
        Connect();
        Stream();
    }

    /// <summary>
    /// 设置当前gameServer负载
    /// </summary>
    /// <param name="load">当前gameServer负载情况，取值范围（0，100）</param>
    public void SetLoad(uint load)
    {
        if (load >= 0 && load <= 100)
        {
            this.Load = load;
        }
    }

    /// <summary>
    /// 停止服务注册
    /// </summary>
    public void Stop()
    {
        Stopped = true;
    }

    /// <summary>
    /// 向directory服务注册gameServer
    /// </summary>
    private async void Stream()
    {
        Metadata meta = new Metadata
        {
            { "userid", userID + "" },
            { "token", "" }
        };
        try
        {
            using (var call = client.Stream(meta))
            {
                var resp = Task.Run(async () =>
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        //var frame = call.ResponseStream.Current;
                    }
                });
                await call.RequestStream.WriteAsync(LoginMsg());
                while (true)
                {
                    if (Stopped)
                    {
                        await call.RequestStream.WriteAsync(LogoutMsg());
                        break;
                    }
                    await call.RequestStream.WriteAsync(HeartbeatMsg());
                    System.Threading.Thread.Sleep(5000);
                }
                await call.RequestStream.CompleteAsync();
                await resp;
            }
        }
        catch (RpcException e)
        {
            Logger.Error("catch error {0}", e);
            Connect();
            System.Threading.Thread.Sleep(5000);
            Stream();
        }
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
    /// 注册消息
    /// </summary>
    /// <returns></returns>
    private Package.Types.Frame LoginMsg()
    {
        var login = new GSLogin()
        {
            GameID = reg.GameID,
            SvcName = reg.SvcName,
            PodName = reg.PodName,
            Host = reg.LocalHost,
            Port = reg.LocalPort,
        };
        return FrameMsg(ByteUtils.ObjectToByteString(login), (UInt32)GSDirectoryCmdID.GsloginCmd);
    }

    /// <summary>
    /// 注销消息
    /// </summary>
    /// <returns></returns>
    private Package.Types.Frame LogoutMsg()
    {
        var logout = new GSLogout()
        {
            GameID = reg.GameID,
            SvcName = reg.SvcName,
            PodName = reg.PodName
        };
        return FrameMsg(ByteUtils.ObjectToByteString(logout), (UInt32)GSDirectoryCmdID.GslogoutCmd);
    }

    /// <summary>
    /// 心跳消息
    /// </summary>
    /// <returns></returns>
    private Package.Types.Frame HeartbeatMsg()
    {
        this.Load = 1;
        var heartbeat = new GSHeartbeat()
        {
            Load = this.Load
        };
        return FrameMsg(ByteUtils.ObjectToByteString(heartbeat), (UInt32)GSDirectoryCmdID.GsheartbeatCmd);
    }

    /// <summary>
    /// 消息打包
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cmd"></param>
    /// <returns></returns>
    private Package.Types.Frame FrameMsg(ByteString message, UInt32 cmd)
    {
        var req = new Package.Types.Frame()
        {
            Type = Package.Types.FrameType.LeagueMessage,
            Version = 2,
            CmdId = cmd,
            UserId = userID,
            Reserved = GetReverse(),
            Message = message
        };
        return req;
    }
}
