using Grpc.Core;
using System;
using Stream;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Google.Protobuf;
using System.Threading;

namespace gsClient
{
    public class SendMessage
    {
        public UInt32 UserID;
        public Int32 Msg;
    }
    class Clienter
    {
        readonly CSStream.CSStreamClient client;
        public Clienter(CSStream.CSStreamClient client)
        {
            this.client = client;
        }
        public static ByteString ObjectToByteString(IMessage message)
        {
            return Google.Protobuf.MessageExtensions.ToByteString(message);
        }
        public static void ByteStringToObject(IMessage message, ByteString data)
        {
            Google.Protobuf.MessageExtensions.MergeFrom(message, data);
        }
        public string GetReply(Reply reply)
        {
            return string.Format("Reply:userID:{0} gameID:{1} roomID:{2} errno:{3} errMsg:{4}",
                reply.UserID, reply.GameID, reply.RoomID, reply.Errno, reply.ErrMsg);
        }
        public async Task OnCreateRoom()
        {
            try
            {
                Request request = new Request()
                {
                    UserID = 62,
                    GameID = 32,
                    RoomID = 52,
                };
                CreateExtInfo createEx = new CreateExtInfo()
                {
                    UserID = 62,
                    RoomID = 52,
                    State = 2,
                    CreateTime = 1223344,
                };
                request.CpProto = ObjectToByteString(createEx);

                Package.Types.Frame message = new Package.Types.Frame()
                {
                    Type = Package.Types.FrameType.CpsService,
                    Version = 2,
                    CmdId = (UInt32)MvsGsCmdID.MvsCreateRoomReq,
                    UserId = 1001,
                    Reserved = 100,
                    Message = ObjectToByteString(request),
                };
                using (var call = client.Stream(new Metadata { { "ctx", "ctx" }, { "userid", "13" } }))
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var note = call.ResponseStream.Current;
                            Reply reply = new Reply();
                            ByteStringToObject(reply, note.Message);
                            Console.WriteLine("OnCreateRoom:receve msg:" + GetReply(reply));

                        }
                        Console.WriteLine("OnCreateRoom:response over");
                    });

                    Console.WriteLine("OnCreateRoom:send OnCreateRoom start");

