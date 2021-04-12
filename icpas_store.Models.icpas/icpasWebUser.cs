namespace icpas_store.Models.icpas
{
    public class icpasWebUser : BaseModel 
    {
        public virtual string UserName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string PersonId { get; set; }
        public virtual string Password { get; set; }
        public virtual bool Disabled { get; set; }
    }
}