
namespace icpas_store.Models
{
    public class FirmAdminRegistrants : BaseModel
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Email { get; set; }
        public virtual string FirstLast { get; set; }
    }
}
