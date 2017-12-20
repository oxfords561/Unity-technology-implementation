using AhpilyServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankServer
{
    /// <summary>
    /// 多人坦克 服务器启动入口
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer server = new ServerPeer();
            //指定应用层
            server.SetApplication(new NetMsgCenter());
            server.Start(6666,10);

            Console.ReadKey();
        }
    }
}
