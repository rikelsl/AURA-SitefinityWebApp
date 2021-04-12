using System;
using System.Collections.Generic;

namespace icpas_store.Models
{
    public class CPETranscript : BaseModel
    {
        public virtual List<Section> Section { get; set; }
        public virtual List<Sectiontotal> SectionTotal { get; set; }
        public virtual List<GrandTotal> GrandTotal { get; set; }
    }

    public class Section : BaseModel
    {
        public virtual string City { get; set; }
        public virtual int MeetingID { get; set; }
        public virtual string CPAMeetingTitle { get; set; }
        public virtual string AttendeeStatus_Name { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual DateTime EventTypeDate { get; set; }
        public virtual string Credits { get; set; }
        public virtual double Unit { get; set; }
        public virtual int SurveyID { get; set; }
        public virtual bool Taken { get; set; }
        public virtual bool issurveyactive { get; set; }
        public virtual int ID { get; set; }
    }

    public class Sectiontotal : BaseModel
    {
        public virtual string Totals { get; set; }
        public virtual double Units { get; set; }
    }

    public class GrandTotal : BaseModel
    {
        public virtual string EducationCategory { get; set; }
        public virtual double Units { get; set; }
    }
}