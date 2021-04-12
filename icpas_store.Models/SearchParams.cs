using System.Collections.Generic;

namespace icpas_store.Models
{
    public class SearchParams
    {
        public virtual IList<Param> TopicCodes { get; set; } 
        public virtual IList<Param> CreditTypes { get; set; } 
        public virtual IList<NameParam> Locations { get; set; } 
        public virtual IList<Param> Speakers { get; set; } 
        public virtual IList<Param> ProgramTypes { get; set; }
        public virtual IList<NameParam> CourseLevels { get; set; }
        public virtual IList<NameParam> DeliveryTypes { get; set; }
    }
}
