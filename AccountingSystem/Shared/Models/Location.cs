using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Location
    {
        public Location()
        {
            ReceivedItemDetails = new HashSet<ReceivedItemDetail>();
            ReleasedItemDetails = new HashSet<ReleasedItemDetail>();
        }

        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public bool? IsActive { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<ReceivedItemDetail> ReceivedItemDetails { get; set; }
        public virtual ICollection<ReleasedItemDetail> ReleasedItemDetails { get; set; }
    }
}
