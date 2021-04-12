using System;

namespace icpas_store.Models
{
    public class WebShoppingCartItemView : BaseModel
    {
        public virtual string MeetingName { get; set; }
        public virtual int ProductId { get; set; }
        public virtual int MeetingId { get; set; }
        public virtual string Description { get; set; }
        public virtual int PersonId { get; set; }
        public virtual DateTime MeetingStart { get; set; }
        public virtual DateTime MeetingEnd { get; set; }
        public virtual decimal Price { get; set; }
        public virtual bool ClassPass { get; set; }
        public virtual bool AllowGuests { get; set; }
        public virtual int CLEId { get; set; }
        public virtual int SessionCount { get; set; }
        public virtual string PromoCode { get; set; }
        public virtual bool IsMeeting { get; set; }
        public virtual bool IsSession { get; set; }
        public virtual string Location { get; set; }
        public virtual string WebDescription { get; set; }
    }
}