//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    using PropertyChanged;
    [AddINotifyPropertyChangedInterface]
    public partial class Salary
    {
        public int ID { get; set; }
        public Nullable<int> StaffID { get; set; }
        public decimal Sum { get; set; }
        public Nullable<int> TypeID { get; set; }
        public string Note { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<int> CarID { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Car Car { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual SalaryType SalaryType { get; set; }
    }
}
