using AhpilyServer;
using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankServer.Controller;

namespace TankServer
{
    /// <summary>
    /// 网络消息处理中心
    /// </summary>
    public class NetMsgCenter : IApplication
    {
        //实例化账号处理器
        IHandler user = new UserHandler();
        IHandler game = new GameHandler();
        IHandler player = new PlayerHandler();
        /// <summary>
        /// 当客户端断开连接的时候，对客户端进行相关处理
        /// </summary>
        /// <param name="client"></param>
        public void OnDisconnect(ClientPeer client)
        {
            user.OnDisconnect(client);
        }

        /// <summary>
        /// 当客户端连接的时候进行相关数据的分发
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            switch (msg.OpCode)
            {
                case OpCode.User:
                    user.OnReceive(client,msg.SubCode,msg.Value);
                    break;
                case OpCode.Game:
                    game.OnReceive(client,msg.SubCode,msg.Value);
                    break;
                case OpCode.Player:
                    player.OnReceive(client,msg.SubCode,msg.Value);
                    break;
                default:
                    break;
            }
        }
    }
}
