using System;
using System.Runtime.Serialization;

namespace icpas_store.Models
{
	[Serializable]
	[DataContract]
	public class MemberResultList : ICloneable
	{
		[DataMember]
		public virtual int Id { get; set; }

		[DataMember]
		public virtual int PersonID { get; set; }

		[DataMember]
        public virtual string CPADefaultChapter { get; set; }

		[DataMember]
        public virtual string Name { get; set; }

		[DataMember]
        public virtual int CompanyID { get; set; }

		[DataMember]
        public virtual string CompanyName { get; set; }

		[DataMember]
        public virtual string LastName { get; set; }

		[DataMember]
        public virtual string FirstName { get; set; }

		[DataMember]
        public virtual bool DirExclude { get; set; }

		[DataMember]
        public virtual string ZipCode { get; set; }

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}