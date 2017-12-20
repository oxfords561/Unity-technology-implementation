using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    public class PlayerCode
    {
        //玩家位置旋转信息的同步 申请
        public const int PLAYER_SYNC_POS_ROT_CREQ = 0;
        //玩家位置旋转信息的同步 反馈
        public const int PLAYER_SYNC_POS_ROT_SREC = 1;
        //玩家被攻击 申请 扣血命令
        public const int PLAYER_SYNC_LIFE_CREQ = 2;
        //玩家被攻击 反馈 扣血命令
        public const int PLAYER_SYNC_LIFE_SREC = 3;
        //子弹发射位置同步 申请 
        public const int BULLET_SYNC_POS_ROT_CREQ = 4;
        //子弹发射位置同步 反馈
        public const int BULLET_SYNC_POS_ROT_SREC = 5;
    }
}
