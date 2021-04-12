using System;

namespace icpas_store.Models
{
    public class ProductRelation : BaseModel
    {
        public virtual Product Product { get; set; }
        public virtual int Sequence { get; set; }
        public virtual Product RelatedProduct { get; set; }
        public virtual ProductRelationshipType ProductRelationshipType { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual bool AutoPrompt { get; set; }
        public virtual string PromptText { get; set; }
        public virtual bool WebPrompt { get; set; }
        public virtual string WebPromptText { get; set; }
        public virtual string Comments { get; set; }
    }
}