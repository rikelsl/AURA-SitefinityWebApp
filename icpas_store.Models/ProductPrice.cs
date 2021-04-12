namespace icpas_store.Models
{
    public class ProductPrice : BaseModel
    {
        public virtual int ProductId { get; set; }
        public virtual string Name { get; set; }
        //public virtual MemberType MemberType { get; set; }
        public virtual decimal Price { get; set; }
        public virtual MemberType MemberType { get; set; }
    }
}