using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace Renshaw.Commom
{
    public class UserService
    {
        private CacheCollection userCache;
        public UserService()
        {
            Init();
        }

        public void Load(int userId)
        {
            if (userCache[userId.ToString()] != null)
                return;
            //load from mysql
            User user;
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                user = connection.SingleById<User>(userId);
            }
            //add to cache
            userCache.Add(userId.ToString(), user);
            //add to redis
            var redisDb = GameManager.RedisMultiplexer.GetDatabase(0);
            var key = user.GetType().FullName;
            var field = userId;
            var value = ProtoBufUtils.Serialize(user);
            redisDb.HashSet(key, field, value);

        }

        public void Unload(int userId)
        {
            //remove from cache
            userCache.Remove(userId.ToString());
            //remove from redis
            var redisDb = GameManager.RedisMultiplexer.GetDatabase(0);
            var key = typeof(User).FullName;
            redisDb.HashDelete(key, userId);
        }

        public void Init()
        {
            userCache = new CacheCollection(1000);
        }

        public User GetUser(int userId)
        {
            //get from cache
            return (User)userCache[userId.ToString()];
        }

        public void UpdateUser(int userId)
        {
            //update cache,   already updated,then call this method

            //update redis
            var user = GetUser(userId);
            var redisDb = GameManager.RedisMultiplexer.GetDatabase(0);
            var key = user.GetType().FullName;
            var field = userId;
            var value = ProtoBufUtils.Serialize(user);
            redisDb.HashSet(key, field, value);

            //update mysql
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                connection.Update(user);
            }
        }
        public User CreateUser(int userId)
        {
            return new User
            {
                UserId = userId,
                Level = 1,
                NickName = "新兵#" + userId.ToString()
            };
        }

        public void AddUser(int userId)
        {
            var user = CreateUser(userId);
            //add to cache
            userCache.Add(userId.ToString(), user);
            //add to redis
            var redisDb = GameManager.RedisMultiplexer.GetDatabase(0);
            var key = user.GetType().FullName;
            var field = userId;
            var value = ProtoBufUtils.Serialize(user);
            redisDb.HashSet(key, field, value);

            //add to mysql
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                connection.Insert(user);
            }
        }

        
    }

    
}
