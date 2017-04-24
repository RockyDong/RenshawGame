using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Renshaw.Service
{
    public interface IGameService
    {
        ILifetimeScope ServiceScope { get; }
    }
}
