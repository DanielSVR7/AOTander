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
    
    public partial class Employees
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Phone { get; set; }
        public Nullable<int> ShopID { get; set; }
        public Nullable<int> PositionID { get; set; }
        public Nullable<int> HoursWorked { get; set; }
        public string PhotoPath { get; set; }
    
        public virtual Shops Shops { get; set; }
        public virtual Positions Positions { get; set; }
    }
}
