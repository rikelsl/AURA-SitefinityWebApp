using System.Collections.Generic;

namespace icpas_store.Models
{
    public class OrderViewModel : Order
    {
        public new IEnumerable<OrderLineViewModel> Lines { get; set; }
    }
}