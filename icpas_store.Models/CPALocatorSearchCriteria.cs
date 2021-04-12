using System;
using System.Runtime.Serialization;

namespace icpas_store.Models
{
	[Serializable]
	[DataContract]
	public class CPALocatorSearchCriteria
	{

		[DataMember]
		public string CompanyName { get; set; }

		[DataMember]
		public string SpecificBusiness { get; set; }

		[DataMember]
		public string IndustriesServed { get; set; }

		[DataMember]
		public string ServicesOffered { get; set; }

		[DataMember]
		public string InternationalExperience { get; set; }

		[DataMember]
		public string Range { get; set; }

		[DataMember]
		public string ZipCode { get; set; }
	}
}