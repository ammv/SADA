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
    
    public partial class ProductArrivalPosition
    {
        public int ID { get; set; }
        public Nullable<int> ArrivalID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<decimal> Allowance { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual ProductArrival ProductArrival { get; set; }
    }
}