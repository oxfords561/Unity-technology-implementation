using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Code
{
    /// <summary>
    /// 关于账号模块的具体操作码申明
    /// </summary>
    public class UserCode
    {
        //注册操作码
        public const int REGIST_CREQ = 0;//注册信息申请
        public const int REGIST_SRES = 1;//注册信息反馈

        //登录操作码
        public const int LOGIN = 2;//

        //准备游戏操作码
        public const int READY_CREQ = 3; //准备游戏申请
        public const int READY_SRES = 4; //准备游戏申请反馈 同步客户端信息

        //房主标识
        public const int HOUSEOWNER = 5;
    }
}
