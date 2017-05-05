using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
namespace Renshaw.Commom
{
    public class MailService
    {
        private CacheCollection mailCache;
        public MailService()
        {
            Init();
        }

        public void Load(int userId)
        {
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                var mails = connection.Select<Mail>(m => m.UserId == userId);
                var mailDict = new ConcurrentDictionary<int, object>();
                foreach (var mail in mails)
                {
                    mailDict.TryAdd(mail.MailId, mail);
                }
                mailCache.Add(userId.ToString(), mailDict);
            }
        }

        public void Unload(int userId)
        {
            mailCache.Remove(userId.ToString());
        }

        public void Init()
        {
            mailCache = new CacheCollection(1000);
        }

        public void OpenMail(int userId, int mailId)
        {
            var mailDict = (ConcurrentDictionary<int, object>) mailCache[userId.ToString()];
            var mail = mailDict[mailId];
        }

        public void DeleteMail(int userId, int mailId)
        {

        }
    }
}
