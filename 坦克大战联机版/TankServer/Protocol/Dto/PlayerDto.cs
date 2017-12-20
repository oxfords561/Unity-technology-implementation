using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
    public class PlayerDto
    {
        public int color = 0;
        //位置信息
        public float posX;
        public float posY;
        public float posZ;

        //旋转信息
        public float rotX;
        public float rotY;
        public float rotZ;

        public PlayerDto()
        {

        }

        public PlayerDto(int color,float posX,float posY,float posZ,float rotX,float rotY,float rotZ)
        {
            this.color = color;
            this.posX = posX;
            this.posY = posY;
            this.posZ = posZ;
            this.rotX = rotX;
            this.rotY = rotY;
            this.rotZ = rotZ;
        }
    }
}
