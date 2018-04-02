/*******************************************************************
** 文件名:	FightHandler
** 版  权:	(C)  2018 - 掌玩
** 创建人:	ZJ
** 日  期:	2018/01/30
** 版  本:	1.0
** 描  述:	
** 应  用:  战斗示例

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
********************************************************************/
using System;
using Stream;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;

public class FightHandler : BaseHandler
{
    private BaseServer baseServer;
    public FightHandler(BaseServer server)
    {
        baseServer = server;
    }
    /// <summary>
    /// 创建房间
    /// </summary>
    /// <param name="msg"></param>
    public override IMessage OnCreateRoom(ByteString msg)
    {
        Request request = new Request();
        ByteUtils.ByteStringToObject(request, msg);

        Reply reply = new Reply()
        {
            UserID = request.UserID,
            GameID = request.GameID,
            RoomID = request.RoomID,
            Errno = ErrorCode.Ok,
            ErrMsg = "OnCreateRoom success"
        };

        Logger.Info("OnCreateRoom start, userId={0}, gameId={1}, roomId={2}", request.UserID, request.GameID, request.RoomID);
        
        CreateExtInfo createEx = new CreateExtInfo();
        ByteUtils.ByteStringToObject(createEx, request.CpProto);
        Logger.Info("OnCreateRoom CreateExtInfo, userId={0}, roomId={1}, state={2}, CreateTime={3}", createEx.UserID, createEx.RoomID, createEx.State, createEx.CreateTime);

        return reply;
    }
    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="msg"></param>
    public override IMessage OnJoinRoom(ByteString msg)
    {
        Request request = new Request();
        ByteUtils.ByteStringToObject(request, msg);

        Reply reply = new Reply()
        {
            UserID = request.UserID,
            GameID = request.GameID,
            RoomID = request.RoomID,
            Errno = ErrorCode.Ok,
            ErrMsg = "OnJoinRoom success"
        };

        Logger.Info("OnJoinRoom start, userId={0}, gameId={1}, roomId={2}", request.UserID, request.GameID, request.RoomID);

        JoinExtInfo joinEx = new JoinExtInfo();
        ByteUtils.ByteStringToObject(joinEx, request.CpProto);
        Logger.Info("OnJoinRoom JoinExtInfo, userId={0}, roomId={1}, JoinType ={2}", joinEx.UserID, joinEx.RoomID, joinEx.JoinType);

        return reply;
    }
    /// <summary>
    /// 加入房间Over
    /// </summary>
    /// <param name="msg"></param>
    public override IMessage OnJoinOver(ByteString msg)
    {
        Request request = new Request();
        ByteUtils.ByteStringToObject(request, msg);

        Reply reply = new Reply()
        {
            UserID = request.UserID,
            GameID = request.GameID,
            RoomID = request.RoomID,
            Errno = ErrorCode.Ok,
            ErrMsg = "OnJoinOver success"
        };

        Logger.Info("OnJoinOver start, userId={0}, gameId={1}, roomId={2}", request.UserID, request.GameID, request.RoomID);

        return reply;
    }
    /// <summary>
    /// 离开房间
    /// </summary>
    /// <param name="msg"></param>
    public override IMessage OnLeaveRoom(ByteString msg)
    {
        Request request = new Request();
        ByteUtils.ByteStringToObject(request, msg);

        Reply reply = new Reply()
        {
            UserID = request.UserID,
            GameID = request.GameID,
            RoomID = request.RoomID,
            Errno = ErrorCode.Ok,
            ErrMsg = "OnLeaveRoom success"
        };

        Logger.Info("OnLeaveRoom start, userId={0}, gameId={1}, roomId={2}", request.UserID, request.GameID, request.RoomID);

        return reply;
    }

