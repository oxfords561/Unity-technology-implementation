using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using AhpilyServer.Tool;

namespace AhpilyServer
{
    /// <summary>
    /// 封装的客户端的连接对象
    /// </summary>
    public class ClientPeer
    {
        public Socket ClientSocket { get; set; }
        private MySqlConnection mySqlConn;

        public ClientPeer()
        {
            this.ReceiveArgs = new SocketAsyncEventArgs();
            this.ReceiveArgs.UserToken = this;
            this.ReceiveArgs.SetBuffer(new byte[1024], 0, 1024);
            this.SendArgs = new SocketAsyncEventArgs();
            this.SendArgs.Completed += SendArgs_Completed;

            mySqlConn = ConnHelper.Connect();
        }

        public MySqlConnection MySqlConn
        {
            get { return mySqlConn; }
        }

        #region 接收数据

        public delegate void ReceiveCompleted(ClientPeer client, SocketMsg msg);

        /// <summary>
        /// 一个消息解析完成的回调
        /// </summary>
        public ReceiveCompleted receiveCompleted;

        /// <summary>
        /// 一旦接收到数据 就存到缓存区里面
        /// </summary>
        private List<byte> dataCache = new List<byte>();

        /// <summary>
        /// 接受的异步套接字请求
        /// </summary>
        public SocketAsyncEventArgs ReceiveArgs { get; set; }

        /// <summary>
        /// 是否正在处理接受的数据
        /// </summary>
        private bool isReceiveProcess = false;

        /// <summary>
        /// 自身处理数据包
        /// </summary>
        /// <param name="packet"></param>
        public void StartReceive(byte[] packet)
        {
            dataCache.AddRange(packet);
            if (!isReceiveProcess)
                processReceive();
        }

        /// <summary>
        /// 处理接受的数据
        /// </summary>
        private void processReceive()
        {
            isReceiveProcess = true;
            //解析数据包
            byte[] data = EncodeTool.DecodePacket(ref dataCache);

            if (data == null)
            {
                isReceiveProcess = false;
                return;
            }

            SocketMsg msg = EncodeTool.DecodeMsg(data);
            //回调给上层
            if (receiveCompleted != null)
                receiveCompleted(this, msg);

            //尾递归
            processReceive();
        }

        ////粘包拆包问题 ： 解决决策 ：消息头和消息尾。
        //// 比如 发送的数据：  12345
        //void test()
        //{
        //    byte[] bt = Encoding.Default.GetBytes("12345");

        //    //怎么构造
        //    //头：消息的长度 bt.Length
        //    //尾：具体的消息 bt
        //    int length = bt.Length;
        //    byte[] bt1 = BitConverter.GetBytes(length);
        //    //得到消息就是：bt1 + bt

        //    ///怎么读取
        //    //int length = 前四个字节转成int类型
        //    //然后读取 这个长度的数据
        //}

        #endregion

        #region 断开连接

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            //清空数据
            dataCache.Clear();
            isReceiveProcess = false;
            sendQueue.Clear();
            isSendProcess = false;

            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            ClientSocket = null;

            ConnHelper.CLose(mySqlConn);
        }

        #endregion

        #region 发送数据

        /// <summary>
        /// 发送的消息的一个队列
        /// </summary>
        private Queue<byte[]> sendQueue = new Queue<byte[]>();

        private bool isSendProcess = false;

        /// <summary>
        /// 发送的异步套接字操作
        /// </summary>
        private SocketAsyncEventArgs SendArgs;

        /// <summary>
        /// 发送的时候 发现 断开连接的回调
        /// </summary>
        /// <param name="client"></param>
        /// <param name="reason"></param>
        public delegate void SendDisconnect(ClientPeer client, string reason);

        public SendDisconnect sendDisconnect;

        /// <summary>
        /// 发送网络消息
        /// </summary>
        /// <param name="opCode">操作码</param>
        /// <param name="subCode">子操作</param>
        /// <param name="value">参数</param>
        public void Send(int opCode, int subCode, object value)
        {
            SocketMsg msg = new SocketMsg(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMsg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);

            //存入消息队列里面
            sendQueue.Enqueue(packet);
            if (!isSendProcess)
                send();
        }

        /// <summary>
        /// 处理发送的消息
        /// </summary>
        private void send()
        {
            isSendProcess = true;

            //如果数据的条数等于0的话 就停止发送
            if (sendQueue.Count == 0)
            {
                isSendProcess = false;
                return;
            }
            //取出一条数据
            byte[] packet = sendQueue.Dequeue();
            //设置消息 发送的异步套接字操作 的发送数据缓冲区
            SendArgs.SetBuffer(packet, 0, packet.Length);
            bool result = ClientSocket.SendAsync(SendArgs);
            if (result == false)
            {
                processSend();
            }
        }

        private void SendArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            processSend();
        }

        /// <summary>
        /// 当异步发送请求完成的时候调用
        /// </summary>
        private void processSend()
        {
            //发送的有没有错误
            if (SendArgs.SocketError != SocketError.Success)
            {
                //发送出错了 客户端断开连接了
                sendDisconnect(this, SendArgs.SocketError.ToString());
            }
            else
            {
                send();
            }
        }

        #endregion
    }
}
