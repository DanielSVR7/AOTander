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
    
    public partial class Positions
    {
        public Positions()
        {
            this.Employees = new HashSet<Employees>();
        }
    
        public int Id { get; set; }
        public string Position { get; set; }
    
        public virtual ICollection<Employees> Employees { get; set; }
    }
}
