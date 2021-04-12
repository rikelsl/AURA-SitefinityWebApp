using System;
using System.Collections.Generic;

namespace icpas_store.Models
{
    public class WebShoppingCartDetails : BaseModel
    {
        public virtual int WebShoppingCartId { get; set; }
        public virtual int RegistrantId { get; set; }
        public virtual int ProductId { get; set; }
        public virtual bool UserPricingOverride { get; set; }
        public virtual bool ClassPassCardApplied { get; set; }
        public virtual string CouponPromotionalCode { get; set; }
        public virtual string PriceName { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual string BadgeName { get; set; }
        public virtual int AssociatedLineId { get; set; }
        public virtual IList<WebShoppingCartDetailsSessionRank> SessionRanks { get; set; }
        public virtual string JobTitle { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual Int64 ICPASAccPassSubsID { get; set; }
    }
}
