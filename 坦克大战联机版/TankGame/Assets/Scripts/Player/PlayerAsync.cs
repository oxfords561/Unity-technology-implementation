using Protocol.Code;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置玩家的信息同步与接收
/// </summary>
public class PlayerAsync : PlayerBase {

    private float refreshRate = 0.02f;
    private PlayerDto playerDto_CREQ = new PlayerDto();
    private string[] playerNames = new string[] { "RedTank(Clone)", "YellowTank(Clone)", "GreenTank(Clone)", "BlueTank(Clone)" };
    private SocketMsg socketMsg = new SocketMsg();
    private int color = 0;

    private void Awake()
    {
        InvokeRepeating("SyncPositionAndRotation_CREQ", 1f, refreshRate);
    }

    public override void Execute(int eventCode, object message)
    {

    }

    void SyncPositionAndRotation_CREQ()
    {
        if (!isLife)
            Destroy(this);

        for (int i = 0; i < playerNames.Length; i++)
        {
            if (gameObject.name.Equals(playerNames[i]))
                color = i + 1;
        }

        //将本地玩家标识上传到父类 方便使用
        if (identifyColor == 0)
            identifyColor = color;

        playerDto_CREQ.color = color;
        playerDto_CREQ.posX = transform.position.x;
        playerDto_CREQ.posY = transform.position.y;
        playerDto_CREQ.posZ = transform.position.z;
        playerDto_CREQ.rotX = transform.localEulerAngles.x;
        playerDto_CREQ.rotY = transform.localEulerAngles.y;
        playerDto_CREQ.rotZ = transform.localEulerAngles.z;

        //发送消息给Net模块 进行玩家位置相关信息的同步
        socketMsg.Change(OpCode.Player, PlayerCode.PLAYER_SYNC_POS_ROT_CREQ, playerDto_CREQ);
        Dispatch(AreaCode.NET, 0, socketMsg);
    }
}
