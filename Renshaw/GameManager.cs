using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace Renshaw
{
    public static class GameManager
    {
        private static ConnectionMultiplexer redis;
        private static readonly object locker = new object();

        /// <summary>
        /// redis单例
        /// </summary>
        public static ConnectionMultiplexer RedisMultiplexer
        {
            get
            {
                if (redis == null)
                {
                    lock (locker)
                    {
                        redis = ConnectionMultiplexer.Connect("localhost");
                    }
                }
                return redis;
            }
        }

    }
}
