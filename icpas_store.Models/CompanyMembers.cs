namespace icpas_store.Models
{
    public class CompanyMembers : BaseModel
    {
        public virtual string CompanyID { get; set; }
        public virtual string MemberType { get; set; }
        public virtual string Name { get; set; }
        public virtual string PrimaryFunction { get; set; }
    }
}