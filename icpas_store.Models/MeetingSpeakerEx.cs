namespace icpas_store.Models
{
    public class MeetingSpeakerEx : MeetingSpeaker
    {
        public virtual new PersonEx Speaker { get; set; }
        public virtual string ThirdPartyName { get; set; }
        public virtual string SpeakerCatalogName { get; set; }
    }
}