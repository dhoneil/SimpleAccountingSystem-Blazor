using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class ReceivedItem
    {
        public int ReceivedItemId { get; set; }
        public string? ReceiveTransactionNo { get; set; }
        public decimal? QtyReceived { get; set; }
        public DateTime? DateReceived { get; set; }
        public decimal? InvoiceNo { get; set; }
        public decimal? Amount { get; set; }
        public int? SupplierId { get; set; }
        public int? ItemId { get; set; }
        public int? LocationId { get; set; }

        public virtual Item? Item { get; set; }
        public virtual Location? Location { get; set; }
        public virtual Supplier? Supplier { get; set; }
    }
}
