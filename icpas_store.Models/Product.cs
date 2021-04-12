using System;
using System.Collections.Generic;

namespace icpas_store.Models
{
    public class Product : BaseModel
    {
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual IList<ProductPrice> Prices { get; set; }
        public virtual IList<ProductParts> Parts { get; set; } 
        //public virtual ProductType Type { get; set; }
        public virtual bool IsSold { get; set; }
        public virtual bool IsSubscription { get; set; }
        public virtual bool WebEnabled { get; set; }
        public virtual string WebDescription { get; set; }
        //public virtual string WebLongDescription { get; set; }
        public virtual string WebName { get; set; }
        public virtual string WebImage { get; set; }
        public virtual ProductCategory Category { get; set; }
        public virtual int ParentId { get; set; }
        public virtual string WebPageType { get; set; }
        public virtual string WebProductPage { get; set; }
        public virtual string Vendor { get; set; }
        public virtual DateTime AvailableUntil { get; set; }
        public virtual string WebDescriptionWithHTML { get; set; }
        public virtual DateTime DateCreated { get; set; }
    }
}