namespace icpas_store.Models
{
    public class Survey : BaseModel
    {
        public virtual string Name { get; set; }
        public virtual string CPAAfterCompleteRedirectURL { get; set; }
        public virtual int RedirectSurveyID { get; set; }
    }
}