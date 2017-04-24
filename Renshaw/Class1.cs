using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using MySql.Data.MySqlClient;
using StackExchange.Redis;

namespace Renshaw
{
    public class Class1
    {
        public void Connection()
        {
            IDbConnection connection = new MySqlConnection();
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase(0);
            List<HashEntry> entrys = new List<HashEntry>();
            Dictionary<RedisValue,RedisValue> entries = new Dictionary<RedisValue, RedisValue>();
            entries.Add("UserId",96189);
            entries.Add("MailId",3);
            entries.Add("Status",0);
            foreach (var redisValue in entries)
            {
                entrys.Add(redisValue);
            }
            db.HashSet("PersonalMail:96189", entrys.ToArray());
            var mails = db.HashGetAll("PersonalMail");
        }
    }
}
