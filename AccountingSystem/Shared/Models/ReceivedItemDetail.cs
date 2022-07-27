using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class ReceivedItemDetail
    {
        public int ReceivedItemDetailId { get; set; }
        public int? ReceivedItemId { get; set; }
        public int? LocationId { get; set; }
        public int? ItemId { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Cost { get; set; }

        public virtual Item? Item { get; set; }
        public virtual Location? Location { get; set; }
        public virtual ReceivedItem? ReceivedItem { get; set; }
    }
}
