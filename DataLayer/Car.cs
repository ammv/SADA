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
    
    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            this.CarExpense = new HashSet<CarExpense>();
            this.CarPaymentToCounteragent = new HashSet<CarPaymentToCounteragent>();
            this.CarPaymentFromCounteragent = new HashSet<CarPaymentFromCounteragent>();
            this.CarPhoto = new HashSet<CarPhoto>();
            this.Salary = new HashSet<Salary>();
        }
    
        public int ID { get; set; }
        public int CarDealershipID { get; set; }
        public Nullable<int> EquipmentID { get; set; }
        public Nullable<int> YearOfRelease { get; set; }
        public Nullable<int> ColorID { get; set; }
        public Nullable<int> Mileage { get; set; }
        public Nullable<int> ManufacturerID { get; set; }
        public string VIN { get; set; }
        public Nullable<decimal> AmountToOwner { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public Nullable<System.DateTime> ArrivalDate { get; set; }
        public Nullable<decimal> PurchasePrice { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<decimal> SalonPrice { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<decimal> PrepaymentAmount { get; set; }
        public Nullable<int> Buyer { get; set; }
        public Nullable<System.DateTime> SaleDate { get; set; }
        public Nullable<int> PaymentTypeID { get; set; }
        public Nullable<int> ManagerBuyout { get; set; }
        public Nullable<int> ManagerSales { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Counteragent Counteragent { get; set; }
        public virtual CarDealership CarDealership { get; set; }
        public virtual CarColor CarColor { get; set; }
        public virtual CarEquipment CarEquipment { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Staff Staff1 { get; set; }
        public virtual Counteragent Counteragent1 { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual Counteragent Counteragent2 { get; set; }
        public virtual CarStatus CarStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarExpense> CarExpense { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarPaymentToCounteragent> CarPaymentToCounteragent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarPaymentFromCounteragent> CarPaymentFromCounteragent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CarPhoto> CarPhoto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Salary> Salary { get; set; }
    }
}
