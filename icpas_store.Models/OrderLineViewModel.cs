namespace icpas_store.Models
{
    public class OrderLineViewModel : OrderLine
    {
        public MeetingEx Meeting { get; set; }
        public int WebShoppingCartId { get; set; }
    }
}
