using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
namespace Renshaw.Commom
{
    public static class SyncUtils
    {
        public static void SyncRedis(string key, int field, byte[] value)
        {
            var redisDb = GameManager.RedisMultiplexer.GetDatabase(0);
            redisDb.HashSet(key, field, value);
        }

        public static void SyncMysql(DbOperation operation, object obj)
        {
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                switch (operation)
                {
                    case DbOperation.Add:
                        connection.Insert(obj);
                        break;
                    case DbOperation.Delete:
                        connection.Delete(obj);
                        break;
                    case DbOperation.Update:
                        connection.Update(obj);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public enum DbOperation
    {
        Add = 0,
        Delete = 1,
        Get = 2,
        Update = 3
    }
}
