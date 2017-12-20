using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.Code;
using TankServer.Cache;
using Protocol.Dto;

namespace TankServer.Controller
{
    class GameHandler :IHandler
    {
        RoomCache roomCache = Caches.roomCache;

        public void OnDisconnect(ClientPeer client)
        {
            //意外离开房间

        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case GameCode.GAMESTART_CREQ:
                    GameStart(client);
                    break;
                case GameCode.GAMEOVER_CREQ:
                    GameOver(client,value);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 游戏开始
        /// </summary>
        /// <param name="client"></param>
        private void GameStart(ClientPeer client)
        {
            //游戏开始之后将当前房间设置为进行状态 防止其他人加入游戏又没有房主
            roomCache.GetRoomByClient(client).isPlay = true;
            roomCache.BroadcastMessageToAll(client,OpCode.Game,GameCode.GAMESTART_SRES,null);
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="client"></param>
        private void GameOver(ClientPeer client,object value)
        {
            //游戏失败 需要将当前客户端从当前房间移除 并且让当前玩家控制的坦克在地图上消失
            roomCache.BroadcastMessage(client,OpCode.Game,GameCode.GAMEOVER_SRES,value);
            roomCache.OutRoom(roomCache.GetRoomByClient(client), client);
            //如果房间里面只剩下一个人 则为胜利者
            if (roomCache.GetRoomByClient(client) != null && roomCache.GetRoomByClient(client).userDic.Count == 1)
            {
                roomCache.GetRoomByClient(client).userDic.Keys.First().Send(OpCode.Game, GameCode.GAMEVICTORY, null);
                roomCache.OutRoom(roomCache.GetRoomByClient(client), client);
            }

        }
    }
}
