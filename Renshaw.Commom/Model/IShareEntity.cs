using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom
{
    public interface IShareEntity
    {
        long UniqueId { get; set; }
        string GetIdentity();
    }
}
