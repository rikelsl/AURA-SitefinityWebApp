﻿namespace icpas_store.Models
{
    public class Address : BaseModel
    {
        public virtual string Line1 { get; set; }
        public virtual string Line2 { get; set; }
        public virtual string Line3 { get; set; }
        public virtual string Line4 { get; set; }
        public virtual string City { get; set; }
        public virtual string StateProvince { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Country { get; set; }
    }
}