    /// <summary>
    /// 踢人
    /// </summary>
    /// <param name="msg"></param>
    public override IMessage OnKickPlayer(ByteString msg)
    {
        Request request = new Request();
        ByteUtils.ByteStringToObject(request, msg);

        Reply reply = new Reply()
        {
            UserID = request.UserID,
            GameID = request.GameID,
            RoomID = request.RoomID,
            Errno = ErrorCode.Ok,
            ErrMsg = "OnKickPlayer success"
        };

        Logger.Info("OnKickPlayer start, userId={0}, gameId={1}, roomId={2}", request.UserID, request.GameID, request.RoomID);

        return reply;
    }
    /// <summary>
    /// 连接状态
    /// </summary>
    /// <param name="msg"></param>
    public override IMessage OnConnectStatus(ByteString msg)
    {
        Request request = new Request();
        ByteUtils.ByteStringToObject(request, msg);

        Reply reply = new Reply()
        {
            UserID = request.UserID,
            GameID = request.GameID,
            RoomID = request.RoomID,
            Errno = ErrorCode.Ok,
            ErrMsg = "OnConnectStatus success"
        };
        string status = request.CpProto.ToStringUtf8();

        Logger.Info("OnConnectStatus start, userId={0}, gameId={1}, roomId={2}, status = {3}", request.UserID, request.GameID, request.RoomID, status);

        //1.掉线了  2.重连成功  3.重连失败
        if (status == "3")
        {
            Logger.Info("OnConnectStatus leaveroom, userId={0}, gameId={1}, roomId={2}, status = {3}", request.UserID, request.GameID, request.RoomID, status);
            return OnLeaveRoom(msg);
        }

        Logger.Info("OnConnectStatus end, userId={0}, gameId={1}, roomId={2}, status = {3}", request.UserID, request.GameID, request.RoomID, status);

        return reply;
    }

    /// <summary>
    /// 房间详情
    /// </summary>
    /// <param name="msg"></param>
    public override void OnRoomDetail(ByteString msg)
    {
        Request request = new Request();
        ByteUtils.ByteStringToObject(request, msg);

        RoomDetail roomDetail = new RoomDetail();
        ByteUtils.ByteStringToObject(roomDetail, request.CpProto);

        Logger.Info("OnRoomDetail, roomId={0}, state={1}, maxPlayer={2}, mode={3}, canWatch={4}, owner={5}",
            roomDetail.RoomID, roomDetail.State, roomDetail.MaxPlayer, roomDetail.Mode, roomDetail.CanWatch, roomDetail.Owner);
        foreach (PlayerInfo player in roomDetail.PlayerInfos)
        {
            Logger.Info("player userId={0}", player.UserID);
        }
    }

