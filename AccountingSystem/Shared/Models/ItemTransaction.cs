using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class ItemTransaction
    {
        public int ItemTransactionId { get; set; }
        public int? ItemId { get; set; }
        public int? TransactionTypeId { get; set; }
        public decimal? Qty { get; set; }

        public virtual Item? Item { get; set; }
        public virtual TransactionType? TransactionType { get; set; }
    }
}
