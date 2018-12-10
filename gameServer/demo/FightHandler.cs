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
    private RoomManager roomManger;
    public FightHandler(BaseServer server, RoomManager rmgr)
    {
        baseServer = server;
        roomManger = rmgr;
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
    /// 允许加入房间
    /// </summary>
    /// <param name="msg"></param>
    public override IMessage OnJoinOpen(ByteString msg)
    {
        Request request = new Request();
        ByteUtils.ByteStringToObject(request, msg);

        Reply reply = new Reply()
        {
            UserID = request.UserID,
            GameID = request.GameID,
            RoomID = request.RoomID,
            Errno = ErrorCode.Ok,
            ErrMsg = "OnJoinOpen success"
        };

        Logger.Info("OnJoinOpen start, userId={0}, gameId={1}, roomId={2}", request.UserID, request.GameID, request.RoomID);

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
    public override IMessage OnRoomDetail(ByteString msg)
    {
        Request request = new Request();
        ByteUtils.ByteStringToObject(request, msg);

        Reply reply = new Reply()
        {
            UserID = request.UserID,
            GameID = request.GameID,
            RoomID = request.RoomID,
            Errno = ErrorCode.Ok,
            ErrMsg = "OnRoomDetail success"
        };

        RoomDetail roomDetail = new RoomDetail();
        ByteUtils.ByteStringToObject(roomDetail, request.CpProto);

        Logger.Info("OnRoomDetail, roomId={0}, state={1}, maxPlayer={2}, mode={3}, canWatch={4}, owner={5}, roomProperty={6}",
            roomDetail.RoomID, roomDetail.State, roomDetail.MaxPlayer, roomDetail.Mode, roomDetail.CanWatch, roomDetail.Owner, roomDetail.RoomProperty.ToStringUtf8());
        foreach (PlayerInfo player in roomDetail.PlayerInfos)
        {
            Logger.Info("player userId={0}", player.UserID);
        }
        foreach (BrigadeInfo brigade in roomDetail.Brigades)
        {
            Logger.Info("brigade brigadeId={0}", brigade.BrigadeID);
            foreach (TeamDetail team in brigade.Teams)
            {
                Logger.Info("team teamId={0}, password={1}, capacity={2}, mode={3}, visibility={4}, owner={5}",
                    team.TeamInfo.TeamID, team.TeamInfo.Password, team.TeamInfo.Capacity, team.TeamInfo.Mode, team.TeamInfo.Visibility, team.TeamInfo.Owner);
                foreach (PlayerInfo player in team.Player)
                {
                    Logger.Info("player userId={0}", player.UserID);
                }
            }
        }
        return reply;
    }

    /// <summary>
    /// 设置房间自定义属性
    /// </summary>
    /// <param name="msg"></param>
    public override IMessage OnSetRoomProperty(ByteString msg)
    {
        Request request = new Request();
        ByteUtils.ByteStringToObject(request, msg);

        Reply reply = new Reply()
        {
            UserID = request.UserID,
            GameID = request.GameID,
            RoomID = request.RoomID,
            Errno = ErrorCode.Ok,
            ErrMsg = "OnSetRoomProperty success"
        };
        string roomProperty = request.CpProto.ToStringUtf8();

        Logger.Info("OnSetRoomProperty start, userId={0}, gameId={1}, roomId={2}, roomProperty={3}", request.UserID, request.GameID, request.RoomID, roomProperty);

        return reply;
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
            PushType = PushMsgType.UserTypeAll,
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
                    PushJoinOver(roomID, gameID);
                }
            }
            else if (result[0] == "joinopen")
            {
                PushJoinOpen(broadcast.RoomID, broadcast.GameID);
            }
            else if (result[0] == "kickplayer")
            {
                String[] param = result[1].Split(",");
                if (param.Length > 2)
                {
                    UInt64 roomID = UInt64.Parse(param[0]);
                    UInt32 destID = UInt32.Parse(param[1]);
                    PushKickPlayer(roomID, destID);
                }
            }
            else if (result[0] == "getRoomDetail")
            {
                String[] param = result[1].Split(",");
                if (param.Length > 1)
                {
                    UInt32 gameID = UInt32.Parse(param[0]);
                    UInt64 roomID = UInt64.Parse(param[1]);
                    PushGetRoomDetail(roomID, gameID, 2);
                }
            }
            else if (result[0] == "setRoomProperty")
            {
                ByteString roomProperty = Google.Protobuf.ByteString.CopyFromUtf8(result[1]);
                PushSetRoomProperty(broadcast.RoomID, broadcast.GameID, roomProperty);
            }
            else if (result[0] == "createRoom")
            {
                CreateRoom request = new CreateRoom()
                {
                    GameID = broadcast.GameID,
                    Ttl = 600,
                    RoomInfo = new RoomInfo()
                    {
                        RoomName = "game server room",
                        MaxPlayer = 2,
                        Mode = 1,
                        CanWatch = 1,
                        Visibility = 1,
                        RoomProperty = Google.Protobuf.ByteString.CopyFromUtf8("hello"),
                    },
                    WatchSetting = new WatchSetting()
                    {
                        MaxWatch = 3,
                        WatchPersistent = false,
                        WatchDelayMs = 10*1000,
                        CacheTime = 60*1000,
                    },
                };
                var reply = CreateRoom(request);
                Logger.Debug("create room request: {0}, reply: {1}", request, reply);
            }
            else if (result[0] == "touchRoom")
            {
                String[] param = result[1].Split(",");
                TouchRoom request = new TouchRoom()
                {
                    GameID = broadcast.GameID,
                    RoomID = UInt64.Parse(param[0]),
                    Ttl = UInt32.Parse(param[1]),
                };
                var reply = TouchRoom(request);
                Logger.Debug("touch room request: {0}, reply: {1}", request, reply);
            }
            else if (result[0] == "destroyRoom")
            {
                DestroyRoom request = new DestroyRoom()
                {
                    GameID = broadcast.GameID,
                    RoomID = UInt64.Parse(result[1]),
                };
                var reply = DestroyRoom(request);
                Logger.Debug("destroy room request: {0}, reply: {1}", request, reply);
            }
            else if (result[0] == "setFrameSyncRate")
            {
                var rate = UInt32.Parse(result[1]);
                SetFrameSyncRate(broadcast.RoomID, broadcast.GameID, rate, 1);
                Logger.Debug("set frame sync rate: {0}", rate);
            }
            else if (result[0] == "frameBroadcast")
            {
                var cpProto = result[1];
                FrameBroadcast(broadcast.RoomID, broadcast.GameID, ByteString.CopyFromUtf8(cpProto), 2);
                Logger.Info("frame broadcast: {0}", cpProto);
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
    public override IMessage OnHotelCheckin(ByteString msg)
    {
        PlayerCheckin checkin = new PlayerCheckin();
        ByteUtils.ByteStringToObject(checkin, msg);
        Logger.Info("PlayerCheckin, gameID:{0} roomID:{1} userID:{2}", checkin.GameID, checkin.RoomID, checkin.UserID);
        return new PlayerCheckinAck() { Status = (UInt32)ErrorCode.Ok };   
    }
    /// <summary>
    /// 设置帧率
    /// </summary>
    public override void OnHotelSetFrameSyncRate(FrameSyncRate request)
    {
        Logger.Info("OnHotelSetFrameSyncRate, gameID:{0} roomID:{1} frameRate:{2}", request.GameID, request.RoomID, request.FrameRate);
        return;
    }
    /// <summary>
    /// 接收帧数据
    /// </summary>
    public override void OnHotelFrameUpdate(FrameData request)
    {
        Logger.Info("OnHotelFrameUpdate, gameID:{0} roomID:{1} frameIndex:{2}", request.GameID, request.RoomID, request.FrameIndex);
        foreach (var item in request.FrameItems)
        {
            Logger.Info("SrcUser:{0}, cpProto:{1}", item.SrcUserID, item.CpProto.ToStringUtf8());
        }
        return;
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
            GameID = gameId
        };
        baseServer.PushToMvs(userId, version, (UInt32)MvsGsCmdID.MvsJoinOverReq, joinReq);
    }
    /// <summary>
    /// 主动推送给MVS，房间可以再加人
    /// </summary>
    public void PushJoinOpen(UInt64 roomId, UInt32 gameId, UInt32 userId = 0, UInt32 version = 2)
    {
        Logger.Info("PushJoinOpen, roomID:{0}, gameID:{1}", roomId, gameId);
        JoinOpenReq joinReq = new JoinOpenReq()
        {
            RoomID = roomId,
            GameID = gameId
        };
        baseServer.PushToMvs(userId, version, (UInt32)MvsGsCmdID.MvsJoinOpenReq, joinReq);
    }
    /// <summary>
    /// 主动推送给MVS，踢掉某人
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="destId"></param>
    public void PushKickPlayer(UInt64 roomId, UInt32 destId, UInt32 userId = 0, UInt32 version = 2)
    {
        Logger.Info("PushKickPlayer, roomID:{0}, destId:{2}", roomId, destId);

        KickPlayer kick = new KickPlayer()
        {
            RoomID = roomId,
            UserID = destId
        };
        baseServer.PushToMvs(userId, version, (UInt32)MvsGsCmdID.MvsKickPlayerReq, kick);
    }
    /// <summary>
    /// 获取房间详情
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="gameId"></param>
    /// <param name="latestWatcherNum"></param>
    public void PushGetRoomDetail(UInt64 roomId, UInt32 gameId, UInt32 latestWatcherNum, UInt32 userId = 0, UInt32 version = 2)
    {
        Logger.Info("PushGetRoomDetail, roomID:{0}, gameId:{1}", roomId, gameId);
        GetRoomDetailReq roomDetail = new GetRoomDetailReq()
        {
            RoomID = roomId,
            GameID = gameId,
            LatestWatcherNum = latestWatcherNum,
        };
        baseServer.PushToMvs(userId, version, (UInt32)MvsGsCmdID.MvsGetRoomDetailReq, roomDetail);
    }
    /// <summary>
    /// 设置房间自定义属性
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="gameId"></param>
    /// <param name="roomProperty"></param>
    public void PushSetRoomProperty(UInt64 roomId, UInt32 gameId, ByteString roomProperty, UInt32 userId = 0, UInt32 version = 2)
    {
        Logger.Info("PushSetRoomProperty, roomID:{0}, gameId:{1}", roomId, gameId);
        SetRoomPropertyReq roomPropertyReq = new SetRoomPropertyReq()
        {
            RoomID = roomId,
            GameID = gameId,
            RoomProperty = roomProperty
        };
        baseServer.PushToMvs(userId, version, (UInt32)MvsGsCmdID.MvsSetRoomPropertyReq, roomPropertyReq);
    }
    /// <summary>
    /// 设置帧同步帧率
    /// </summary>
    /// <param name="roomId">房间ID</param>
    /// <param name="gameId">游戏ID</param>
    /// <param name="rate">帧率</param>
    /// <param name="enableGS">GameServer是否参与帧同步（0：不参与；1：参与）</param>
    public void SetFrameSyncRate(UInt64 roomId, UInt32 gameId, UInt32 rate, UInt32 enableGS, UInt32 userId = 1, UInt32 version = 2)
    {
        GSSetFrameSyncRate setFrameSyncRateReq = new GSSetFrameSyncRate()
        {
            GameID = gameId,
            RoomID = roomId,
            FrameRate = rate,
            Priority = 0,
            FrameIdx = 1,
            EnableGS = enableGS,
        };
        baseServer.PushToHotel(userId, version, roomId, (UInt32)HotelGsCmdID.GssetFrameSyncRateCmdid, setFrameSyncRateReq);
    }
    /// <summary>
    /// 发送帧消息
    /// </summary>
    /// <param name="roomId">房间ID</param>
    /// <param name="gameId">游戏ID</param>
    /// <param name="cpProto">消息内容</param>
    /// <param name="operation">0：只发客户端；1：只发GS；2：同时发送客户端和GS</param>
    public void FrameBroadcast(UInt64 roomId, UInt32 gameId, ByteString cpProto, Int32 operation, UInt32 userId = 1, UInt32 version = 2)
    {
        GSFrameBroadcast frameBroadcastReq = new GSFrameBroadcast()
        {
            GameID = gameId,
            RoomID = roomId,
            CpProto = cpProto,
            Priority = 0,
            Operation = operation,
        };
        baseServer.PushToHotel(userId, gameId, roomId, (UInt32)HotelGsCmdID.GsframeBroadcastCmdid, frameBroadcastReq);
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
    public CreateRoomAck CreateRoom(CreateRoom request)
    {
        return roomManger.CreateRoom(request);
    }
    public TouchRoomAck TouchRoom(TouchRoom request)
    {
        return roomManger.TouchRoom(request);
    }
    public DestroyRoomAck DestroyRoom(DestroyRoom request)
    {
        return roomManger.DestroyRoom(request);
    }
}

