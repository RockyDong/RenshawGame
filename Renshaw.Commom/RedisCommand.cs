using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom
{
    public class RedisCommand
    {
        public string Field { get; set; }

        public string Key { get; set; }

        public byte[] DataBytes { get; set; }
    }
}
