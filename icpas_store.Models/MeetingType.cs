namespace icpas_store.Models
{
    public class MeetingType : BaseModel
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}