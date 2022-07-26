using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            ReceivedItems = new HashSet<ReceivedItem>();
        }

        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactNo { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<ReceivedItem> ReceivedItems { get; set; }
    }
}
