using System;
using System.Runtime.Serialization;

namespace icpas_store.Models
{
	[Serializable]
	[DataContract]
	public class LocatorCompanyDetail : ICloneable
	{
		[DataMember]
		public virtual int Id { get; set; }

		[DataMember]
		public virtual int CompanyID { get; set; }

		[DataMember]
		public virtual bool icpas_IncludeInCPASearch { get; set; }

		[DataMember]
		public virtual string icpas_ReferralContactName { get; set; }

		[DataMember]
		public virtual string icpas_ReferralContactEmail { get; set; }

		[DataMember]
		public virtual string CompanyName { get; set; }

		[DataMember]
		public virtual string Line1 { get; set; }

		[DataMember]
		public virtual string Line2 { get; set; }

		[DataMember]
		public virtual string City { get; set; }

		[DataMember]
		public virtual string County { get; set; }

		[DataMember]
		public virtual string StateProvince { get; set; }

		[DataMember]
		public virtual string PostalCode { get; set; }

		[DataMember]
		public virtual string Phone { get; set; }

		[DataMember]
		public virtual string Email { get; set; }

		[DataMember]
		public virtual string CompanyType { get; set; }

		[DataMember]
		public virtual string Chapter { get; set; }

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}