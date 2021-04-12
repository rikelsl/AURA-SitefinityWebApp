using System;
using System.Runtime.Serialization;

namespace icpas_store.Models
{
	[Serializable]
	[DataContract]
	public class PeerReviewSearchCriteria
	{
		[DataMember]
		public bool IsEngagement { get; set; }

		[DataMember]
		public bool IsSystem { get; set; }

		[DataMember]
		public string CompanyName { get; set; }

		[DataMember]
		public string AICPACenter { get; set; }

		[DataMember]
		public string AreasOfPractice { get; set; }

		[DataMember]
		public string Range { get; set; }

		[DataMember]
		public string ZipCode { get; set; }
	}
}