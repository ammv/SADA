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
    public partial class ProductSpecific
    {
        public int ID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> CarDealershipID { get; set; }
        public Nullable<int> Remains { get; set; }
    
        public virtual CarDealership CarDealership { get; set; }
        public virtual Product Product { get; set; }
    }
}
