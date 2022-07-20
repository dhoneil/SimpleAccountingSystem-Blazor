using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Expense
    {
        public int Id { get; set; }
        public string? TransactionNo { get; set; }
        public int? ContractId { get; set; }
        public string? Description { get; set; }
        public string? ExpenseCode { get; set; }
        public decimal? Amount { get; set; }
        public string? PayableTo { get; set; }
        public string? Remarks { get; set; }

        public virtual Contract? Contract { get; set; }
    }
}
