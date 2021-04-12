namespace icpas_store.Models
{
    public class PaymentType : BaseModel
    {
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }
        public virtual string Description { get; set; }
        public virtual bool Active { get; set; }
        public virtual bool Inflow { get; set; }
    }
}