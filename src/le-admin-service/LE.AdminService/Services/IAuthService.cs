using LE.AdminService.Infrastructure.Infrastructure.Entities;
using LE.AdminService.Models.Requests;
using LE.AdminService.Models.Responses;
using System;
using System.Threading.Tasks;

namespace LE.AdminService.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(AuthRequest model);
        //IEnumerable<User> GetAll();
        Admin GetById(Guid id);
        Admin GetByEmail(string email);
        void Register(RegisterRequest model);
        void UpdatePassword(Guid id, string password);
        //void Update(int id, UpdateRequest model);
        void Delete(Guid id);
    }
}
