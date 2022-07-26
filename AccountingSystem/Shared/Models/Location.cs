using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Location
    {
        public Location()
        {
            ReceivedItems = new HashSet<ReceivedItem>();
        }

        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public bool? IsActive { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<ReceivedItem> ReceivedItems { get; set; }
    }
}
