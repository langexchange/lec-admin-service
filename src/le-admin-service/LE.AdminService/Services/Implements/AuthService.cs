using AutoMapper;
using LE.AdminService.Infrastructure.Infrastructure;
using LE.AdminService.Infrastructure.Infrastructure.Entities;
using LE.AdminService.Models.Requests;
using LE.AdminService.Models.Responses;
using LE.AdminService.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LE.AdminService.Services.Implements
{
    public class AuthService : IAuthService
    {
        private LanggeneralDbContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public AuthService(LanggeneralDbContext context, IJwtUtils jwtUtils, IMapper mapper)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthRequest model)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);

            // validate
            if (admin == null)
            {
                throw new Exception("Username or password is incorrect");
            }

            // authentication successful
            var response = _mapper.Map<AuthResponse>(admin);
            response.Token = _jwtUtils.GenerateToken(admin);
            return response;
        }

        public void Delete(Guid id)
        {
            throw new System.NotImplementedException();
        }

        public Admin GetById(Guid id)
        {
            return _context.Admins.FirstOrDefault(x => x.Adminid == id);
        }

        public void Register(RegisterRequest model)
        {
            // validate
            if (_context.Admins.Any(x => x.Email.Equals(model.Email)))
                throw new Exception("Email '" + model.Email + "' is already taken");

            // map model to new user object
            var admin = _mapper.Map<Admin>(model);
            // hash password
            //user.PasswordHash = BCryptNet.HashPassword(model.Password);

            // save user
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }
        public void UpdatePassword(Guid id, string password)
        {
            var admin = _context.Admins.FirstOrDefault(x => x.Adminid == id);
            if (admin == null)
                throw new Exception($"Not exist user has id: {id}");

            admin.Password = password;
            _context.Update(admin);
            _context.SaveChanges();
        }

        public Admin GetByEmail(string email)
        {
            return _context.Admins.FirstOrDefault(x => x.Email == email);
        }
    }
}
