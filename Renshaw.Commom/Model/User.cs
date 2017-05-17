using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom
{
    public class User : IUserSingleEntity
    {
        public int UserId { get; set; }

        public string NickName { get; set; }

        public int Level { get; set; }

        public int CurrentExp { get; set; }

        public int Coin { get; set; }

        public int Diamond { get; set; }
    }
}
