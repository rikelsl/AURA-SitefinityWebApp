using System;
using System.Runtime.Serialization;

namespace icpas_store.Models
{
	[Serializable]
	[DataContract]
	public class PeerReviewList : ICloneable
	{
		[DataMember]
		public virtual int Id { get; set; }

		[DataMember]
		public virtual int CompanyID { get; set; }

		[DataMember]
		public virtual string Name { get; set; }

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
		public virtual int PeerID { get; set; }

		[DataMember]
		public virtual bool IsActive { get; set; }

		[DataMember]
		public virtual bool IsSystem { get; set; }

		[DataMember]
		public virtual bool IsEngagement { get; set; }

		[DataMember]
		public virtual string FirstLast { get; set; }

		[DataMember]
		public virtual string CPACredential { get; set; }

		[DataMember]
		public virtual string MainPhone { get; set; }

		[DataMember]
		public virtual string FaxPhone { get; set; }

		[DataMember]
		public virtual string ContactEmail { get; set; }

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}