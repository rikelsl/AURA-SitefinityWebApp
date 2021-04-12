using System;
using System.Runtime.Serialization;

namespace icpas_store.Models
{
	[Serializable]
	[DataContract]
	public class PeerToPeerList : ICloneable
	{
		[DataMember]
		public virtual int Id { get; set; }

		[DataMember]
		public virtual int PeerID { get; set; }

		[DataMember]
		public virtual int PersonID { get; set; }

		[DataMember]
		public virtual string FullName { get; set; }

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
		public virtual bool IsActive { get; set; }

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}