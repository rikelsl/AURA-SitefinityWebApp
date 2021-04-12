using System;
using System.Runtime.Serialization;

namespace icpas_store.Models
{
	[Serializable]
	[DataContract]
	public class CompanyResultList : ICloneable
	{
		[DataMember]
		public virtual int Id { get; set; }

		[DataMember]
        public virtual string CPACompanyCategory { get; set; }

		[DataMember]
        public virtual string Name { get; set; }

		[DataMember]
        public virtual int CompanyID { get; set; }

		[DataMember]
        public virtual string City { get; set; }
        
		[DataMember]
        public virtual string ZipCode { get; set; }

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}