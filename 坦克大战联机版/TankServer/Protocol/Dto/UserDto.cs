using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class UserDto
    {
        public string username;
        public string password;

        public UserDto()
        {
            
        }

        public UserDto(string username, string pwd)
        {
            this.username = username;
            this.password = pwd;
        }
    }
}
