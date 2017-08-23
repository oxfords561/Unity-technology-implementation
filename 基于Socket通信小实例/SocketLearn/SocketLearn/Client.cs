using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketLearn
{
    class Client
    {
        private Socket clientSocket;
        private string clientName;
        private byte[] recieveData;

        public Client(Socket client,string clientName)
        {
            this.clientSocket = client;
            this.clientName = clientName;
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
            if (data.Equals("hello"))
            {
                SendMessage("hello too");
            }else if (data.Equals("haha"))
            {
                SendMessage("heihei");
            }
        }

        //发送消息给客户端
        private void SendMessage(string data)
        {
            byte[] msgData = Encoding.UTF8.GetBytes(data);
            clientSocket.Send(msgData);
        }
    }
}
