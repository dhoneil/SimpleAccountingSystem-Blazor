using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class ReceivedItem
    {
        public ReceivedItem()
        {
            ReceivedItemDetails = new HashSet<ReceivedItemDetail>();
        }

        public int ReceivedItemId { get; set; }
        public string? ReceiveTransactionNo { get; set; }
        public string? InvoiceNo { get; set; }
        public int? SupplierId { get; set; }
        public DateTime? DateReceived { get; set; }
        public string? Remarks { get; set; }

        public virtual Supplier? Supplier { get; set; }
        public virtual ICollection<ReceivedItemDetail> ReceivedItemDetails { get; set; }
    }
}
