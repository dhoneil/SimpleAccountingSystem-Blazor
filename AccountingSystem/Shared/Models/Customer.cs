using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactNo { get; set; }
    }
}
