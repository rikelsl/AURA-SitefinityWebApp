using System;
using System.Collections.Generic;

namespace icpas_store.Models
{
    public class WebUser : BaseModel
    {
        public virtual string UserName { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual int PersonId { get; set; }
        public virtual string LinkType { get; set; }
        public virtual int LinkId { get; set; }
        public virtual string UniqueId { get; set; }
        public virtual string EncryptedPassword { get; set; }
        public virtual string Password { get; set; }
        public virtual string PasswordHint { get; set; }
        public virtual string PasswordHintAnswer { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime DateUpdated { get; set; }
        public virtual bool Disabled { get; set; }
        public virtual IList<WebShoppingCart> ShoppingCarts { get; set; }
    }
}
