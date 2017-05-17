using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf;
using Renshaw.Commom.Model;
using ServiceStack.DataAnnotations;

namespace Renshaw.Commom
{
    public class Mail : IMultiEntity
    {
        public int SourceId { get; set; }
        public int UserId { get; set; }
        public string Sender { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Reward> Attach { get; set; }
        public DateTime CreateAt { get; set; }
        public int ValidMinute { get; set; }
        public MailStatus Status { get; set; }
    }

    public class SystemMail : IShareEntity
    {
        public SystemMail()
        {

        }
        [AutoIncrement]
        public int IndexId { get; set; }
        [PrimaryKey]
        public long UniqueId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Reward> Attach { get; set; }
        public DateTime CreateAt { get; set; }
        public int ValidMinute { get; set; }
    }

    public enum MailStatus
    {
        Unread = 0,
        Readed = 1
    }
    [ProtoContract]
    public class Reward
    {
        [ProtoMember(1)]
        public int Type { get; set; }
        [ProtoMember(2)]
        public int Id { get; set; }
        [ProtoMember(3)]
        public int Amount { get; set; }
    }
}
