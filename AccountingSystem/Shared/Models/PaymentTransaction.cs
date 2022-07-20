using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class PaymentTransaction
    {
        public int Id { get; set; }
        public string? TransactionNo { get; set; }
        public int? ContractId { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? CreatedBy { get; set; }

        public virtual Contract? Contract { get; set; }
    }
}
