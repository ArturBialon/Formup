﻿using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class Cost
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int Tax { get; set; }
        public string Name { get; set; }
        public int CasesId { get; set; }
        public int ServiceProvidersId { get; set; }

        public virtual Case Cases { get; set; }
        public virtual ServiceProvider ServiceProviders { get; set; }
    }
}
