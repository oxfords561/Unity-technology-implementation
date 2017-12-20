using System;
using System.Collections.Generic;

/// <summary>
/// 客户端处理基类
/// </summary>
public abstract class HandlerBase
{
    public abstract void OnReceive(int subCode, object value);

    /// <summary>
    /// 为了方便发消息
    /// </summary>
    protected void Dispatch(int areaCode, int eventCode, object message)
    {
        MsgCenter.Instance.Dispatch(areaCode, eventCode, message);
    }
}
