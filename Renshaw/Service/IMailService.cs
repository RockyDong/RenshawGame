using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Renshaw.Service
{
    public interface IMailService
    {
        void OpenMail(int userId, int mailId);
        void DeleteMail(int userId, int mailId);
    }
}
