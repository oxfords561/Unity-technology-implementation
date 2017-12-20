using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    public class GameCode
    {
        //游戏开始 申请
        public const int GAMESTART_CREQ = 0;
        //游戏开始 反馈
        public const int GAMESTART_SRES = 1;

        //游戏结束 申请
        public const int GAMEOVER_CREQ = 2;
        //游戏结束 反馈
        public const int GAMEOVER_SRES = 3;
        //游戏胜利
        public const int GAMEVICTORY = 4;
    }
}
