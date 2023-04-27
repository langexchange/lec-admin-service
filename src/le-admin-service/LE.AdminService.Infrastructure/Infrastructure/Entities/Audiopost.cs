using System;
using System.Collections.Generic;

#nullable disable

namespace LE.AdminService.Infrastructure.Infrastructure.Entities
{
    public partial class Audiopost
    {
        public Guid Audiopostid { get; set; }
        public Guid Postid { get; set; }
        public string Url { get; set; }

        public virtual Post Post { get; set; }
    }
}
