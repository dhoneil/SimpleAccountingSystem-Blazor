using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Item
    {
        public int ItemId { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Price { get; set; }
        public decimal? UnitCost { get; set; }
        public int? PartNumberId { get; set; }
        public int? UomId { get; set; }

        public virtual PartNumber? PartNumber { get; set; }
        public virtual Uom? Uom { get; set; }
    }
}
