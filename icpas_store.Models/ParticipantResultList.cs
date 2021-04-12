using System;
using System.Runtime.Serialization;

namespace icpas_store.Models
{
	[Serializable]
	[DataContract]
	public class ParticipantResultList : ICloneable
	{
		[DataMember]
		public virtual int Id { get; set; }

		[DataMember]
        public virtual string Name { get; set; }

		[DataMember]
        public virtual string CompanyName { get; set; }

		[DataMember]
        public virtual int StatusID { get; set; }

		[DataMember]
        public virtual bool DirExclude { get; set; }

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}