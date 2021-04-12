using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace icpas_store.Models
{
    public class ProductDiscountViewModel
    {
        public virtual string Name { get; set; }
        public virtual decimal DiscountValue { get; set; }
    }
}