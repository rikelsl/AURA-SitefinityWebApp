namespace icpas_store.Models
{
    public class CompanyDetails : BaseModel
    {
        public virtual int CompanyID { get; set; }
        public virtual string Name { get; set; }
        public virtual string City { get; set; }
        public virtual string CPACompanyCategory { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string AddressLine3 { get; set; }
        public virtual string State { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string Country { get; set; }
        public virtual string MainCountryCode { get; set; }
        public virtual string MainAreaCode { get; set; }
        public virtual string MainPhone { get; set; }
        public virtual string MainFaxCountryCode { get; set; }
        public virtual string MainFaxAreaCode { get; set; }
        public virtual string MainFaxNumber { get; set; }
        public virtual string WebSite { get; set; }
    }
}