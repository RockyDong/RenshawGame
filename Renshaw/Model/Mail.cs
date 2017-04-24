using System;
using System.Collections.Generic;
using System.Text;

namespace Renshaw.Model
{
    public class Mail
    {
        public int MailId { get; set; }
        public int UserId { get; set; }
        public string Sender { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Reward> Attach { get; set; }
        public DateTime CreateAt { get; set; }
        public int ValidMinute { get; set; }
    }

    public class MailData
    {
        public int MailId { get; set; }
        public MailStatus Status { get; set; }
    }

    public enum MailStatus
    {
        Unread = 0,
        Readed = 1
    }

    public class Reward
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public int Amount { get; set; }
    }
}
