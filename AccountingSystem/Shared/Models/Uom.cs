using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Uom
    {
        public Uom()
        {
            Items = new HashSet<Item>();
        }

        public int UomId { get; set; }
        public string? UomName { get; set; }
        public string? UomDescription { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
