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
    public partial class ProductArrival
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductArrival()
        {
            this.ProductArrivalPosition = new HashSet<ProductArrivalPosition>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CounteragentID { get; set; }
        public Nullable<int> CarDealershipID { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual CarDealership CarDealership { get; set; }
        public virtual Counteragent Counteragent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductArrivalPosition> ProductArrivalPosition { get; set; }
    }
}
