using System;
using System.Collections.Generic;
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
        private int flag = 1;
        public List<Client> clientList = new List<Client>();

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
            Client client = new Client(clientSocket, clientName,this);
            client.Start();
            Console.WriteLine(clientName + "已经登录。。。");

            //将连接上的客户端记录在案
            clientList.Add(client);

            //并且每连上一个客户端便给予当前客户端一个标记
            //这里的flag是给先后连上服务器的客户端进行的编号
            //输出给客户端是为了让客户端知道顺序，从而进行当前客户端来选择控制对象
            client.SendMessage(""+flag);
            flag++;

            //继续循环监听客户端的连接
            serverSocket.BeginAccept(AcceptCallback, null);
        }
    }
}
