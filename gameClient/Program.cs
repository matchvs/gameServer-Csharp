using Grpc.Core;
using System;
using Stream;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Google.Protobuf;

namespace gsClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("game client start!");
            //var channel = new Channel("115.231.9.79:31252", ChannelCredentials.Insecure);
            var channel = new Channel("127.0.0.1:30049", ChannelCredentials.Insecure);
            var client = new Clienter(new CSStream.CSStreamClient(channel));

            client.OnCreateRoom().Wait();
            Console.WriteLine("OnCreateRoom over!");
            client.OnJoinRoom().Wait();
            Console.WriteLine("OnJoinRoom over!");
            client.OnJoinOver().Wait();
            Console.WriteLine("OnJoinOver over!");
            client.OnLeaveRoom().Wait();
            Console.WriteLine("OnLeaveRoom over!");

            client.OnKickPlayer().Wait();
            Console.WriteLine("OnKickPlayer over!");
            client.OnConnectStatus().Wait();
            Console.WriteLine("OnConnectStatus over!");
            client.OnHotelConnect().Wait();
            Console.WriteLine("OnHotelConnect over!");
            client.OnHotelBroadCast().Wait();
            Console.WriteLine("OnHotelBroadCast over!");
            client.OnHotelConnect().Wait();
            Console.WriteLine("OnHotelConnect over!");

            Console.Read();
        }
    }
}
