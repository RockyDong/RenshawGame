using System;
using System.Collections.Generic;
using System.Text;

namespace Renshaw.MemoryModel
{
    class RedisEntityItem
    {
        public string HashId { get; set; }
        public int UserId { get; set; }

        public byte[] KeyBytes { get; set; }
        public byte[] ValueBytes { get; set; }

        public int State { get; set; }

        public bool HasMutilKey { get; set; }
    }
}
