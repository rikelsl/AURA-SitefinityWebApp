using System;

namespace icpas_store.Models
{
    public class CertificateOfCompletionModel
    {
        public virtual int ID { get; set; }
        public virtual string code { get; set; }
        public virtual string cpapreferredaddressline1 { get; set; }
        public virtual string cpapreferredaddressline2 { get; set; }
        public virtual string cpapreferredaddressline3 { get; set; }
        public virtual string cpapreferredcity { get; set; }
        public virtual string cpapreferredstate { get; set; }
        public virtual string cpapreferredzip { get; set; }
        public virtual string cpe { get; set; }
        public virtual DateTime dateearned { get; set; }
        public virtual DateTime enddate { get; set; }
        public virtual string firstlast { get; set; }
        public virtual int meetingid { get; set; }
        public virtual string meetingtitle { get; set; }
        public virtual string nickname { get; set; }
        public virtual int personid { get; set; }
        public virtual string icpas_facilityaddress1 { get; set; }
        public virtual string icpas_facilityaddress2 { get; set; }
        public virtual string icpas_facilitycity { get; set; }
        public virtual string icpas_facilityname { get; set; }
        public virtual string icpas_facilitystate { get; set; }
        public virtual string icpas_facilityzip { get; set; }
        public virtual int productid { get; set; }
        public virtual string status { get; set; }
    }
}