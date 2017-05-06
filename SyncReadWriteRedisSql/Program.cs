using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;

namespace SyncReadWriteRedisSql
{
    class Program
    {
        static void Main(string[] args)
        {
            int mailId = 1;
            var mail = new Mail()
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
                Attach = 1,//new List<Reward>() { new Reward() { Type = 10, Amount = 1, Id = 100301 }, new Reward() { Type = 9, Amount = 1, Id = 100303 } },
                CreateAt = DateTime.Now,
                ValidMinute = 4800
            };
            string connectStr = "server=localhost;database=renshaw;username=root;password=4365617";
            IDbConnection connection = new MySqlConnection(connectStr);
            try
            {
                connection.Open();
                connection.Execute("insert into Mail(UserId,MailId,Titile,Sender,Content,Attach,CreateAt,ValidMinute) values(@UserId,@MailId,@Title,@Sender,@Content,@Attach,@CreateAt,@ValidMinute)", mail);
                //var command = connection.CreateCommand();
                //command.CommandText = "update Mail set";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
