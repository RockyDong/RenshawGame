using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Renshaw.Test
{
    class Program
    {
        private static int _result;
        static void Main(string[] args)
        {
//            TestMaxThreads();
            //TestInterlock();
            int[] testsInts = new int[1024*1024*250];
            for (int i = 0; i < testsInts.Length; i++)
            {
                testsInts[i] = i;
            }
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
                    Attach = new List<Reward>() { new Reward() { Type = 10, Amount = 1, Id = 100301 }, new Reward() { Type = 9, Amount = 1, Id = 100303 } },
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
