/*******************************************************************
** 文件名:	StreamServer
** 版  权:	(C)  2017 - 掌玩
** 创建人:	ZJ
** 日  期:	2017/08/29
** 版  本:	1.0
** 描  述:	
** 应  用:  stream 收发服务端

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
********************************************************************/
using Grpc.Core;
using Grpc.Core.Utils;
using Stream;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class StreamServer : CSStream.CSStreamBase
{
    private BaseServer baseServer;
    private Server server;
    IServerStreamWriter<Package.Types.Frame> responseStream = null;  
    private Dictionary<UInt64, IServerStreamWriter<Package.Types.Frame>> hotelMap = 
        new Dictionary<UInt64, IServerStreamWriter<Package.Types.Frame>>();
    public StreamServer(BaseServer driver, string hostIp, int hostPort)
    {
        server = new Server()
        {
            Services = { CSStream.BindService(this) },
            Ports = { new ServerPort(hostIp, hostPort, ServerCredentials.Insecure) }
        };

        baseServer = driver;
    }
 
    public void Run()
    {
        server.Start();
    }
    public void WaitOver()
    {
        AwaitCancellation().Wait();
    }
    private Task AwaitCancellation()
    {
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        var taskSource = new TaskCompletionSource<bool>();
        tokenSource.Token.Register(() => taskSource.SetResult(true));
        return taskSource.Task;
    }
    /// <summary>
    /// Metadata里面可以做一些合法连接校验
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userID"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private bool GetMetadata(ServerCallContext context, out UInt32 userID, out string token)
    {
        Metadata md = context.RequestHeaders;
        userID = 1;
        token = string.Empty;

        if (md.Count > 1)           //数组从1开始,0的Key=user-agent
        {
            if (md[1].Key == "userid")
            {
                userID = UInt32.Parse(md[1].Value);
            }
            if (md.Count > 2)
            {
                if (md[2] != null && md[2].Key == "token")
                {
                    token = md[2].Value;
                }
            }
            return true;
        }
        return false;
    }
    public override async Task Stream(IAsyncStreamReader<Package.Types.Frame> requestStream, IServerStreamWriter<Package.Types.Frame> responseStream, ServerCallContext context)
    {
        if (GetMetadata(context, out uint userID, out string token) == false)
        {
            Logger.Error("Invalid connection");
            return;
        }
        baseServer.Connect(userID, token);

        try
        {
            while (await requestStream.MoveNext(CancellationToken.None))          
            {
                Package.Types.Frame p = (Package.Types.Frame)requestStream.Current;
                var responseTask = Task.Run(async () =>             
                {
                    Package.Types.Frame rep = null;
                    if (p.CmdId == Gvalue.CmdHeartbeat)
                    {
                        rep = p;
                    }
                    else
                    {
                        if (p.CmdId == (UInt32)HotelGsCmdID.HotelCreateConnect
                            || p.CmdId == (UInt32)HotelGsCmdID.HotelBroadcastCmdid
                            || p.CmdId == (UInt32)HotelGsCmdID.HotelCloseConnet)
                        {
                            if (p.CmdId == (UInt32)HotelGsCmdID.HotelCreateConnect)
                            {
                                Connect connectInfo = Connect.Parser.ParseFrom(p.Message);
                                hotelMap.Add(connectInfo.RoomID, responseStream);
                            }
                        }
                        else
                        {
                            if (this.responseStream == null)    //MVS消息只用一个channel返回
                            {
                                this.responseStream = responseStream;
                            }
                        }
                        rep = baseServer.DealMsg(p);
                    }

                    if (rep != null)
                    {
                        await responseStream.WriteAsync(rep);
                    }
                });

                await responseTask;                 
            }
            Logger.Info("Rcv package over!");
        }
        catch (RpcException e)
        {
            Logger.Info("e:", e);
        }

        Logger.Info("requestTask over!");
        baseServer.Disconnect(userID, token);
    }
    /// <summary>
    /// 推送给MVS
    /// </summary>
    /// <param name="package"></param>
    /// <returns></returns>
    public async Task PushToMvs(Package.Types.Frame package)
    {
        Logger.Debug("PushToMvs");
        await this.responseStream.WriteAsync(package);
    }
    /// <summary>
    /// 推送给指定的hotel
    /// </summary>
    /// <param name="roomID"></param>
    /// <param name="package"></param>
    /// <returns></returns>
    public async Task PushToHotel(UInt64 roomID, Package.Types.Frame package)
    {
        if (hotelMap.TryGetValue(roomID, out IServerStreamWriter<Package.Types.Frame> response))
        {
            Logger.Debug("PushToHotel roomID={0}", roomID);
            await response.WriteAsync(package);
            return;
        }

        Logger.Warn("PushToHotel failed, roomID={0}", roomID);
    }
    /// <summary>
    /// 删除hotel的stream映射关系
    /// </summary>
    /// <param name="roomID"></param>
    public void DeleteStreamMap(UInt64 roomID)
    {
        Logger.Debug("DeleteStreamMap roomID={0}", roomID);
        hotelMap.Remove(roomID);
    }
    /// <summary>
    /// 释放gRPC服务
    /// </summary>
    public void Dispose()
    {
        server.ShutdownAsync().Wait();
    }
}
