using LE.AdminService.Dtos;
using LE.AdminService.Infrastructure.Infrastructure.Entities;
using LE.AdminService.Models.Requests;
using LE.AdminService.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LE.AdminService.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(AuthRequest model);
        //IEnumerable<User> GetAll();
        Task<Admin> GetById(Guid id);
        Task<Admin> GetByEmail(string email);
        void Register(RegisterRequest model);
        void UpdatePassword(Guid id, string password);
        //void Update(int id, UpdateRequest model);
        Task DeleteAsync(Guid id);
        Task<List<AdminDto>> GetAdminsAsync();
    }
}
