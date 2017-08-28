using System;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SocketLearn
{
    class Client
    {
        private Socket clientSocket;
        private string clientName;
        private byte[] recieveData;
        private Server server;
        public List<Client> clientList = new List<Client>();

        public Client(Socket client,string clientName,Server server)
        {
            this.clientSocket = client;
            this.clientName = clientName;
            this.server = server;
            recieveData = new byte[clientSocket.ReceiveBufferSize];
        }

        public void Start()
        {
            clientSocket.BeginReceive(recieveData,0, clientSocket.ReceiveBufferSize,SocketFlags.None, RecieveCallback, null);
        }

        //接收客户端的消息
        private void RecieveCallback(IAsyncResult ar)
        {
            try
            {
                //接收到的数据长度
                int count = clientSocket.EndReceive(ar);

                //防止客户端异常退出
                if (count == 0)
                {
                    clientSocket.Close();
                    return;
                }

                //对接收到的数据进行处理
                string msgRec = Encoding.UTF8.GetString(recieveData, 0, count);
                HandleResponse(msgRec);

                //输出到控制台
                Console.WriteLine(msgRec);

                //循环接收客户端发送过来的数据
                clientSocket.BeginReceive(recieveData, 0, clientSocket.ReceiveBufferSize, SocketFlags.None, RecieveCallback, null);
            }
            catch (Exception)
            {
                if (clientSocket != null)
                {
                    clientSocket.Close();
                    return;
                }
            }
            
        }

        //对客户端返回过来的数据进行处理
        private void HandleResponse(string data)
        {
           //进行数据解析的判断，如果不包含flag标记的数据不执行之后代码
            if (data.EndsWith("*")) return;

            //只有一个客户端的时候不同步信息
            clientList = server.clientList;
            if (clientList.Count < 2) return;
           
            //将当前客户端传送过来的同步信息转发给其他客户端进行同步
            foreach (Client item in clientList)
            {
                if (item != this)
                {
                    Console.WriteLine("转发消息 "+data);
                    item.SendMessage(data);
                }
            }

        }

        //发送消息给客户端
        public void SendMessage(string data)
        {
            Console.WriteLine("发送消息 "+data);
            byte[] msgData = Encoding.UTF8.GetBytes(data);
            try
            {
                clientSocket.Send(msgData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
