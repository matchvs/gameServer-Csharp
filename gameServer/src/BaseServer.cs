/*******************************************************************
** 文件名:	BaseServer
** 版  权:	(C)  2018 - 掌玩
** 创建人:	ZJ
** 日  期:	2018/01/29
** 版  本:	1.0
** 描  述:	
** 应  用:  服务基类,可以派生诸如gameServer,chatServer...

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
********************************************************************/
using Google.Protobuf;
using Stream;
using System;

public abstract class BaseServer
{
    public abstract void Connect(UInt32 userID, string token);
    public abstract void Disconnect(UInt32 userID, string token);
    public abstract void WaitOver();
    public abstract void Run();
    public abstract Package.Types.Frame DealMsg(Package.Types.Frame request);
    public abstract void PushToMvs(UInt32 userID, UInt32 version, UInt32 cmdID, IMessage msg);
    public abstract void PushToHotel(UInt32 userID, UInt32 version, UInt64 roomID, UInt32 cmdID, IMessage msg);
    public abstract void DeleteStreamMap(UInt64 roomID);
}
