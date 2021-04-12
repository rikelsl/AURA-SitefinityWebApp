using System;
using System.Runtime.Serialization;

namespace icpas_store.Models
{
	[Serializable]
	[DataContract]
	public class PeerToPeerSearchCriteria
	{
		[DataMember]
		public string MemberName { get; set; }

		[DataMember]
		public string AreasOfExpertise { get; set; }

		[DataMember]
		public string Range { get; set; }

		[DataMember]
		public string ZipCode { get; set; }
	}
}