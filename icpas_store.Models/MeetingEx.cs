using System;
using System.Collections.Generic;

namespace icpas_store.Models
{
    public class MeetingEx : Meeting
    {
        //public virtual ProductCategoryGroupItem CategoryGroupItem { get; set; }
        public new virtual ProductEx Product { get; set; }
        public new virtual IList<MeetingSpeakerEx> Speakers { get; set; }
        public virtual int SessionNumber { get; set; }
        public virtual string Level { get; set; }
        public virtual int cleProductId { get; set; }
        public virtual int SessionCount { get; set; }
        public virtual string MatrixWorkshop { get; set; }
        public virtual int MatrixTimeslot { get; set; }
        public virtual int MatrixRequiredNumSessions { get; set; }
        public virtual string Summary { get; set; }
        public virtual string AdditionalInformation { get; set; }
        public virtual string Objectives { get; set; }
        public virtual string Prerequisites { get; set; }
        public virtual string OnsiteDescription { get; set; }
        public virtual int AvailSpace { get; set; }
        public virtual string FacilityName { get; set; }
        public virtual string RelatedWebcastInformation { get; set; }
        public virtual string CpeMarketingMeetingDescription { get; set; }
        public virtual string CpeMarketingMeetingSideNavDescription { get; set; }
        public virtual int GroupId { get; set; }
        public virtual decimal TotalCredits { get; set; }
        public virtual string FieldOfStudy { get; set; }
        public virtual string SpecialtyCredits { get; set; }
        public virtual DateTime OpenTime { get; set; }
        public virtual bool NoRegistration { get; set; }
        public virtual int EMaterials { get; set; }
        public virtual int TotalRegistrants { get; set; }
        public virtual int TotalWaitList { get; set; }
        public virtual string DeliveryType { get; set; }
    }
}
