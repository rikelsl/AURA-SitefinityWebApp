using System;

namespace icpas_store.Models
{
	public class CalendarItem : BaseModel
	{
		public override int Id { get; set; }
		public DateTime CourseStart { get; set; }
		public DateTime CourseEnd { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int CourseTypeId { get; set; }
	}
}