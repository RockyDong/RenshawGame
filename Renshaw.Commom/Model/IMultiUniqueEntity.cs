﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom
{
    public interface IMultiUniqueEntity
    {
        int UserId { get; set; }
        long UniqueId { get; set; }

        string GetIdentity();

        string GetSubIdentity();
    }
}
