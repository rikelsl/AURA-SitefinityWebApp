using System;
using System.Collections.Generic;

namespace icpas_store.Models
{
    public class Order : BaseModel
    {
        public virtual Person ShipToPerson { get; set; }
        public virtual Person BillToPerson { get; set; }

        public virtual Company BillToCompany { get; set; }
        public virtual Company ShipToCompany { get; set; }

        public virtual Address ShippingAddress { get; set; }
        public virtual Address BillingAddress { get; set; }
        public virtual IEnumerable<OrderLine> Lines { get; set; }

        public virtual DateTime OrderDate { get; set; }
        public virtual decimal SubTotal { get; set; }
        public virtual decimal ShippingTotal { get; set; }
        public virtual decimal Tax { get; set; }
        public virtual decimal GrandTotal { get; set; }
        public virtual decimal PaymentTotal { get; set; }
        public virtual decimal Balance { get; set; }
        public virtual int PaymentTypeId { get; set; }
        public virtual string CardNumber { get; set; }
    }
}