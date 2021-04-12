using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace icpas_store.Models
{
    public class FieldOfStudyViewModel
    {
        public virtual int Id { get; set; }
        public virtual string EducationCategory { get; set; }
        public virtual decimal EducationUnits { get; set; }
        public virtual string Status { get; set; }
    }
}