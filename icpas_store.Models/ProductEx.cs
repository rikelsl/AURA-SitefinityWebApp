
using System;

namespace icpas_store.Models
{
    public class ProductEx : Product
    {
        public virtual bool IsVariableDonation { get; set; }
        public virtual string PrimaryVendor { get; set; }
        public virtual bool ICPASAccessPassDiscount { get; set; }
        public virtual bool CPASeasonTicketApplies { get; set; }
        public virtual string WebProductContent { get; set; }
        public virtual DateTime JustAddedStart { get; set; }
        public virtual DateTime JustAddedEnd { get; set; }
    }
}
