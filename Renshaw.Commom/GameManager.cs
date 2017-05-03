using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using StackExchange.Redis;

namespace Renshaw.Commom
{
    public static class GameManager
    {
        private static ConnectionMultiplexer redis;
        private static OrmLiteConnectionFactory dbFactory;
        private static readonly object RedisLocker = new object();
        private static readonly object MysqlLocker = new object();
        /// <summary>
        /// redis单例
        /// </summary>
        public static ConnectionMultiplexer RedisMultiplexer
        {
            get
            {
                if (redis == null)
                {
                    lock (RedisLocker)
                    {
                        redis = ConnectionMultiplexer.Connect("localhost");
                    }
                }
                return redis;
            }
        }
        /// <summary>
        /// mysql单例
        /// </summary>
        public static OrmLiteConnectionFactory DbFactory
        {
            get
            {
                if (dbFactory == null)
                {
                    lock (MysqlLocker)
                    {
                        dbFactory = new OrmLiteConnectionFactory("server=localhost;database=renshaw;username=root;password=123456", MySqlDialect.Provider);
                    }
                }
                return dbFactory;
            }
        }
    }
}
