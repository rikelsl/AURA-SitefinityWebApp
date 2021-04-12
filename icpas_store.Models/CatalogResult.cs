using System;

namespace icpas_store.Models
{
	public class CatalogResult : BaseModel
	{
		public override int Id { get; set; }
		public DateTime CalEndDate { get; set; }
		public DateTime CalStartDate { get; set; }
		public string CourseLevel { get; set; }
		public string Location { get; set; }
		public string MeetDate { get; set; }
		public string Meeting { get; set; }
		public string MeetingCategory { get; set; }
		public decimal? MemberPrice { get; set; }
		public decimal? NonMemberPrice { get; set; }
		public string TotalCredits { get; set; }
		public string WebDescription { get; set; }
	}
}