namespace icpas_store.Models
{
    public class OrderLine : BaseModel
    {
        public virtual int OrderId { get; set; }
        public virtual decimal Extended { get; set; }
        public virtual Product Product { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal Discount { get; set; }
        //public virtual Campaign Campaign { get; set; }
        public virtual int RequestedLineId { get; set; }
        public virtual int RequestedRegistrantId { get; set; }
        public virtual string Description { get; set; }
    }
}