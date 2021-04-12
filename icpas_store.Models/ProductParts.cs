namespace icpas_store.Models
{
    public class ProductParts : BaseModel
    {
        public virtual int ProductId { get; set; }
        public virtual int Sequence { get; set; }
        public virtual int SubProductId { get; set; }
        public virtual string SubProductId_Name { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual decimal PctOfRev { get; set; }
        public virtual string Comments { get; set; }
    }
}