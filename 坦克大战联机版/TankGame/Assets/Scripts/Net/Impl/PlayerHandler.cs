using Protocol.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : HandlerBase
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case PlayerCode.PLAYER_SYNC_POS_ROT_SREC:
                SyncPositionAndRotation(value);
                break;
            case PlayerCode.BULLET_SYNC_POS_ROT_SREC:
                SyncBulletPositionAndRotation(value);
                break;
            case PlayerCode.PLAYER_SYNC_LIFE_SREC:
                SyncLife(value);
                break;
            default:
                break;
        }
    }

    void SyncLife(object value)
    {
        //通知玩家模块开始同步血量数据数据
        Dispatch(AreaCode.CHARACTER, PlayerEvent.PLAYER_SYNC_LIFE, value);
    }

    void SyncPositionAndRotation(object value)
    {
        //通知玩家模块开始同步数据
        Dispatch(AreaCode.CHARACTER, PlayerEvent.PLAYER_SYNC_POS_ROT, value);
    }

    void SyncBulletPositionAndRotation(object value)
    {
        //通知玩家模块开始同步数据
        Dispatch(AreaCode.CHARACTER, PlayerEvent.BULLET_SYNC_POS_ROT, value);
    }
}