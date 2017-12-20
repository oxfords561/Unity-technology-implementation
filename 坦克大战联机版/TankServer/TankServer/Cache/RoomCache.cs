using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.Dto;
using TankServer.Model;

namespace TankServer.Cache
{
    class RoomCache
    {
        public List<Room> roomList = new List<Room>();
        private Room room = new Room();//第一个房间

        public void EnterRoom(ClientPeer client, string username, string password)
        {
            if(roomList.Count == 0)
            {
                AddRoomToList(room, client, username, password);
                return;
            }
               
            if (roomList[roomList.Count - 1].isPlay || isLoserJoin(username) || roomList[roomList.Count - 1].userList.Count >= 4)
            {
                Room roomOther = new Room();
                AddRoomToList(roomOther, client, username, password);
            }
            else
            {                  
                AddRoomToList(roomList[roomList.Count - 1], client, username, password);
            }
        }

        /// <summary>
        /// 判断是否是从之前房间游戏失败退下来的玩家
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool isLoserJoin(string username)
        {
            Console.WriteLine("失败者的名字 "+username);
            //进行失败者加入游戏的判断
            for (int i = 0; i < roomList[roomList.Count - 1].loserList.Count; i++)
            {
                if (username.Equals(roomList[roomList.Count - 1].loserList[i].username))
                {
                    Console.WriteLine("失败者想参加游戏");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 添加房间到房间列表
        /// </summary>
        /// <param name="roomTemp"></param>
        /// <param name="client"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        void AddRoomToList(Room roomTemp, ClientPeer client, string username, string password)
        {
            if (roomTemp.userDic.ContainsKey(client))
            {
                return;
            }

            UserDto dto = new UserDto(username, password);
            roomTemp.userDic.Add(client, dto);
            roomTemp.userList.Add(dto);
            roomTemp.loserList.Add(dto);
            roomList.Add(roomTemp);
        }

        public void OutRoom(Room roomTemp, ClientPeer client)
        {
            if (roomTemp.userList.Count <= 1)
            {
                roomList.Remove(roomTemp);
                if (roomTemp == room)
                {
                    room.userDic.Clear();
                    room.userList.Clear();
                }
                return;
            }

            if (roomTemp.userDic.ContainsKey(client))
            {
                roomTemp.userList.Remove(roomTemp.userDic[client]);
                roomTemp.userDic.Remove(client);
            }

        }

        /// <summary>
        /// 广播到除去自身的房间其他玩家
        /// </summary>
        /// <param name="client"></param>
        /// <param name="opCode"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        public void BroadcastMessage(ClientPeer client, int opCode, int subCode, object value)
        {
            SingleExecute.Instance.Execute(() =>
            {
                Room roomTemp = GetRoomByClient(client);
                if (roomTemp == null)
                    return;

                if (roomTemp.userDic.Count <= 0)
                    return;

                foreach (var clientItem in roomTemp.userDic)
                {
                    if (clientItem.Key != client)
                        clientItem.Key.Send(opCode, subCode, value);
                }
            });
        }

        /// <summary>
        /// 广播到房间的所有玩家
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        public void BroadcastMessageToAll(ClientPeer client, int opCode, int subCode, object value)
        {
            SingleExecute.Instance.Execute(() =>
            {
                Room roomTemp = GetRoomByClient(client);

                if (roomTemp == null)
                    return;

                foreach (var clientItem in roomTemp.userDic)
                {
                    clientItem.Key.Send(opCode, subCode, value);
                }
            });
        }

        /// <summary>
        /// 通过客户端连接找到所属房间
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public Room GetRoomByClient(ClientPeer client)
        {
            int index = 0;
            for (int roomItem = 0; roomItem < roomList.Count; roomItem++)
            {
                foreach (ClientPeer key in roomList[roomItem].userDic.Keys)
                {
                    if(client == key)
                    {
                        index = roomItem;
                    }
                }
            }

            if (roomList.Count > 0)
                return roomList[index];
            return null;
        }
    }
}
