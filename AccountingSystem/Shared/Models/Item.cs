using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Item
    {
        public Item()
        {
            ReceivedItems = new HashSet<ReceivedItem>();
        }

        public int ItemId { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? Price { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? ReorderPoint { get; set; }
        public int? CategoryId { get; set; }
        public int? PartNumberId { get; set; }
        public int? UomId { get; set; }
        public int? BrandId { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual Category? Category { get; set; }
        public virtual PartNumber? PartNumber { get; set; }
        public virtual Uom? Uom { get; set; }
        public virtual ICollection<ReceivedItem> ReceivedItems { get; set; }
    }
}
