using System;

namespace icpas_store.Models
{
    public class MeetingSpeaker : BaseModel
    {
        public virtual int MeetingId { get; set; }
        public virtual int Sequence { get; set; }

        public virtual Person Speaker { get; set; }

        public virtual string Status { get; set; }
        public virtual string Type { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual string Evaluation { get; set; }
        public virtual string ContentQuality { get; set; }
        public virtual string SpeakerQuality { get; set; }
    }
}