                    await call.RequestStream.WriteAsync(message);
                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine("RPC failed", e);
                throw;
            }
        }
        public async Task OnJoinRoom()
        {
            try
            {
                Request request = new Request()
                {
                    UserID = 62,
                    GameID = 32,
                    RoomID = 52,
                };
                JoinExtInfo joinEx = new JoinExtInfo()
                {
                    UserID = 62,
                    RoomID = 52,
                    JoinType = 2,
                };
                request.CpProto = ObjectToByteString(joinEx);

                Package.Types.Frame message = new Package.Types.Frame()
                {
                    Type = Package.Types.FrameType.CpsService,
                    Version = 2,
                    CmdId = (UInt32)MvsGsCmdID.MvsJoinRoomReq,
                    UserId = 1001,
                    Reserved = 100,
                    Message = ObjectToByteString(request),
                };
                using (var call = client.Stream(new Metadata { { "ctx", "ctx"},{ "userid", "10" } }))
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var note = call.ResponseStream.Current;
                            Reply reply = new Reply();
                            ByteStringToObject(reply, note.Message);
                            Console.WriteLine("OnJoinRoom:receve msg:" + GetReply(reply));
                        }
                        Console.WriteLine("OnJoinRoom:response over");
                    });

                    Console.WriteLine("OnJoinRoom:send OnJoinRoom start");

                    await call.RequestStream.WriteAsync(message);
                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine("RPC failed", e);
                throw;
            }
        }

        public async Task OnJoinOver()
        {
            try
            {
                Request request = new Request()
                {
                    UserID = 62,
                    GameID = 32,
                    RoomID = 52,
                };
                Package.Types.Frame message = new Package.Types.Frame()
                {
                    Type = Package.Types.FrameType.CpsService,
                    Version = 2,
                    CmdId = (UInt32)MvsGsCmdID.MvsJoinOverReq,
                    UserId = 1001,
                    Reserved = 100,
                    Message = ObjectToByteString(request),
                };
                using (var call = client.Stream(new Metadata{ { "ctx", "ctx" }, { "userid", "11" }}))
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var note = call.ResponseStream.Current;
                            Reply reply = new Reply();
                            ByteStringToObject(reply, note.Message);
                            Console.WriteLine("OnJoinOver:receve msg:" + GetReply(reply));
                        }
                        Console.WriteLine("OnJoinOver:response over");
                    });

                    Console.WriteLine("OnJoinOver:send OnJoinOver start");

                    await call.RequestStream.WriteAsync(message);
                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine("RPC failed", e);
                throw;
            }
        }

        public async Task OnLeaveRoom()
        {
            try
            {
                Request request = new Request()
                {
                    UserID = 62,
                    GameID = 32,
                    RoomID = 52,
                };
                Package.Types.Frame message = new Package.Types.Frame()
                {
                    Type = Package.Types.FrameType.CpsService,
                    Version = 2,
                    CmdId = (UInt32)MvsGsCmdID.MvsLeaveRoomReq,
                    UserId = 1001,
                    Reserved = 100,
                    Message = ObjectToByteString(request),
                };
                using (var call = client.Stream(new Metadata { { "ctx", "ctx" }, { "userid", "12" } }))
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var note = call.ResponseStream.Current;
                            Reply reply = new Reply();
                            ByteStringToObject(reply, note.Message);
                            Console.WriteLine("OnLeaveRoom:receve msg:" + GetReply(reply));

                        }
                        Console.WriteLine("OnLeaveRoom:response over");
                    });

                    Console.WriteLine("OnLeaveRoom:send OnLeaveRoom start");

                    await call.RequestStream.WriteAsync(message);
                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine("RPC failed", e);
                throw;
            }
        }

        public async Task OnKickPlayer()
        {
            try
            {
                Request request = new Request()
                {
                    UserID = 62,
                    GameID = 32,
                    RoomID = 52,
                };
                Package.Types.Frame message = new Package.Types.Frame()
                {
                    Type = Package.Types.FrameType.CpsService,
                    Version = 2,
                    CmdId = (UInt32)MvsGsCmdID.MvsKickPlayerReq,
                    UserId = 1001,
                    Reserved = 100,
                    Message = ObjectToByteString(request),
                };
                using (var call = client.Stream(new Metadata { { "ctx", "ctx" }, { "userid", "14" } }))
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var note = call.ResponseStream.Current;
                            Reply reply = new Reply();
                            ByteStringToObject(reply, note.Message);
                            Console.WriteLine("OnKickPlayer:receve msg:" + GetReply(reply));
                        }
                        Console.WriteLine("OnKickPlayer:response over");
                    });

                    Console.WriteLine("OnKickPlayer:send OnKickPlayer start");

                    await call.RequestStream.WriteAsync(message);
                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine("RPC failed", e);
                throw;
            }
        }
        public async Task OnConnectStatus()
        {
            try
            {
                Request request = new Request()
                {
                    UserID = 62,
                    GameID = 32,
                    RoomID = 52,
                    CpProto = ByteString.CopyFromUtf8("3")
                };
                Package.Types.Frame message = new Package.Types.Frame()
                {
                    Type = Package.Types.FrameType.CpsService,
                    Version = 2,
                    CmdId = (UInt32)MvsGsCmdID.MvsNetworkStateReq,
                    UserId = 1001,
                    Reserved = 100,
                    Message = ObjectToByteString(request),
                };
                using (var call = client.Stream(new Metadata { { "ctx", "ctx" }, { "userid", "14" } }))
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var note = call.ResponseStream.Current;
                            Reply reply = new Reply();
                            ByteStringToObject(reply, note.Message);
                            Console.WriteLine("OnConnectStatus:receve msg:" + GetReply(reply));
                        }
                        Console.WriteLine("OnConnectStatus:response over");
                    });

                    Console.WriteLine("OnConnectStatus:send OnConnectStatus start");

                    await call.RequestStream.WriteAsync(message);
                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine("RPC failed", e);
                throw;
            }
        }
        public async Task OnHotelConnect()
        {
            try
            {
                Connect connect = new Connect()
                {
                    GameID = 12,
                    RoomID = 13,
                };
                Package.Types.Frame message = new Package.Types.Frame()
                {
                    Type = Package.Types.FrameType.HotelServer,
                    Version = 2,
                    CmdId = (UInt32)HotelGsCmdID.HotelCreateConnect,
                    UserId = 1001,
                    Reserved = 100,
                    Message = ObjectToByteString(connect)
                };
                using (var call = client.Stream(new Metadata { { "ctx", "ctx" }, { "userid", "15" } }))
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            Package.Types.Frame note = call.ResponseStream.Current;
                            ConnectAck connectAck = new ConnectAck();
                            ByteStringToObject(connectAck, note.Message);

                            Console.WriteLine("OnHotelConnect:receve msg status:" + connectAck.Status);

                        }
                        Console.WriteLine("OnHotelConnect:response over");
                    });

                    Console.WriteLine("OnHotelConnect:send ConnectV32 start");

                    await call.RequestStream.WriteAsync(message);
                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine("RPC failed", e);
                throw;
            }
        }

        public async Task OnHotelBroadCast()
        {
            try
            {
                SendMessage send = new SendMessage
                {
                    UserID = 11,
                    Msg = 1,
                };
                HotelBroadcast broadcast = new HotelBroadcast()
                {
                    UserID = 11,
                    GameID = 12,
                    RoomID = 13,
                    //CpProto = Google.Protobuf.ByteString.CopyFromUtf8("kickplayer|1588649253385801743,200945,200945"),
                    //CpProto = Google.Protobuf.ByteString.CopyFromUtf8(JsonConvert.SerializeObject(send)),
                    CpProto = Google.Protobuf.ByteString.CopyFromUtf8("getRoomDetail|158864,200945"),
                };
                Package.Types.Frame message = new Package.Types.Frame()
                {
                    Type = Package.Types.FrameType.HotelServer,
                    Version = 2,
                    CmdId = (UInt32)HotelGsCmdID.HotelBroadcastCmdid,
                    UserId = 1001,
                    Reserved = 100,
                    Message = ObjectToByteString(broadcast)
                };
                using (var call = client.Stream(new Metadata { { "ctx", "ctx" }, { "userid", "17" } }))
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            Package.Types.Frame note = call.ResponseStream.Current;
                            HotelBroadcastAck broadcastAck = new HotelBroadcastAck();
                            ByteStringToObject(broadcastAck, note.Message);

                            Console.WriteLine("OnHotelBroadCast:receve msg userID:" + broadcastAck.UserID + " status:" + broadcastAck.Status);

                        }
                        Console.WriteLine("OnHotelBroadCast:response over");
                    });

                    Console.WriteLine("OnHotelBroadCast:send OnHotelBroadCast start");

                    await call.RequestStream.WriteAsync(message);
                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine("RPC failed", e);
                throw;
            }
        }
        public async Task OnHotelCloseConnect()
        {
            try
            {
                CloseConnect closeconnect = new CloseConnect()
                {
                    GameID = 12,
                    RoomID = 13,
                };
                Package.Types.Frame message = new Package.Types.Frame()
                {
                    Type = Package.Types.FrameType.HotelServer,
                    Version = 2,
                    CmdId = (UInt32)HotelGsCmdID.HotelCloseConnet,
                    UserId = 1001,
                    Reserved = 100,
                    Message = ObjectToByteString(closeconnect)
                };
                using (var call = client.Stream(new Metadata { { "ctx", "ctx" }, { "userid", "16" } }))
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            Package.Types.Frame note = call.ResponseStream.Current;
                            CloseConnectAck closeConnectAck = new CloseConnectAck();
                            ByteStringToObject(closeConnectAck, note.Message);

                            Console.WriteLine("OnHotelCloseConnect:receve msg status:" + closeConnectAck.Status);

                        }
                        Console.WriteLine("OnHotelCloseConnect:response over");
                    });

                    Console.WriteLine("OnHotelCloseConnect:send CloseconnectV32 start");

                    await call.RequestStream.WriteAsync(message);
                    await call.RequestStream.CompleteAsync();
                    await responseReaderTask;
                }
            }
            catch (RpcException e)
            {
                Console.WriteLine("RPC failed", e);
                throw;
            }
        }
    }
}
