using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol.Code;
using UnityEngine.UI;
using Protocol.Dto;

public class ReadyPanel : UIBase {

    private void OnEnable()
    {
        Bind(UIEvent.READY_COUNT_SHOW);
        Bind(UIEvent.HOUSE_OWNER);
        Bind(UIEvent.READY_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.READY_COUNT_SHOW:
                SetReadyCount(message);
                break;
            case UIEvent.HOUSE_OWNER:
                SetHouseOwnerButton();
                break;
            case UIEvent.READY_PANEL_ACTIVE:
                setPanelActive((bool)message);
                InitData();//初始化数据
                break;
            default:
                break;
        }
    }

    private Button readyButton;
    private Button mainStartButton;
    private Text playerCount;
    private Text readyText;
    private Transform users;

    private PromptMsg promptMsg;
    private SocketMsg socketMsg;
    private UserDto dto;

    void Start()
    {
        readyButton = transform.Find("ReadyButton").GetComponent<Button>();
        mainStartButton = transform.Find("MainStartButton").GetComponent<Button>();
        playerCount = transform.Find("PlayerCount").GetComponent<Text>();
        readyText = readyButton.GetComponentInChildren<Text>();
        users = transform.Find("Users");

        readyButton.onClick.AddListener(ReadyClick);
        mainStartButton.onClick.AddListener(MainStartClick);

        //房主开始按钮只有房主才能看到
        mainStartButton.gameObject.SetActive(false);

        Debug.Log("执行");
    }

    void InitData()
    {
        readyText.text = "准备";
        readyButton.interactable = true;

        for (int i = 0; i < users.childCount; i++)
        {
            users.GetChild(i).GetComponent<Text>().text = "";
        }

        playerCount.text = "0";

        mainStartButton.gameObject.SetActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        readyButton.onClick.RemoveListener(ReadyClick);
        mainStartButton.onClick.RemoveListener(MainStartClick);
    }

    /// <summary>
    /// 准备按钮点击事件处理
    /// </summary>
    private void ReadyClick()
    {
        readyText.text = "已准备";
        readyButton.interactable = false;

        SocketMsg socketMsg = new SocketMsg(OpCode.User, UserCode.READY_CREQ, null);
        
        if (PlayerPrefs.GetString("username") != null)
             dto = new UserDto(PlayerPrefs.GetString("username"),PlayerPrefs.GetString("password"));
        socketMsg.Change(OpCode.User, UserCode.READY_CREQ, dto);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }

    /// <summary>
    /// 房主点击开始游戏
    /// </summary>
    private void MainStartClick()
    {
        
        //1、房主同步开始游戏消息到服务器
        SocketMsg socketMsg = new SocketMsg(OpCode.Game, GameCode.GAMESTART_CREQ, null);
        socketMsg.Change(OpCode.Game, GameCode.GAMESTART_CREQ, null);
        Dispatch(AreaCode.NET, 0, socketMsg);

    }

    private void SetReadyCount(object value)
    {
        RoomDto roomDto = value as RoomDto;
        playerCount.text = ""+roomDto.userList.Count;
        for (int i = 0; i < roomDto.userList.Count; i++)
        {
            users.GetChild(i).GetComponent<Text>().text = roomDto.userList[i].username + " 已加入游戏";
        }

        //发送消息给Game模块 赋予玩家颜色信息标识
        Dispatch(AreaCode.GAME, GameEvent.GAME_COLOR, roomDto.color);
    }

    private void SetHouseOwnerButton()
    {
        if (transform.Find("MainStartButton").gameObject == null)
            return;

        transform.Find("MainStartButton").gameObject.SetActive(true);
    }
}
