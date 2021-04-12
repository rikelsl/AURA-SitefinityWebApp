namespace icpas_store.Models
{
    public class PersonEx : Person
    {
        public virtual string CpaNickname { get; set; }
        public virtual string CpaSpeakerBio { get; set; }
        public virtual PersonFunctions PersonFunction { get; set; }
    }
}
