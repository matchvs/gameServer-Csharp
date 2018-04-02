/*******************************************************************
** 文件名:	logger
** 版  权:	(C)  2017 - 掌玩
** 创建人:	ZJ
** 日  期:	2017/08/28
** 版  本:	1.0
** 描  述:	
** 应  用:  

**************************** 修改记录 ******************************
** 修改人: ZJ 增加log4net日志系统，一共3个日志，Info日志(DEBUG,INFO,WARN,ERROR,FATAL),输出受日志等级影响，
*          但访问日志和流水日志不受影响
** 日  期: 2017.9.6
** 描  述: 
********************************************************************/
//ALL
//DEBUG
//INFO
//WARN
//ERROR
//FATAL
//OFF
using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Diagnostics;
using System.Xml;

public class Logger
{
    private static ILog logInfo;
    private static ILog logAccess;
    private static ILog logFlow;
    private static string guidStr;
    public static void Init()
    {
        XmlDocument xml = new XmlDocument();
        string conPath = System.AppDomain.CurrentDomain.BaseDirectory;
        xml.Load(System.IO.Path.Combine(conPath, "conf/logConf.xml"));
        guidStr = Guid.NewGuid().ToString();
        
         ILoggerRepository rep = LogManager.CreateRepository(guidStr);
        XmlConfigurator.Configure(rep, xml["log4net"]);
        
        logInfo = LogManager.GetLogger(rep.Name, typeof(Logger));
        logAccess = LogManager.GetLogger(rep.Name, "Access");
        logFlow = LogManager.GetLogger(rep.Name, "Flow");
    }
    public static void Access(string str, params object[] args)
    {
        int index = 1;
        StackTrace st = new StackTrace(true);
        StackFrame[] sf = st.GetFrames();
        string filefullName = sf[index].GetFileName();
        string fileName = filefullName.Substring(filefullName.LastIndexOf("\\") + 1);
        int line = sf[index].GetFileLineNumber();

        logAccess.Info(string.Format("{0}:{1} {2}", fileName, line, string.Format(str, args)));
    }
    public static void Flow(string str, params object[] args)
    {
        int index = 1;
        StackTrace st = new StackTrace(true);
        StackFrame[] sf = st.GetFrames();
        string filefullName = sf[index].GetFileName();
        string fileName = filefullName.Substring(filefullName.LastIndexOf("\\") + 1);
        int line = sf[index].GetFileLineNumber();

        logFlow.Info(string.Format("{0}:{1} {2}", fileName, line, string.Format(str, args)));
    }
    public static void Info(string str, params object[] args)
    {
        int index = 1;
        StackTrace st = new StackTrace(true);               //参考log4net源码,配置%L打印不出来行号
        StackFrame[] sf = st.GetFrames();
        string fileName = sf[index].GetMethod().ReflectedType.FullName;
        int line = sf[index].GetFileLineNumber();

        logInfo.Info(string.Format("{0}:{1} {2}", fileName, line, string.Format(str, args)));
    }
    public static void Debug(string str, params object[] args)
    {
        int index = 1;
        StackTrace st = new StackTrace(true);
        StackFrame[] sf = st.GetFrames();
        string fileName = sf[index].GetMethod().ReflectedType.FullName;
        int line = sf[index].GetFileLineNumber();

        logInfo.Debug(string.Format("{0}:{1} {2}", fileName, line, string.Format(str, args)));
    }
    public static void Warn(string str, params object[] args)
    {
        int index = 1;
        StackTrace st = new StackTrace(true);
        StackFrame[] sf = st.GetFrames();
        string fileName = sf[index].GetMethod().ReflectedType.FullName;
        int line = sf[index].GetFileLineNumber();

        logInfo.Warn(string.Format("{0}:{1} {2}", fileName, line, string.Format(str, args)));
    }
    public static void Error(string str, params object[] args)
    {
        int index = 1;
        StackTrace st = new StackTrace(true);
        StackFrame[] sf = st.GetFrames();
        string fileName = sf[index].GetMethod().ReflectedType.FullName;
        int line = sf[index].GetFileLineNumber();

        logInfo.Error(string.Format("{0}:{1} {2}", fileName, line, string.Format(str, args)));
    }
    public static void Fatal(string str, params object[] args)
    {
        int index = 1;
        StackTrace st = new StackTrace(true);
        StackFrame[] sf = st.GetFrames();
        string fileName = sf[index].GetMethod().ReflectedType.FullName;
        int line = sf[index].GetFileLineNumber();

        logInfo.Fatal(string.Format("{0}:{1} {2}", fileName, line, string.Format(str, args)));
    }
    //ALL    6
    //DEBUG  1
    //INFO   2
    //WARN   3
    //ERROR  4
    //FATAL  5
    //OFF   0
    // > level的日志会显示出来
    public static void SetLevel(int level)
    {
        log4net.Core.Level logLevel = log4net.Core.Level.All;
        if (level == 0)
        {
            logLevel = log4net.Core.Level.Off;
        }
        else if (level == 1)
        {
            logLevel = log4net.Core.Level.Debug;
        }
        else if (level == 2)
        {
            logLevel = log4net.Core.Level.Info;
        }
        else if (level == 3)
        {
            logLevel = log4net.Core.Level.Warn;
        }
        else if (level == 4)
        {
            logLevel = log4net.Core.Level.Error;
        }
        else if (level == 5)
        {
            logLevel = log4net.Core.Level.Fatal;
        }
        else if (level == 6)
        {
            logLevel = log4net.Core.Level.All;
        }
        ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository(guidStr)).Root.Level = logLevel;
    }
}
