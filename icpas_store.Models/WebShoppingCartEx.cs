using System;
using System.Collections.Generic;

namespace icpas_store.Models
{
    public class WebShoppingCartEx : WebShoppingCart
    {
        public virtual WebShoppingCartType Type { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime DateUpdated { get; set; }
        public virtual IList<WebShoppingCartDetails> Lines { get; set; }
        public virtual int? OrderId { get; set; }
    }
}
