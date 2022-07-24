using System;
using System.Collections.Generic;

namespace AccountingSystem.Shared.Models
{
    public partial class Brand
    {
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
        public bool? IsActive { get; set; }
    }
}
