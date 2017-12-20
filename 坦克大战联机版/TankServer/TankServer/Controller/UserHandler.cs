using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.Code;
using Protocol.Dto;
using TankServer.DAO;
using TankServer.Model;
using TankServer.Cache;

namespace TankServer.Controller
{
    /// <summary>
    /// 账号处理的逻辑层
    /// </summary>
    class UserHandler : IHandler
    {
        UserCache userCache = Caches.user;
        RoomCache roomCache = Caches.roomCache;
        private UserDAO userDAO = new UserDAO();
        private RoomDto roomDto = new RoomDto();
        private int color = 0;

        public void OnDisconnect(ClientPeer client)
        {
            if (userCache.IsOnline(client))
                userCache.Offline(client);
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case UserCode.REGIST_CREQ:
                    {
                        UserDto dto = value as UserDto;
                        Regist(client,dto.username,dto.password);
                    }
                    break;
                case UserCode.LOGIN:
                    {
                        UserDto dto = value as UserDto;
                        Login(client, dto.username, dto.password);
                    }
                    break;
                case UserCode.READY_CREQ:
                    {
                        UserDto dto = value as UserDto;
                        Ready(client,dto.username,dto.password);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        private void Login(ClientPeer client, string username, string password)
        {
            SingleExecute.Instance.Execute(() =>
            {
                bool res = userDAO.GetUserByUsername(client.MySqlConn, username);
                User user = userDAO.VerifyUser(client.MySqlConn, username, password);

                if (!res)
                {
                    client.Send(OpCode.User, UserCode.LOGIN, -1);//-1 账号不存在
                    return;
                }

                if (userCache.IsOnline(username))
                {
                    //表示账号在线
                    client.Send(OpCode.User, UserCode.LOGIN, -2);
                    return;
                }

                if (user == null)
                {
                    //表示账号密码不匹配
                    client.Send(OpCode.User, UserCode.LOGIN, -3);
                    return;
                }

                //登陆成功
                userCache.Online(client, username);
                client.Send(OpCode.User, UserCode.LOGIN, 0);
            });
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="client"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        private void Regist(ClientPeer client,string username,string password)
        {
            SingleExecute.Instance.Execute(() =>
            {
                bool res = userDAO.GetUserByUsername(client.MySqlConn,username);
                if(res)
                {
                    //表示当前账号存在
                    client.Send(OpCode.User,UserCode.REGIST_SRES,-1);//-1 代表账号已经存在
                    return;
                }

                if (string.IsNullOrEmpty(username))
                {
                    //表示当前账号不合法
                    client.Send(OpCode.User,UserCode.REGIST_SRES,-2);//-2 代表账号不合法
                    return;
                }

                if (string.IsNullOrEmpty(password) || password.Length < 4 || password.Length > 16)
                {
                    //表示密码不合法
                    client.Send(OpCode.User, UserCode.REGIST_SRES, -3);//-3 表示账号密码不合法
                    return;
                }

                //可以注册了
                userDAO.AddUser(client.MySqlConn,username,password);
                client.Send(OpCode.User, UserCode.REGIST_SRES, 0);//0 代表注册成功
            });
        }

        /// <summary>
        /// 用户准备
        /// </summary>
        /// <param name="client"></param>
        private void Ready(ClientPeer client, string username, string password)
        {
            color++;//将颜色赋予参加房间游戏的玩家 TODO 多线程时需注意color的状态锁
            if (color > 4)
            {
                color = 1;
            }

            roomCache.EnterRoom(client,username,password);

            //将已准备的人员数量广播到房间其他人
            roomDto.userList = roomCache.GetRoomByClient(client).userList;
            roomDto.color = color;

            Console.WriteLine(roomDto.userList[0].username);

            roomCache.BroadcastMessageToAll(client,OpCode.User, UserCode.READY_SRES, roomDto);
            //如果是创建人或者第一个准备的人作为房主 开放房主按钮进行使用
            roomCache.GetRoomByClient(client).userDic.Keys.First().Send(OpCode.User,UserCode.HOUSEOWNER,0);
        }
    }
}
