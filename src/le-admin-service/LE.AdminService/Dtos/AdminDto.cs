using System;

namespace LE.AdminService.Dtos
{
    public class AdminDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string RemainName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsSupperAdmin { get; set; }
    }
}
