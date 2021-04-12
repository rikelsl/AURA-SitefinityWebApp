using System;

namespace icpas_store.Models
{
    public class Person : BaseModel
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Title { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Email { get; set; }
        public virtual string PreferredAddress { get; set; }
        public virtual string PreferredBillingAddress { get; set; }
        public virtual string PreferredShippingAddress { get; set; }
        public virtual DateTime? JoinDate { get; set; }
        public virtual Address HomeAddress { get; set; }
        public virtual Address BusinessAddress { get; set; }
        public virtual Company Company { get; set; }
        public virtual DateTime? DateCreated { get; set; }
        public virtual DateTime? DateUpdated { get; set; }
        public virtual string Phone { get; set; }
        public virtual string HomeAreaCode { get; set; }
        public virtual string HomePhone { get; set; }
        public virtual string PhoneAreaCode { get; set; }
        public virtual string PhoneExtension { get; set; }
        public virtual int Status { get; set; }
        public virtual DateTime DuesPaidThru { get; set; }
        public virtual int functionId { get; set; }
        public virtual string functionName { get; set; }

        
    }
}