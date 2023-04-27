using System;
using System.Collections.Generic;

#nullable disable

namespace LE.AdminService.Infrastructure.Infrastructure.Entities
{
    public partial class Vocabgoal
    {
        public Guid Goalid { get; set; }
        public int? Amount { get; set; }
        public string Type { get; set; }
    }
}
