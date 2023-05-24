using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Common
{

    public abstract class BaseEntity<Tkey> 
    {
        public Tkey Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int>
    {

    }
}
