using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Category
    {
        public Category()
        {
            Items = new HashSet<Item>();
            Locations = new HashSet<Location>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}
