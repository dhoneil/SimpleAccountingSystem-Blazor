using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Location
    {
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public bool? IsActive { get; set; }
    }
}
