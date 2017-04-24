using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Renshaw.Service
{
    public class UserService:GameService,IUserService
    {
        public UserService(ILifetimeScope scope) 
            : base(scope)
        {

        }

        public void GetUser(int userId)
        {
            
        }
    }
}
