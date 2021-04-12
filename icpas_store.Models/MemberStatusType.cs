using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace icpas_store.Models
{
    public class MemberStatusType : BaseModel
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}