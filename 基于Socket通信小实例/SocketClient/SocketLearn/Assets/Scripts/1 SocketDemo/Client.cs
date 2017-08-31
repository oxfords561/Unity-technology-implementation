using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System;
using UnityEngine.UI;

public class Client : MonoBehaviour {

    private Socket clientSocket;
    private byte[] recieveData;
    private Button btn1;
    private Button btn2;
	// Use this for initialization
	void Start () {

        InitNet();

        btn1 = transform.Find("Btn1").GetComponent<Button>();
        btn2 = transform.Find("Btn2").GetComponent<Button>();

        btn1.onClick.AddListener(() => {
            SendMsg("hello");
        });

        btn2.onClick.AddListener(() => {
            SendMsg("haha");
        });
    }

    private void InitNet()
    {
        // 首先声明一个Socket
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //申明IP和端口号
        IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
        IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 8899);
        //连接服务器端
        clientSocket.Connect(iPEndPoint);

        //接收服务端发送过来的消息
        recieveData = new byte[clientSocket.ReceiveBufferSize];
        clientSocket.BeginReceive(recieveData, 0, clientSocket.ReceiveBufferSize, SocketFlags.None, RevieveCallback, null);
    }

    private void RevieveCallback(IAsyncResult ar)
    {
        int count = clientSocket.EndReceive(ar);
        string msgRec = Encoding.UTF8.GetString(recieveData, 0, clientSocket.ReceiveBufferSize);
        //输出信息
        Debug.Log(msgRec);
        recieveData = new byte[clientSocket.ReceiveBufferSize];
        clientSocket.BeginReceive(recieveData, 0, clientSocket.ReceiveBufferSize, SocketFlags.None, RevieveCallback, null);
    }

    private void SendMsg(string data)
    {
        byte[] sendMsg = Encoding.UTF8.GetBytes(data);
        clientSocket.Send(sendMsg);
    }
}
