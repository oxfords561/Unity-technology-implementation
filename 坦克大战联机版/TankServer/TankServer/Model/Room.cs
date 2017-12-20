using AhpilyServer;
using Protocol.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankServer.Model
{
    public class Room
    {
        public Dictionary<ClientPeer, UserDto> userDic = new Dictionary<ClientPeer, UserDto>();
        public List<UserDto> userList = new List<UserDto>();
        public List<UserDto> loserList = new List<UserDto>();
        public bool isPlay = false;

        public Room()
        {

        }

        public Room(Dictionary<ClientPeer,UserDto> userDic,List<UserDto> userList,List<UserDto> loserList)
        {
            this.userDic = userDic;
            this.userList = userList;
            this.loserList = loserList;
        }
    }
}
