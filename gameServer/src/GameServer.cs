/*******************************************************************
** 文件名:	GameServer
** 版  权:	(C)  2018 - 掌玩
** 创建人:	ZJ
** 日  期:	2018/01/29
** 版  本:	1.0
** 描  述:	
** 应  用:  gs服务

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
********************************************************************/
using Google.Protobuf;
using Newtonsoft.Json;
using Stream;
using System;
using System.Collections.Generic;

public class GameServer : BaseServer
{
    private BaseHandler msgHandler;
    private List<UInt32> connections = new List<UInt32>();
    private StreamServer serverImp;

    public Gsconfig ConfData
    {
        get;
        private set;
    }
    public void Init(string filePath)
    {
        ConfData = JsonUtils.DecodeCps(filePath);
        Logger.SetLevel(ConfData.LogLevel);

    }
    public void Bind(BaseHandler msgHandler)
    {
        this.msgHandler = msgHandler;
    }
    public override void Run()
    {
        Logger.Info("GameServer Run! listening hostIP:{0}, port:{1}", ConfData.HostIp, ConfData.HostPort);
        serverImp = new StreamServer(this, ConfData.HostIp, Convert.ToInt32(ConfData.HostPort));
        serverImp.Run();
    }
    public override void WaitOver()
    {
        if (serverImp != null)
        {
            serverImp.WaitOver();
            serverImp.Dispose();
        }
    }
    public override void Connect(UInt32 userID, string token)
    {
        Logger.Info("recv a connect event uid:{0}, token:{1}", userID, token);
        connections.Add(userID);
    }
    public override void Disconnect(UInt32 userID, string token)
    {
        Logger.Info("recv a disconnect event uid:{0}, token:{1}", userID, token);
        connections.Remove(userID);
    }
    public override Package.Types.Frame DealMsg(Package.Types.Frame req)
    {
        Logger.Info("DealMsg, CmdID={0}, UserID={1}", req.CmdId, req.UserId);

        IMessage reply = null;
        Package.Types.Frame response = req;
        
        if (req.CmdId == (UInt32)HotelGsCmdID.HotelCreateConnect)
        {
            reply = msgHandler.OnHotelConnect(req.Message);
        }
        else if (req.CmdId == (UInt32)HotelGsCmdID.HotelBroadcastCmdid)
        {
            reply = msgHandler.OnHotelBroadCast(req.Message);
        }
        else if (req.CmdId == (UInt32)HotelGsCmdID.HotelCloseConnet)
        {
            reply = msgHandler.OnHotelCloseConnect(req.Message);
        }
        else if (req.CmdId == (UInt32)MvsGsCmdID.MvsJoinRoomReq)
        {
            reply = msgHandler.OnJoinRoom(req.Message);
        }
        else if (req.CmdId == (UInt32)MvsGsCmdID.MvsCreateRoomReq)
        {
            reply = msgHandler.OnCreateRoom(req.Message);
        }
        else if (req.CmdId == (UInt32)MvsGsCmdID.MvsLeaveRoomReq)
        {
            reply = msgHandler.OnLeaveRoom(req.Message);
        }
        else if (req.CmdId == (UInt32)MvsGsCmdID.MvsJoinOverReq)
        {
            reply = msgHandler.OnJoinOver(req.Message);
        }
        else if (req.CmdId == (UInt32)MvsGsCmdID.MvsKickPlayerReq)
        {
            reply = msgHandler.OnKickPlayer(req.Message);
        }
        else if (req.CmdId == (UInt32)MvsGsCmdID.MvsNetworkStateReq)
        {
            reply = msgHandler.OnConnectStatus(req.Message);
        }
        else if (req.CmdId == (UInt32)MvsGsCmdID.MvsGetRoomDetailPush)
        {
            msgHandler.OnRoomDetail(req.Message);
        }
        else
        {
            reply = new Reply()
            {
                Errno = ErrorCode.NotImplemented,
                ErrMsg = string.Format("not found the cmdid:{0}", req.CmdId),
            };
        }

        response.CmdId += 1;
        response.Message = ByteUtils.ObjectToByteString(reply);

        return response;
    }

    /// <summary>
    /// 推送消息给MVS
    /// </summary>
    /// <param name="msg"></param>
    public override void PushToMvs(UInt32 userID, UInt32 version, UInt32 cmdID, IMessage msg)
    {
        Package.Types.Frame package = new Package.Types.Frame()
        {
            Type = Package.Types.FrameType.PushMessage,
            Version = version,
            CmdId = cmdID,
            UserId = userID,
            Message = ByteUtils.ObjectToByteString(msg)
        };
        serverImp.PushToMvs(package).Wait();
    }
    /// <summary>
    /// 推送消息给Hotel
    /// </summary>
    /// <param name="msg"></param>
    public override void PushToHotel(UInt32 userID, UInt32 version, UInt64 roomID, UInt32 cmdID, IMessage msg)
    {
        Package.Types.Frame package = new Package.Types.Frame()
        {
            Type = Package.Types.FrameType.PushMessage,
            Version = version,
            CmdId = cmdID,
            UserId = userID,
            Message = ByteUtils.ObjectToByteString(msg)
        };
        serverImp.PushToHotel(roomID, package).Wait();
    }
    /// <summary>
    /// 删除stream映射
    /// </summary>
    /// <param name="roomID"></param>
    public override void DeleteStreamMap(UInt64 roomID)
    {
        serverImp.DeleteStreamMap(roomID);
    }
}
