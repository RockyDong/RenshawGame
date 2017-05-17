using Renshaw.Commom.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom
{
    public interface IUserSingleEntity:ISingleEntity
    {
        int UserId { get; set; }
    }

    
}
