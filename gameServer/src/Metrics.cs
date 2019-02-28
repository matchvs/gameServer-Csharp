using Google.Protobuf;
using Grpc.Core;
using Stream;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Metrics
{
    static uint reserved = 0;
    readonly uint maxReserved = 1000;

    readonly UInt32 userID;
    readonly string ns;
    readonly string podName;
    readonly string svcName;
    readonly string remoteHost;
    readonly int remotePort;
    readonly ObjectMeta meta;
    readonly bool enable;

    // metrics 统计数据
    static int roomCount;
    static int playerCount;
    static int msgCount;

    Timer reporter;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="filePath"></param>
    public Metrics(string filePath)
    {
        var confData = JsonUtils.DecodeCps(filePath);
        if (confData.Metrics != null && confData.Metrics.Enable)
        {
            ns = Environment.GetEnvironmentVariable("GS_NAMESPACE");
            if (ns != null && ns.Length != 0)
            {
                enable = true;
                svcName = Environment.GetEnvironmentVariable("DIRECTORY_SVC_NAME");
                podName = Environment.GetEnvironmentVariable("DIRECTORY_POD_NAME");
                remoteHost = Environment.GetEnvironmentVariable("EXPORTER_REMOTE_HOST");
                remotePort = int.Parse(Environment.GetEnvironmentVariable("EXPORTER_REMOTE_PORT"));
                userID = CRC32Utils.GetCRC32(svcName + podName);

                meta = new ObjectMeta(){
                    Namespace = ns,
                    Name = podName,
                };

                reporter = new Timer(this.ReportCoreMetrics, null, 0, 1000*60);
            }
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
        ByteString req = ByteUtils.ObjectToByteString(request);

        Package.Types.Frame reply = new Package.Types.Frame();
        try
        {
            IPAddress address = Dns.GetHostEntry(remoteHost).AddressList[0];
            IPEndPoint sender = new IPEndPoint(address, remotePort);
            UdpClient sock = new UdpClient();
    
            sock.Send(req.ToByteArray(), req.Length, sender);
            sock.Client.ReceiveTimeout = 100;
            byte[] rsp = sock.Receive(ref sender);
            ByteUtils.ByteStringToObject(reply, ByteString.CopyFrom(rsp));
            sock.Close();
        }
        catch (Exception e)
        {
            Logger.Warn("Send message error: {0}", e.ToString());
            return null;
        }
        return reply;
    }

    public ReportAck Report(params MetricPoint[] points)
    {
        ReportAck rsp = new ReportAck();
        if (!enable)
        {
            Logger.Warn("Metrics is disabled");
            rsp.Status = 400;
            return rsp;
        }

        PodMetrics podMetrics = new PodMetrics(){
            Meta = meta,
        };
        for (int i = 0; i < points.Length; i++)
        {
            podMetrics.Usages.Insert(i, points[i]);
        }

        Report req = new Report(){
            Metrics = podMetrics,
        };
        
        var reply = SendMsg(ByteUtils.ObjectToByteString(req), (UInt32)MetricCMD.Report);
        if (reply == null)
        {
            rsp.Status = 500;
        }
        else
        {
            ByteUtils.ByteStringToObject(rsp, reply.Message);
        }
        return rsp;
    }

    private void ReportCoreMetrics(Object stateInfo)
    {
        var rCount = Interlocked.CompareExchange(ref roomCount, 0, 0);
        var pCount = Interlocked.CompareExchange(ref playerCount, 0, 0);
        var mCount = Interlocked.Exchange(ref msgCount, 0);

        var rPoint = new MetricPoint()
        {
            Name = "RoomCount",
            Value = rCount,
            Attr = MetricAttr.Core,
        };
        var pPoint = new MetricPoint()
        {
            Name = "PlayerCount",
            Value = pCount,
            Attr = MetricAttr.Core,
        };
        var mPoint = new MetricPoint()
        {
            Name = "MessageCount",
            Value = mCount,
            Attr = MetricAttr.Core,
        };
        Report(rPoint, pPoint, mPoint);
    }

    public int RoomCountIncrement()
    {
        if (!enable)
        {
            return 0;
        }
        return Interlocked.Increment(ref roomCount);
    }

    public int RoomCountDecrement()
    {
        if (!enable)
        {
            return 0;
        }
        return Interlocked.Decrement(ref roomCount);
    }

    public int PlayerCountIncrement()
    {
        if (!enable)
        {
            return 0;
        }
        return Interlocked.Increment(ref playerCount);
    }

    public int PlayerCountDecrement()
    {
        if (!enable)
        {
            return 0;
        }
        return Interlocked.Decrement(ref playerCount);
    }

    public int MessageCountIncrement()
    {
        if (!enable)
        {
            return 0;
        }
        return Interlocked.Increment(ref msgCount);
    }
}
