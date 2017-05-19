using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom
{
    public class MysqlCommand
    {
        public string CommandType { get; set; }

        public string TypeFullName { get; set; }

        public byte[] DataBytes { get; set; }
    }
}
