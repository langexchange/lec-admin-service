using LE.AdminService.Infrastructure.Infrastructure.Entities;

namespace LE.AdminService.Services
{
    public interface IJwtUtils
    {
        public string GenerateToken(Admin admin);
        public string ValidateToken(string token);
    }
}
