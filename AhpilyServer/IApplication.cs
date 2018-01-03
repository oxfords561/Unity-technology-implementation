using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AhpilyServer
{
    public interface IApplication
    {
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="client"></param>
        void OnDisconnect(ClientPeer client);

        /// <summary>
        /// 接受数据
        /// </summary>
        void OnReceive(ClientPeer client, SocketMsg msg);
    }
}
