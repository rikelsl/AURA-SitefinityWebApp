namespace icpas_store.Models
{
    public class Company : BaseModel
    {
        public virtual string Name { get; set; }
        public virtual Address Address { get; set; }
        //public virtual CompanyType Type { get; set; }
        //public virtual PhoneNumber Phone { get; set; }
        //public virtual PhoneNumber Fax { get; set; }
        public virtual string Website { get; set; }
    }
}