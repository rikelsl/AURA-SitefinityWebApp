namespace icpas_store.Models
{
    public class PeerTopics : BaseModel
    {
        public virtual int RecordID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Parent { get; set; }
        public virtual string Description { get; set; }
        public virtual int TopicCodeID { get; set; }
    }
}