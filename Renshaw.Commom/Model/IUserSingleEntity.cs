using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom
{
    public interface IUserSingleEntity
    {
        int UserId { get; set; }

        string GetIdentity();
    }

    
}
