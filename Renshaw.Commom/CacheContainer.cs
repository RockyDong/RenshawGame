﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom
{
    public class CacheContainer
    {
        private CacheCollection collection;

        private ITest test;
        
        public CacheContainer()
        {
            collection = new CacheCollection(3000);
            
        }
    }
}
