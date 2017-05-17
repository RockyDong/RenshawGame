using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace Renshaw.Commom.Model
{
    public class ShareEntity<T> where T : IShareEntity
    {
        private ConcurrentDictionary<string, object> shareCache;
        public ShareEntity()
        {
            shareCache = new ConcurrentDictionary<string, object>(Environment.ProcessorCount * 2, 1000);
            //UniqueId = GenerateUtils.CreateUniqueId();
        }
        //public long UniqueId { get; set; }

        public void Load()
        {
            List<T> objects = null;
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                objects = connection.Select<T>();
            }
            foreach (var o in objects)
            {
                shareCache[o.UniqueId.ToString()] = o;
            }
        }

        public void Unload()
        {

        }

        public void Reload()
        {

        }

        public T Get(long uniqueId)
        {
            return (T)shareCache[uniqueId.ToString()];
        }
    }
}
