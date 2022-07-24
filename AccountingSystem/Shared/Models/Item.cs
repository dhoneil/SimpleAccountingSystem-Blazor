using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Item
    {
        public int ItemId { get; set; }
        public string? ItemCode { get; set; }
        public string? ItemDescription { get; set; }
        public int? PartNumberId { get; set; }

        public virtual PartNumber? PartNumber { get; set; }
    }
}
