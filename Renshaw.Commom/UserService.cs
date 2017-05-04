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
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                var user = connection.SingleById<User>(userId);
                userCache.Add(userId.ToString(), user);
            }
        }

        public void Unload(int userId)
        {
            
        }

        public void Init()
        {
            userCache = new CacheCollection(1000);
        }

        public User GetUser(int userId)
        {
            return (User)userCache[userId.ToString()];
        }
    }
}
