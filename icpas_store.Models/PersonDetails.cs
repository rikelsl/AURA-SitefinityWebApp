namespace icpas_store.Models
{
    public class PersonDetails : BaseModel
    {
        public virtual int PersonID { get; set; }
        public virtual string Name { get; set; }
        public virtual int CompanyID { get; set; }
        public virtual string Email { get; set; }
        public virtual string MemberType { get; set; }
        public virtual string PhoneAreaCode { get; set; }
        public virtual string Phone { get; set; }
        public virtual string PhoneCountryCode { get; set; }
        public virtual string AICPAMemberNo { get; set; }
        public virtual string CPADefaultChapter { get; set; }
        public virtual string CPACompanyCategory { get; set; }
        public virtual string FirstLast { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string ZipCode { get; set; }
    }
}