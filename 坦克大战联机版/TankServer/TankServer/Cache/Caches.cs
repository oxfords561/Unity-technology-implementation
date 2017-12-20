using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankServer.Cache
{
    class Caches
    {
        public static UserCache user { get; set; }
        public static RoomCache roomCache { get; set; }

        static Caches()
        {
            user = new UserCache();
            roomCache = new RoomCache();
        }
    }
}
