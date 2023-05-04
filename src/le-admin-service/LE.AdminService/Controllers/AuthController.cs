﻿using LE.AdminService.Models.Requests;
using LE.AdminService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        [Authorize(Policy = "SupperAdminPolicy")]
        [HttpPost("create-account")]
        public IActionResult Register(RegisterRequest model)
        {
            _authService.Register(model);
            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public IActionResult Login(AuthRequest model)
        {
            var response = _authService.AuthenticateAsync(model);
            return Ok(response);
        }
    }
}