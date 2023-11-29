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
    public partial class SoloTrader
    {
        public int CounteragentID { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public Nullable<int> RegistrationAddres { get; set; }
        public Nullable<int> BusinessAddress { get; set; }
        public string INN { get; set; }
        public string OGRNIP { get; set; }
        public Nullable<System.DateTime> DateOfAssignmentORGRIP { get; set; }
        public string Bank { get; set; }
        public string BIK { get; set; }
        public string CorrespondentAccount { get; set; }
        public string PaymentAccount { get; set; }
        public string OKVED { get; set; }
        public Nullable<int> TaxationSystemTypeID { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual Address Address1 { get; set; }
        public virtual Counteragent Counteragent { get; set; }
        public virtual TaxationSystemType TaxationSystemType { get; set; }
    }
}
