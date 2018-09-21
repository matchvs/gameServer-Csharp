/*******************************************************************
** 文件名:	main
** 版  权:	(C)  2018 - 掌玩
** 创建人:	ZJ
** 日  期:	2018/01/29
** 版  本:	1.0
** 描  述:	
** 应  用:  gs入口

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
********************************************************************/
using System;
using System.IO;
using System.Threading;

class MainServer
{
    static void Main(string[] args)
    {
        Logger.Init();
        Logger.Info("game service start...");

        string confPath = System.AppDomain.CurrentDomain.BaseDirectory;
        string confFile = System.IO.Path.Combine(confPath, "conf/gs.json");

        GameServer gameServer = new GameServer();
        gameServer.Init(confFile);
        RoomManager roomManager = new RoomManager(confFile);
        FightHandler fight = new FightHandler(gameServer, roomManager);
        gameServer.Bind(fight);

        int pid = System.Diagnostics.Process.GetCurrentProcess().Id;
        File.WriteAllText(System.IO.Path.Combine(confPath, "gameServer.dll_pid"), pid.ToString());

        Logger.Info("game service run...");
        gameServer.Run();
        gameServer.WaitOver();
        Logger.Info("game service over...");
    }
}
