using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class ReleasedItem
    {
        public ReleasedItem()
        {
            ReleasedItemDetails = new HashSet<ReleasedItemDetail>();
        }

        public int ReleasedItemId { get; set; }
        public string? ReleaseTransactionNo { get; set; }
        public string? InvoiceNo { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? DateReleased { get; set; }
        public string? Remarks { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<ReleasedItemDetail> ReleasedItemDetails { get; set; }
    }
}
