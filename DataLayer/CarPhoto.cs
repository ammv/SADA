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
    public partial class CarPhoto
    {
        public int ID { get; set; }
        public Nullable<int> CarID { get; set; }
        public Nullable<int> FileID { get; set; }
    
        public virtual Car Car { get; set; }
        public virtual File File { get; set; }
    }
}
