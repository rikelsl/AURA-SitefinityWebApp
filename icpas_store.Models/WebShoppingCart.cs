namespace icpas_store.Models
{
    public class WebShoppingCart : BaseModel
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual int WebUserId { get; set; }
        public virtual string XmlData { get; set; }
    }
}