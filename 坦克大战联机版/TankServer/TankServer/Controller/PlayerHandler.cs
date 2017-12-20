using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AhpilyServer;
using Protocol.Code;
using TankServer.Cache;

namespace TankServer.Controller
{
    public class PlayerHandler : IHandler
    {

        RoomCache roomCache = Caches.roomCache;

        public void OnDisconnect(ClientPeer client)
        {
            
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case PlayerCode.PLAYER_SYNC_POS_ROT_CREQ:
                    SyncPositionAndRotation(client,value);
                    break;
                case PlayerCode.PLAYER_SYNC_LIFE_CREQ:
                    SyncLife(client,value);
                    break;
                case PlayerCode.BULLET_SYNC_POS_ROT_CREQ:
                    SyncBulletPositionAndRotation(client, value);
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 同步玩家生命值
        /// </summary>
        /// <param name="value"></param>
        void SyncLife(ClientPeer client,object value)
        {
            roomCache.BroadcastMessageToAll(client,OpCode.Player,PlayerCode.PLAYER_SYNC_LIFE_SREC,value);
        }
        /// <summary>
        /// 同步玩家位置消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="value"></param>
        void SyncPositionAndRotation(ClientPeer client,object value)
        {
            roomCache.BroadcastMessage(client,OpCode.Player, PlayerCode.PLAYER_SYNC_POS_ROT_SREC, value);
        }

        /// <summary>
        /// 同步子弹位置消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="value"></param>
        void SyncBulletPositionAndRotation(ClientPeer client, object value)
        {
            roomCache.BroadcastMessage(client, OpCode.Player, PlayerCode.BULLET_SYNC_POS_ROT_SREC, value);
        }
    }
}
