//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AOTander.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Users
    {
        public Users()
        {
            this.Logins = new HashSet<Logins>();
        }
    
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<Logins> Logins { get; set; }
    }
}
