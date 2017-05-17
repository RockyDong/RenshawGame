using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using StackExchange.Redis;

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
            //load from mysql
            List<Mail> mails;
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                mails = connection.Select<Mail>(m => m.UserId == userId);
            }
            //add to cache
            var mailDict = new ConcurrentDictionary<int, object>();
            foreach (var mail in mails)
            {
                mailDict.TryAdd(mail.SourceId, mail);
            }
            mailCache.Add(userId.ToString(), mailDict);
            //add to redis
            string key = typeof(Mail).FullName;
            
            List<HashEntry> hashEntries = new List<HashEntry>();
            foreach (var mail in mails)
            {
                string field = mail.UserId.ToString() + "_" + mail.SourceId.ToString();
                byte[] value = ProtoBufUtils.Serialize(mail);
                hashEntries.Add(new HashEntry(field, value));
            }
            var redisDb = GameManager.RedisMultiplexer.GetDatabase(0);
            
            redisDb.HashSet(key, hashEntries.ToArray());
            //这种结构情况下，怎样从redis加载到内存???
        }

        public void Unload(int userId)
        {
            mailCache.Remove(userId.ToString());
        }

        public void Init()
        {
            mailCache = new CacheCollection(1000);
        }

        public List<Mail> GetMails(int userId)
        {
            var mailDict = mailCache[userId.ToString()] as ConcurrentDictionary<int, object>;
            List<Mail> mailList = new List<Mail>();
            foreach (var mailKeyValuePair in mailDict)
            {
                mailList.Add((Mail)mailKeyValuePair.Value);
            }
            return mailList;
        }

        public Mail GetMail(int userId, int sourceId)
        {
            var mailDict = mailCache[userId.ToString()] as ConcurrentDictionary<int, object>;
            Mail mail = null;
            if (mailDict != null)
            {
                mail = (Mail)mailDict[sourceId];
            }
            return mail;
        }

        public void OpenMail(int userId, int mailId)
        {
            var mailDict = (ConcurrentDictionary<int, object>) mailCache[userId.ToString()];
            var mail = mailDict[mailId];
        }

        public void DeleteMail(int userId, int mailId)
        {
            var mailDict = (ConcurrentDictionary<int, object>)mailCache[userId.ToString()];
            object mail;
            mailDict.TryRemove(mailId, out mail);
        }
    }
}
