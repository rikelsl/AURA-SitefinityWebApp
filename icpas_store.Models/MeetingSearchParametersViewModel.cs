using System;
using System.Collections.Generic;
using System.Linq;

namespace icpas_store.Models
{
    public class MeetingSearchParametersViewModel : BaseSearchViewModel
    {
        //public IList<ProductCategoryGroupItem> ProductCategoryGroupItems { get; set; }
        public IList<int> Levels { get; set; }
        public IList<MeetingEducationUnits> CreditTypes { get; set; }
        public int MilesDistance { get; set; }
        public string Zip { get; set; }
        public string UserId { get; set; }
        public virtual string MeetingTitle { get; set; }
        //public virtual Product Product { get; set; }
        public virtual int StatusId { get; set; }
        //public virtual MeetingTypeItem TypeItem { get; set; }
        public virtual TimeSpan OpenTime { get; set; }
        public virtual Address Location { get; set; }
        public virtual IList<MeetingEducationUnits> Credits { get; set; }
        public virtual int MaxRegistrants { get; set; }
        //public virtual Venue Venue { get; set; }
        public virtual int? ParentId { get; set; }
        public virtual int Relevance { get; set; }
        public virtual int ClassLevelId { get; set; }
        public virtual string DeliveryType { get; set; }

        public bool HasCreditTypes
        {
            get { return CreditTypes != null && CreditTypes.Any(); }
        }

        public bool HasLevels
        {
            get { return Levels != null && Levels.Any(); }
        }
    }
}
