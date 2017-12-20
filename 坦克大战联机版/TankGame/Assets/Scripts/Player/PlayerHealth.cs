using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : PlayerBase {

    private float health = 100;
    private Dictionary<string, Slider> sliderDic = new Dictionary<string, Slider>();
    private string[] playerNames = new string[4] { "RedTank(Clone)", "YellowTank(Clone)", "GreenTank(Clone)", "BlueTank(Clone)"};
    private SocketMsg socketMsg = new SocketMsg();
    private PlayerDto playerDto = new PlayerDto();

	void Start () {
        Bind(PlayerEvent.PLAYER_SYNC_LIFE);
        
        for (int i = 0; i < playerNames.Length; i++)
        {
            sliderDic.Add(playerNames[i], GameObject.Find(playerNames[i]).transform.Find("UserName").Find("Life").GetComponent<Slider>());
        }
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case PlayerEvent.PLAYER_SYNC_LIFE:
                DropBlood(message);
                break;
            default:
                break;
        }
    }

    void DropBlood(object value)
    {
        string playerName = value as string;
        //监听自身的血量 进行游戏的结束判断
        if (sliderDic[playerNames[identifyColor - 1]].value <= 0.1f)
        {
            isLife = false;
            Dispatch(AreaCode.UI,UIEvent.GAMEOVER_PANEL_ACTIVE,true);
            //发送游戏失败到服务器
            playerDto.color = identifyColor;
            socketMsg.Change(OpCode.Game,GameCode.GAMEOVER_CREQ,playerDto);
            Dispatch(AreaCode.NET,0,socketMsg);
            gameObject.SetActive(false);
            return;
        }

        sliderDic[playerName].value -= 0.1f;
    }
}
