/*******************************************************************
** 文件名:	JsonUtils
** 版  权:	(C)  2017 - 掌玩
** 创建人:	ZJ
** 日  期:	2017/08/28
** 版  本:	1.0
** 描  述:	
** 应  用:  序列化、反序列化json数据

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
********************************************************************/
using System;
using System.IO;
using Newtonsoft.Json;
using Google.Protobuf;

public class JsonUtils
{
    public static Gsconfig DecodeCps(string path)
    {
        string file = File.ReadAllText(path, System.Text.Encoding.UTF8);
        Gsconfig config = JsonConvert.DeserializeObject<Gsconfig>(file);
        return config;
    }
    public static ByteString EncodetoByteString(object obj)
    {
        return Google.Protobuf.ByteString.CopyFromUtf8(JsonConvert.SerializeObject(obj));
    }

    public static Request DecodeByteStringToRequest(ByteString msg)
    {
        return JsonConvert.DeserializeObject<Request>(msg.ToStringUtf8());
    }
}
