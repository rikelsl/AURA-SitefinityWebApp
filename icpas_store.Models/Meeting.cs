using System;
using System.Collections.Generic;

namespace icpas_store.Models
{
    public class Meeting : BaseModel
    {
        public virtual string MeetingTitle { get; set; }
        public virtual Product Product { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        //public virtual MeetingStatus Status { get; set; }
        public virtual MeetingType Type { get; set; }
        public virtual Address Location { get; set; }
        public virtual IList<MeetingEducationUnits> Credits { get; set; }
        public virtual int MaxRegistrants { get; set; }
        public virtual Venue Venue { get; set; }
        public virtual int ParentId { get; set; }
        public virtual IList<MeetingSpeaker> Speakers { get; set; }

    }
}