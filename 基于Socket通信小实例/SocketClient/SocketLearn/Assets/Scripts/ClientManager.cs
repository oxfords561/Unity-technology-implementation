using System.Text;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;

public class ClientManager : MonoBehaviour{

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject otherPlayerPrefab;
    [HideInInspector]
    public GameObject currentPlayer;
    private Socket clientSocket;
    private byte[] recieveData;
    private PlayerMove playerMove;
    private int recieveNum;
    private string recieveStr = "";
    private bool isRecieve = false;
    private Message msg = new Message();

    void Start()
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
        clientSocket.BeginReceive(msg.dataBytes, msg.startLenght, msg.restDataLength, SocketFlags.None, RevieveCallback, null);
    }

    private void RevieveCallback(IAsyncResult ar)
    {
        int count = clientSocket.EndReceive(ar);

        //输出信息
        msg.ParseData(count, RecieveMsg);

        clientSocket.BeginReceive(msg.dataBytes, msg.startLenght, msg.restDataLength, SocketFlags.None, RevieveCallback, null);
    }

    void Update()
    {
        //由于同步信息是回调回来的，所以要从主线程调用的话就采取这样的形式
        if (isRecieve)
        {
            isRecieve = false;
            if (!string.IsNullOrEmpty(recieveStr))
            {
                HandleRecieveData(recieveStr);
            }

        }
    }

    public void RecieveMsg(string data)
    {
        isRecieve = true;
        recieveStr = data;
    }

    private void HandleRecieveData(string data)
    {
        //当连接服务器第一次的时候会返回当前客户端的flag标记
        if (recieveNum < 1)
        {
            recieveNum++;

            playerPrefab = GameObject.Instantiate<GameObject>(playerPrefab);
            otherPlayerPrefab = GameObject.Instantiate<GameObject>(otherPlayerPrefab);

            //对flag进行判断并进行当前客户端控制对象的选取
            if (int.Parse(data) == 1)
            {
                playerMove = playerPrefab.GetComponent<PlayerMove>();
                Destroy(otherPlayerPrefab.GetComponent<PlayerMove>());
                playerMove.flag = "1";
                currentPlayer = playerPrefab;
            }
            else
            {
                playerMove = otherPlayerPrefab.GetComponent<PlayerMove>();
                Destroy(playerPrefab.GetComponent<PlayerMove>());
                playerMove.flag = "2";
                currentPlayer = otherPlayerPrefab;
            }
        }
        else
        {
            //第一次之后访问服务器则进行数据的接收同步
            playerMove.RecieveData(data);
        }
    }

    public void SendMsg(string data)
    {
        byte[] sendMsg = msg.PackData(data);
        clientSocket.Send(sendMsg);
    }
}
