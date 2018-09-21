/*******************************************************************
** 文件名:	BaseHandler
** 版  权:	(C)  2018 - 掌玩
** 创建人:	ZJ
** 日  期:	2018/01/29
** 版  本:	1.0
** 描  述:	
** 应  用:  消息处理基类

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
********************************************************************/
using Google.Protobuf;
using Stream;
using System;
public abstract class BaseHandler
{
    //MVS
    public abstract IMessage OnCreateRoom(ByteString request);
    public abstract IMessage OnJoinRoom(ByteString request);
    public abstract IMessage OnJoinOver(ByteString request);
    public abstract IMessage OnJoinOpen(ByteString request);
    public abstract IMessage OnLeaveRoom(ByteString request);
    public abstract IMessage OnKickPlayer(ByteString request);
    public abstract IMessage OnConnectStatus(ByteString request);
    public abstract IMessage OnRoomDetail(ByteString request);
    public abstract IMessage OnSetRoomProperty(ByteString request);
    //HOTEL
    public abstract IMessage OnHotelConnect(ByteString request);
    public abstract IMessage OnHotelBroadCast(ByteString request);
    public abstract IMessage OnHotelCloseConnect(ByteString request);
    public abstract IMessage OnHotelCheckin(ByteString request);
    public abstract void OnHotelSetFrameSyncRate(FrameSyncRate request);
    public abstract void OnHotelFrameUpdate(FrameData request);
}
