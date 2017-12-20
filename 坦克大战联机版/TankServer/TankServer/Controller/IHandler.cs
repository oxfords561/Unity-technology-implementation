using AhpilyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankServer.Controller
{
    /// <summary>
    /// 服务器逻辑处理接口 通过这个接口实现多种逻辑处理
    /// </summary>
    public interface IHandler
    {
        void OnReceive(ClientPeer client, int subCode, object value);

        void OnDisconnect(ClientPeer client);
    }
}
