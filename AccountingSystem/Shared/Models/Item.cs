using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Item
    {
        public Item()
        {
            ItemTransactions = new HashSet<ItemTransaction>();
            ReceivedItemDetails = new HashSet<ReceivedItemDetail>();
            ReleasedItemDetails = new HashSet<ReleasedItemDetail>();
        }

        public int ItemId { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public decimal? Price { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? ReorderPoint { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? PartNumberId { get; set; }
        public int? UomId { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual Category? Category { get; set; }
        public virtual PartNumber? PartNumber { get; set; }
        public virtual Uom? Uom { get; set; }
        public virtual ICollection<ItemTransaction> ItemTransactions { get; set; }
        public virtual ICollection<ReceivedItemDetail> ReceivedItemDetails { get; set; }
        public virtual ICollection<ReleasedItemDetail> ReleasedItemDetails { get; set; }
    }
}
