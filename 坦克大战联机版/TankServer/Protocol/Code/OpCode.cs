using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    /// <summary>
    /// 服务器各种模块识别码的申明
    /// </summary>
    public class OpCode
    {
        //账号模块
        public const int User = 0;
        //游戏模块
        public const int Game = 1;
        //玩家模块
        public const int Player = 3;
    }
}
