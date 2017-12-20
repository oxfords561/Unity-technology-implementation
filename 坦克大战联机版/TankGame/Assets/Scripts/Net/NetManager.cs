using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 网络模块
/// </summary>
public class NetManager : ManagerBase
{
    public static NetManager Instance = null;

    private ClientPeer client = new ClientPeer("10.16.29.59", 6666);

    private void Start()
    {
        client.Connect();
    }

    private void Update()
    {
        if (client == null)
            return;

        while (client.SocketMsgQueue.Count > 0)
        {
            SocketMsg msg = client.SocketMsgQueue.Dequeue();
            //处理消息
            processSocketMsg(msg);
        }
    }

    #region 处理接收到的服务器发来的消息

    HandlerBase userHandler = new UserHandler();
    HandlerBase gameHandler = new GameHandler();
    HandlerBase playerHandler = new PlayerHandler();

    /// <summary>
    /// 接受网络的消息
    /// </summary>
    private void processSocketMsg(SocketMsg msg)
    {
        switch (msg.OpCode)
        {
            case OpCode.User:
                userHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            case OpCode.Game:
                gameHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            case OpCode.Player:
                playerHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            default:
                break;
        }
    }

    #endregion


    #region 处理客户端内部 给服务器发消息的 事件

    private void Awake()
    {
        Instance = this;

        Add(0, this);
    }

    public override void Execute(int eventCode, object message)
    {
        client.Send(message as SocketMsg);
    }

    #endregion

}

