namespace icpas_store.Models
{
    public class WebShoppingCartDetailsSessionRank : BaseModel
    {
        public int AuraWebShoppingCartDetailsId { get; set; }

        public int Sequence { get; set; }

        public int SessionProductID { get; set; }
    }
}
