using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 网络消息
///     作用：发送的时候 都要发送这个类
/// </summary>
public class SocketMsg
{
    /// <summary>
    /// 操作码
    /// </summary>
    public int OpCode { get; set; }

    /// <summary>
    /// 子操作
    /// </summary>
    public int SubCode { get; set; }

    /// <summary>
    /// 参数
    /// </summary>
    public object Value { get; set; }

    public SocketMsg()
    {

    }

    public SocketMsg(int opCode, int subCode, object value)
    {
        this.OpCode = opCode;
        this.SubCode = subCode;
        this.Value = value;
    }

    public void Change(int opCode, int subCode, object value)
    {
        this.OpCode = opCode;
        this.SubCode = subCode;
        this.Value = value;
    }
}

