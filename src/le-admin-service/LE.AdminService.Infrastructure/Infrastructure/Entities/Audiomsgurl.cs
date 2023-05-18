using System;
using System.Collections.Generic;

#nullable disable

namespace LE.AdminService.Infrastructure.Infrastructure.Entities
{
    public partial class Audiomsgurl
    {
        public Guid Urlid { get; set; }
        public Guid? Messid { get; set; }
        public string Url { get; set; }

        public virtual Audiomsg Mess { get; set; }
    }
}
