using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankServer.Model
{
    /// <summary>
    /// 账号的数据模型
    /// </summary>
    public class User
    {
        public int Id;
        public string username;
        public string password;

        //...创建日期 电话号码

        public User(int id, string username, string password)
        {
            this.Id = id;
            this.username = username;
            this.password = password;
        }
    }
}
