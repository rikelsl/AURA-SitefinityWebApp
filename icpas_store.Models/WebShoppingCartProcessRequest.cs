namespace icpas_store.Models
{
    public class WebShoppingCartProcessRequestViewModel
    {
        public int SavedShoppingCartId { get; set; }
        public int PaymentTypeId { get; set; }
        public string PaymentSource { get; set; }
        public string CardNumber { get; set; }
        public int CardExpirationMonth { get; set; }
        public int CardExpirationYear { get; set; }
        public string CardSvn { get; set; }

        public Order Order { get; set; }
        public string PaymentIssue { get; set; }
        public int MarketingSourceId { get; set; }
    }
}