using System;
using System.Collections.Generic;

#nullable disable

namespace LE.AdminService.Infrastructure.Infrastructure.Entities
{
    public partial class Correctmsg
    {
        public Guid Messid { get; set; }
        public string CorrectText { get; set; }

        public virtual Message Mess { get; set; }
    }
}
