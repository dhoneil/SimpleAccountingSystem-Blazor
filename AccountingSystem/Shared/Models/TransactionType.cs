using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class TransactionType
    {
        public TransactionType()
        {
            ItemTransactions = new HashSet<ItemTransaction>();
        }

        public int TransactionTypeId { get; set; }
        public string? TransactionTypeName { get; set; }

        public virtual ICollection<ItemTransaction> ItemTransactions { get; set; }
    }
}
