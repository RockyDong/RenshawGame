using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Renshaw.Test
{
    public class RemoteRedisAccessTest
    {
        private ConnectionMultiplexer redis;
        private int dbNo;
        private string desc;
        public RemoteRedisAccessTest(string address,int dbNo,string desc)
        {
            redis = ConnectionMultiplexer.Connect(address);
            this.dbNo = dbNo;
            this.desc = desc;
        }

        public void Test()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var db = redis.GetDatabase(dbNo);
            db.SetAdd("dst", "you are so handsome!");
            sw.Stop();
            Console.WriteLine(desc);
            Console.WriteLine(sw.ElapsedMilliseconds + "ms " + sw.ElapsedTicks + " ticks");
        }
    }
}
