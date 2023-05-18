using System.ComponentModel.DataAnnotations;

namespace LE.AdminService.Models.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string RemainName { get; set; }

        public bool IsSupperAdmin { get; set; } = false;
    }
}
