using LE.AdminService.Models.Requests;
using LE.AdminService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LE.AdminService.Controllers
{
    [ApiController]
    [Route("admin/api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [Authorize(Policy = "SuperAdminPolicy")]
        [HttpPost("create-account")]
        public IActionResult Register(RegisterRequest model)
        {
            _authService.Register(model);
            return Ok();
        }

        [Authorize(Policy = "SuperAdminPolicy")]
        [HttpDelete("admins/{id}")]
        public async Task<IActionResult> DeleteAdmin(Guid id)
        {
            await _authService.DeleteAsync(id);
            return Ok();
        }

        [Authorize(Policy = "SuperAdminPolicy")]
        [HttpGet("admins")]
        public async Task<IActionResult> GetAdminsAsync()
        {
            var dtos = await _authService.GetAdminsAsync();
            return Ok(dtos);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest model)
        {
            var response =  await _authService.AuthenticateAsync(model);
            return Ok(response);
        }
    }
}
