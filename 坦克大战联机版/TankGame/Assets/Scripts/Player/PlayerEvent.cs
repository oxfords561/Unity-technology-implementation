using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent {

    //初始化玩家信息
    public const int PLAYER_INITDATA = 0;
    //玩家标识信息
    public const int PLAYER_IDENTIFY = 1;
    //玩家位置旋转信息同步
    public const int PLAYER_SYNC_POS_ROT = 2;
    //玩家血量信息的同步
    public const int PLAYER_SYNC_LIFE = 3;
    //子弹信息同步
    public const int BULLET_SYNC_POS_ROT = 4;
    //玩家失败退场 消失 同步
    public const int PLAYER_DESTROY = 5;

}
