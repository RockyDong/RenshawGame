using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Renshaw.Service
{
    public class MailService:GameService,IMailService
    {
        public MailService(ILifetimeScope scope) 
            : base(scope)
        {

        }

        public void OpenMail(int userId, int mailId)
        {
            
        }

        public void DeleteMail(int userId, int mailId)
        {

        }
    }
}
