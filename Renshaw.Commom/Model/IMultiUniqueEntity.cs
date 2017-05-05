using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renshaw.Commom.Model
{
    public interface IMultiUniqueEntity
    {
        int UserId { get; set; }
        int SourceId { get; set; }
        long UniqueId { get; set; }
    }
}
