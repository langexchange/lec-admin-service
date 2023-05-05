using System;

namespace LE.AdminService.Models.Responses
{
    public class AuthResponse
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
        public string RemainName { get; set; }
        public string Token { get; set; }
    }
}
