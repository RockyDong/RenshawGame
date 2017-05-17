using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Renshaw.Commom;
using ServiceStack.OrmLite;

namespace Renshaw.Test
{
    class Program
    {
        private static int _result;

        static void Main(string[] args)
        {
            //            TestMaxThreads();
            //TestInterlock();
            /*int[] testsInts = new int[1024*1024*250];
            for (int i = 0; i < testsInts.Length; i++)
            {
                testsInts[i] = i;
            }*/
            /*            new RemoteRedisAccessTest("120.132.57.85:32309,password=ldlsl21WEldsis898239234sdalkakA332e",9,"外网redis访问测试").Test();
                        new RemoteRedisAccessTest("192.168.1.216:6780,password=glee1234", 9, "局域网redis访问测试").Test();
                        new RemoteRedisAccessTest("localhost", 9, "本机redis访问测试").Test();*/

            /*var list = new int[6];
            list[5] = 3;*/

            /*  var mail = CreateMail(5);
              mail.MailId = mail.MailId;
  //            var attachBytes = ProtoBufUtils.Serialize(mail.Attach);
              string connectStr = "server=localhost;database=renshaw;username=root;password=123456;Allow User Variables=True";
             using(IDbConnection connection = new MySqlConnection(connectStr))
              {
                  connection.Open();
                  connection.Execute("insert into mail(UserId,MailId,Title,Sender,Content,Attach,ValidMinute,CreateAt) values('98792',@MailId,@Title,@Sender,@Content,@Attach,@ValidMinute,@CreateAt)", 
                      mail);
                  string sql =
                      "select UserId,MailId,Title,Sender,Content,Attach,ValidMinute,CreateAt from mail where MailId = 2";
  //                var reader = connection.ExecuteReader(sql);
                  var mails = connection.Query<Mail>(sql);
                  foreach (var mail1 in mails)
                  {
                      var attach = ProtoBufUtils.Deserialize<List<Reward>>(mail.Attach);
                      Console.WriteLine($"{attach.Count}");
                  }
  
              }*/

            /*var mail = CreateMail(6);
            var db = GameManager.RedisMultiplexer.GetDatabase(0);
            var mailBytes = ProtoBufUtils.Serialize(mail);
            db.HashSet(mail.GetType().FullName, mail.UserId, mailBytes);
            using (IDbConnection connection = GameManager.DbFactory.OpenDbConnection())
            {
                connection.Insert(mail);
            }*/
            //ITest //接口默认为internal

            int testNum = 1000000;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Dictionary<long,bool> dict = new Dictionary<long, bool>(testNum);
            for (int i = 0; i < testNum; i++)
            {
                bool result;
                long uniqueId = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
                if (!dict.TryGetValue(uniqueId, out result))
                    dict[uniqueId] = true;
                else
                {
                    Console.WriteLine("repeated!");
                }

            }
            sw.Stop();
            Console.WriteLine($"{sw.ElapsedMilliseconds}ms  {sw.ElapsedTicks} ticks");
            Console.WriteLine("not repeated!");
            Console.Read();
        }

        private static void TestMaxThreads()
        {
            while (true)
            {
                (new Thread(Start)).Start();
            }
            /*for (int i = 0; i < 6400; i++)
            {
                (new Thread(Start)).Start();
            }*/
        }

        private static void Start()
        {
            try
            {
                var mailList = new List<Mail>();
                for (int i = 0; i < 10000; i++)
                {
                    var mail = CreateMail(i);
                    mailList.Add(mail);
                }
                Console.WriteLine(Interlocked.Increment(ref _result));
                while (true)
                {
                    try
                    {
                        Thread.Sleep(int.MaxValue);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e + "51");
                        break;
                    }
                }
                foreach (var mail in mailList)
                {
                    Console.WriteLine(mail.ValidMinute + "分钟");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "62");
                throw;
            }
            
        }

        private static Mail CreateMail(int mailId)
        {
            Mail mail = null;
            try
            {
                mail = new Mail()
                {
                    MailId = mailId,
                    UserId = 98791,
                    Title = "万圣节南瓜收集补偿奖励",
                    Sender = "系统",
                    Content = "由于一二三四五六七八九，因此十百千万亿，故而逗乐咪发嗖拉西，导致全国人民都注意了:" +
                         "今天买了一个新的机械键盘，茶轴的，按键比较轻，所以用起来应该会是打字挺舒服的，" +
                         "不知道使用一段时间之后会是什么样子，应该是会很好用的吧之前那个黑轴的和这个相比" +
                         "确实是难摁好多呢这个打字看起来清清爽爽的，没有什么负担的感觉好像键位之间的间隔" +
                         "相比之前那个要小了很多，不清楚是不是变方便了还是麻烦了，想来是方便了的，因为自" +
                         "己的手还是比较小的嘛。",
                    Attach = ProtoBufUtils.Serialize(new List<Reward>() { new Reward() { Type = 10, Amount = 1, Id = 100301 }, new Reward() { Type = 9, Amount = 1, Id = 100303 } }),
                    CreateAt = DateTime.Now,
                    ValidMinute = 4800
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "throw the error from here");
                throw;
            }
            return mail;

        }

        private static void TestInterlock()
        {
            while (true)
            {
                Task[] _tasks = new Task[100];
                int i = 0;

                for (i = 0; i < _tasks.Length; i++)
                {
                    _tasks[i] = Task.Factory.StartNew((num) =>
                    {
                        var taskid = (int)num;
                        Work(taskid);
                    }, i);
                }

                Task.WaitAll(_tasks);
                Console.WriteLine(_result);

                Console.ReadKey();
            }
        }
        //线程调用方法
        private static void Work(int TaskID)
        {
            for (int i = 0; i < 10; i++)
            {
                //                _result++;
                Interlocked.Increment(ref _result);
            }
        }
    }
}
