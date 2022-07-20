using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Contract
    {
        public Contract()
        {
            Expenses = new HashSet<Expense>();
            PaymentTransactions = new HashSet<PaymentTransaction>();
        }

        public int Id { get; set; }
        public string? ContractNo { get; set; }
        public string? BusinessName { get; set; }
        public string? Address { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactNo { get; set; }
        public string? ItemDescription { get; set; }
        public int? TotalItem { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnt { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