    public override IMessage OnHotelConnect(ByteString msg)
    {
        Connect connect = new Connect();
        ByteUtils.ByteStringToObject(connect, msg);
        Logger.Info("OnHotelConnect, gameID:{0}, roomID:{1}", connect.GameID, connect.RoomID);

        return new ConnectAck() { Status = (UInt32)ErrorCode.Ok };
    }
    public override IMessage OnHotelBroadCast(ByteString msg)
    {
        HotelBroadcast broadcast = new HotelBroadcast();
        ByteUtils.ByteStringToObject(broadcast, msg);
        Logger.Info("HotelBroadcast start, userID:{0} gameID:{1} roomID:{2} cpProto:{3}", broadcast.UserID, broadcast.GameID, broadcast.RoomID, broadcast.CpProto.ToStringUtf8());

        HotelBroadcastAck broadcastAck = new HotelBroadcastAck() { UserID = broadcast.UserID, Status = (UInt32)ErrorCode.Ok };

        PushToHotelMsg pushMsg = new PushToHotelMsg()
        {
            PushType = PushMsgType.UserTypeExclude,
            GameID = broadcast.GameID,
            RoomID = broadcast.RoomID,
            CpProto = broadcast.CpProto,
        };
        pushMsg.DstUids.Add(broadcast.UserID);

        PushToHotel(broadcast.RoomID, pushMsg);

        //测试主动推送给MVS的两个消息
        string str = broadcast.CpProto.ToStringUtf8();
        Logger.Info("HotelBroadcast, str = {0}", str);

        String[] result = str.Split("|");
        if (result.Length > 1)
        {
            if (result[0] == "joinover")
            {
                String[] param = result[1].Split(",");
                if (param.Length > 1)
                {
                    UInt64 roomID = UInt64.Parse(param[0]);
                    UInt32 gameID = UInt32.Parse(param[1]);
                    UInt32 userID = UInt32.Parse(param[2]);
                    PushJoinOver(roomID, gameID, userID);
                }
            }
            else if (result[0] == "kickplayer")
            {
                String[] param = result[1].Split(",");
                if (param.Length > 2)
                {
                    UInt64 roomID = UInt64.Parse(param[0]);
                    UInt32 srcID = UInt32.Parse(param[1]);
                    UInt32 destID = UInt32.Parse(param[2]);

                    PushKickPlayer(roomID, srcID, destID);
                }
            }
            else if (result[0] == "getRoomDetail")
            {
                String[] param = result[1].Split(",");
                if (param.Length > 1)
                {
                    UInt32 gameID = UInt32.Parse(param[0]);
                    UInt64 roomID = UInt64.Parse(param[1]);
                    PushGetRoomDetail(roomID, gameID);
                }
            }
        }

        Logger.Info("HotelBroadcast end, userID:{0} gameID:{1} roomID:{2} cpProto:{3}", broadcast.UserID, broadcast.GameID, broadcast.RoomID, broadcast.CpProto.ToStringUtf8());

        return broadcastAck;
    }
    public override IMessage OnHotelCloseConnect(ByteString msg)
    {
        CloseConnect close = new CloseConnect();
        ByteUtils.ByteStringToObject(close, msg);
        Logger.Info("CloseConnect, gameID:{0} roomID:{1}", close.GameID, close.RoomID);

        baseServer.DeleteStreamMap(close.RoomID);

        return new CloseConnectAck() { Status = (UInt32)ErrorCode.Ok };
    }
    /// <summary>
    /// 主动推送给MVS，房间不可以再加人
    /// </summary>
    public void PushJoinOver(UInt64 roomId, UInt32 gameId, UInt32 userId = 0, UInt32 version = 2)
    {
        Logger.Info("PushJoinOver, roomID:{0}, gameID:{1}", roomId, gameId);

        JoinOverReq joinReq = new JoinOverReq()
        {
            RoomID = roomId,
            GameID = gameId,
            UserID = userId
        };
        baseServer.PushToMvs(userId, version, (UInt32)MvsGsCmdID.MvsJoinOverReq, joinReq);
    }
    /// <summary>
    /// 主动推送给MVS，踢掉某人
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="srcId"></param>
    /// <param name="destId"></param>
    public void PushKickPlayer(UInt64 roomId, UInt32 srcId, UInt32 destId, UInt32 userId = 0, UInt32 version = 2)
    {
        Logger.Info("PushKickPlayer, roomID:{0}, srcId:{1}, destId:{2}", roomId, srcId, destId);

        KickPlayer kick = new KickPlayer()
        {
            RoomID = roomId,
            SrcUserID = srcId,
            UserID = destId
        };
        baseServer.PushToMvs(userId, version, (UInt32)MvsGsCmdID.MvsKickPlayerReq, kick);
    }
    /// <summary>
    /// 获取房间详情
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="gameId"></param>
    public void PushGetRoomDetail(UInt64 roomId, UInt32 gameId, UInt32 userId = 1, UInt32 version = 2)
    {
        Logger.Info("PushGetRoomDetail, roomID:{0}, gameId:{1}", roomId, gameId);
        GetRoomDetailReq roomDetail = new GetRoomDetailReq()
        {
            RoomID = roomId,
            GameID = gameId
        };
        baseServer.PushToMvs(userId, version, (UInt32)MvsGsCmdID.MvsGetRoomDetailReq, roomDetail);
    }
    /// <summary>
    /// 推送给Hotel，根据roomID来区分是哪个Hotel
    /// </summary>
    /// <param name="roomID"></param>
    /// <param name="msg"></param>
    public void PushToHotel(UInt64 roomID, IMessage msg, UInt32 userId = 1, UInt32 version = 2)
    {
        baseServer.PushToHotel(userId, version, roomID, (UInt32)HotelGsCmdID.HotelPushCmdid, msg);
    }
}

