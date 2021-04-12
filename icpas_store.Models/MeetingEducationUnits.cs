namespace icpas_store.Models
{
    public class MeetingEducationUnits : BaseModel
    {
        public virtual int MeetingId { get; set; }
        public virtual EducationCategory Category { get; set; }
        public virtual decimal EducationUnits { get; set; }
    }
}