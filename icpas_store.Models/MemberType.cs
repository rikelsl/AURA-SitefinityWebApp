namespace icpas_store.Models
{
    public class MemberType : BaseModel
    {
        public virtual string Name { get; set; }
        public virtual string membertypeID { get; set; }
        public virtual bool isMember { get; set; }
    }
}