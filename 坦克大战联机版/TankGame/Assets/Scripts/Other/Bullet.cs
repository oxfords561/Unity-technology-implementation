using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PlayerBase {

    private SocketMsg socketMsg = new SocketMsg();

    void Start () {
        Destroy(gameObject,3f);
	}


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //给服务器发送扣血命令
            //发送消息给Net模块 进行玩家位置相关信息的同步
            socketMsg.Change(OpCode.Player, PlayerCode.PLAYER_SYNC_LIFE_CREQ,collision.gameObject.name);
            Dispatch(AreaCode.NET, 0, socketMsg);
        }
        else
        {
            //击打在墙壁或者地面进行爆炸特效

        }
    }
}
