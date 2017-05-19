using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.MySql;

namespace Renshaw.Commom.Model
{
    public class SingleEntityBase<T> where T : ISingleEntity, new()
    {
        private ConcurrentDictionary<string, T> cache;

        public SingleEntityBase()
        {
            cache = new ConcurrentDictionary<string, T>(Environment.ProcessorCount * 2, 100);
        }

        public void Load(string key)
        {
            
        }

        public void LoadAll()
        {
            List<T> ts = null;
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                ts = connection.Select<T>();
            }
            foreach (dynamic t in ts)
            {
                cache[t.GetIdentity()] = t;
            }
        }

        public void Unload()
        {

        }

        public T Get(string key)
        {
            T t;
            cache.TryGetValue(key, out t);
            return t;
        }

        public bool Set(string key, T value)
        {
            cache[key] = value;
            return true;
        }

        public void AddOrUpdate(string key, T value)
        {
            var t = Get(key);
            cache[key] = value;
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                if (t == null)
                {
                    var valueBytes = ProtoBufUtils.Serialize(value);
                    //add
                    var mysqlCommand = new MysqlCommand();
                    mysqlCommand.TypeFullName = typeof(T).FullName;
                    mysqlCommand.CommandType = "insert";
                    mysqlCommand.DataBytes = valueBytes;

                    GameManager.MysqlQueue.Enqueue(mysqlCommand);

                    var redisCommand = new RedisCommand();
                    redisCommand.Key = typeof(T).FullName;
                    redisCommand.Field = "insert";
                    redisCommand.DataBytes = valueBytes;

                    connection.Insert(value);
                }
                else
                {
                    //update
                    connection.Update(value);
                }
            }

        }

        public void Delete()
        {

        }

        public T GetOrAdd(string key, T value = default(T))
        {
            var t = Get(key);
            if (t == null)
            {
                if (value == null)
                    throw new Exception("add value is null");
                t = value;
                //add
                Add(key, value);
            }
            return t;
        }

        public void Add(string key, T value)
        {
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                connection.Insert(value);
            }
            cache[key] = value;
        }
    }
}
