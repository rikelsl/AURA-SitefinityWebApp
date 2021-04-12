namespace icpas_store.Models
{
    public class WebShoppingCartDetailsCount : BaseModel
    {
        public virtual int WebUserId { get; set; }
        public virtual WebUser WebUser { get; set; }
        public virtual int Items { get; set; }
    }
}
