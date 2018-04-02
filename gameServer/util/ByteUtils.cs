/*******************************************************************
** 文件名:	ByteUtils
** 版  权:	(C)  2018 - 掌玩
** 创建人:	ZJ
** 日  期:	2018/01/23
** 版  本:	1.0
** 描  述:	
** 应  用:  字节工具

**************************** 修改记录 ******************************
** 修改人: 
** 日  期: 
** 描  述: 
********************************************************************/
using Google.Protobuf;

public class ByteUtils
{
    /// <summary>
    /// 对象转换成ByteString
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static ByteString ObjectToByteString(IMessage message)
    {
        return Google.Protobuf.MessageExtensions.ToByteString(message);
    }
    public static void ByteStringToObject(IMessage message, ByteString data)
    {
        Google.Protobuf.MessageExtensions.MergeFrom(message, data);
    }
}
