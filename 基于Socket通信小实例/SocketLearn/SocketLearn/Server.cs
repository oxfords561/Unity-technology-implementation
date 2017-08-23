using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketLearn
{
    class Server
    {
        private Socket serverSocket;
        private Socket clientSocket;
        private IPAddress ipAddress;
        private IPEndPoint ipEndPoint;
        private List<Client> clientList = new List<Client>();

        public Server(string ip,int port)
        {
            //创建Socket
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //声明ip地址和端口号
            this.ipAddress = IPAddress.Parse(ip);
            this.ipEndPoint = new IPEndPoint(ipAddress, port);
        }

        public void Start()
        {
            //Socket绑定IP和端口号
            serverSocket.Bind(ipEndPoint);
            //Socket开始监听
            serverSocket.Listen(0);

            Console.WriteLine("服务器已经启动");


            //客户端接入
            serverSocket.BeginAccept(AcceptCallback,null);
        }

        private  void AcceptCallback(IAsyncResult ar)
        {
            clientSocket = serverSocket.EndAccept(ar);
            //获取客户端名称
            string clientName = clientSocket.RemoteEndPoint.ToString();
            Client client = new Client(clientSocket, clientName);
            client.Start();
            Console.WriteLine(clientName + "已经登录。。。");

            clientList.Add(client);

            //继续循环监听客户端的连接
            serverSocket.BeginAccept(AcceptCallback, null);
        }
    }
}
