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
    
    public partial class CarGroupAttribute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarGroupAttribute()
        {
            this.CarEquipmentGroupAttribute = new HashSet<CarEquipmentGroupAttribute>();
        }
    
        public int ID { get; set; }
        public Nullable<int> EquipmentID { get; set; }
        public string Name { get; set; }
    
        public virtual CarEquipment CarEquipment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarEquipmentGroupAttribute> CarEquipmentGroupAttribute { get; set; }
    }
}