using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
    public class RoomDto
    {
        public List<UserDto> userList = new List<UserDto>();
        public int color = 0;//0标识默认 没有颜色 1、红色 2、黄色 3、绿色 4、蓝色

        public RoomDto()
        {

        }

        public RoomDto(List<UserDto> list)
        {
            this.userList = list;
        }
    }
}
