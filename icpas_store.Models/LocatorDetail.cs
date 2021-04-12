using System.Collections.Generic;

namespace icpas_store.Models
{
	public class LocatorDetail : BaseModel
	{
		public virtual IList<LocatorCompanyDetail> CoDetail { get; set; }
		public virtual IList<TopicCode> CoTopicCodes { get; set; }
	}
}