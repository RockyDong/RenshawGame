using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Renshaw.Service
{
    public abstract class GameService:IGameService
    {
        protected GameService(ILifetimeScope scope)
        {
            this.ServiceScope = scope;
        }

        public ILifetimeScope ServiceScope { get; }
    }
}
