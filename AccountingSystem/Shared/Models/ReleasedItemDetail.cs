using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class ReleasedItemDetail
    {
        public int ReleasedItemDetailId { get; set; }
        public int? ReleasedItemId { get; set; }
        public int? LocationId { get; set; }
        public int? ItemId { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Cost { get; set; }

        public virtual Item? Item { get; set; }
        public virtual Location? Location { get; set; }
        public virtual ReleasedItem? ReleasedItem { get; set; }
    }
}
