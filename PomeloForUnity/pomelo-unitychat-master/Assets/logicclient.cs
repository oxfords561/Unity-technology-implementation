using UnityEngine;
using System.Collections;
using Proto;
using Proto.connector;
using Proto.chat;
using System;

public class logicclient : MonoBehaviour
{
    public UnityEngine.UI.Text text;
    public UnityEngine.UI.InputField input;
    public UnityEngine.UI.Button loginBtn;
    public UnityEngine.UI.Button sendBtn;

    public string uid;

    string user;
    

    chatofpomelo baseClient;
    void Awake()
    {
        baseClient = GetComponent<chatofpomelo>();
        baseClient.connectToConnector += onConnectToConnector;
        baseClient.disconnectConnector += onDisconnectConnector;
        baseClient.connectGateFailed += onConnectGateFailed;
    }

    // Use this for initialization
    void Start()
    {
        if (sendBtn) { sendBtn.interactable = true; }
        baseClient.uid = uid;
        baseClient.ConnectToGate();
    }


    public void send()
    {
        this.send(input.text);
    }

    public void send(string message)
    {
        chatHandler.send(
            "pomelo",
            message,
            user,
            "*"
            );
    }

    /// <summary>
    /// 点击登录
    /// </summary>
    public void login()
    {
        //获取当前登录用户名
        user = "pomelo" + DateTime.Now.Millisecond;

        //将用户名、房间id以及回调传入
        entryHandler.enter(user, "pomelo", delegate (entryHandler.enter_result result)
        {
            //登录成功则发送按钮激活
            if (sendBtn) { sendBtn.interactable = true; }
        });
    }

    void onConnectToConnector()
    {
        ServerEvent.onChat(delegate (ServerEvent.onChat_event ret)
        {
            string strMsg = string.Format("{0} : {1}.", ret.from, ret.msg);
            if (text)
            {
                text.text = text.text.Insert(text.text.Length, strMsg);
                text.text = text.text.Insert(text.text.Length, "\n");
            }
        });

        ServerEvent.onAdd(delegate (ServerEvent.onAdd_event msg)
        {
            string strMsg = string.Format("{0} Joined.", msg.user);
            if (text)
            {
                text.text = text.text.Insert(text.text.Length, strMsg);
                text.text = text.text.Insert(text.text.Length, "\n");
            }
        });

        ServerEvent.onLeave(delegate (ServerEvent.onLeave_event msg)
        {
            string strMsg = string.Format("{0} Leaved.", msg.user);
            if (text)
            {
                text.text = text.text.Insert(text.text.Length, strMsg);
                text.text = text.text.Insert(text.text.Length, "\n");
            }
        });


        login();
    }

    /// <summary>
    /// 点击断开连接的时候UI的变化
    /// </summary>
    void onDisconnectConnector()
    {
        if (loginBtn) { loginBtn.gameObject.SetActive(true); }
        if (sendBtn) { sendBtn.interactable = false; }
    }

    /// <summary>
    /// 连接到pomelo gate服务器失败的情况，UI进行变化
    /// </summary>
    void onConnectGateFailed()
    {
        if (loginBtn) { loginBtn.gameObject.SetActive(true); }
        if (sendBtn) { sendBtn.interactable = false; }
    }
}
