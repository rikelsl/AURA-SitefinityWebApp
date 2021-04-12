using System.Collections.Generic;

namespace icpas_store.Models
{
    public class PeerDirectory : BaseModel
    {
        public virtual int ID { get; set; }
        public virtual int CompanyID { get; set; }
        public virtual string FirmName { get; set; }
        public virtual int ContactID { get; set; }
        public virtual string ContactName { get; set; }
        public virtual string ContactEmail { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Zipcode { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Fax { get; set; }
        public virtual string AdditionalInfo { get; set; }
        public virtual string PrenrolledIn { get; set; }
        public virtual bool icpasPerformsAA { get; set; }
        public virtual bool IsEngagement { get; set; }
        public virtual bool IsSystem { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual IList<PeerTopics> AdditionalPeerTopics { get; set; }

    }
}