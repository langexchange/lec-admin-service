using LE.AdminService.Dtos;
using LE.AdminService.Models.Responses;
using LE.AdminService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LE.AdminService.Controllers
{
    [Route("admin/api/users")]
    [Authorize(Policy = "SuperAdminOrAdminPolicy")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserManagementController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Invalid request");

            var dtos = await _userService.GetUsersPagingAsync(page, pageSize, cancellationToken);
            return Ok(new PageListItemResponse<UserDto>(dtos.Items, dtos.Total, pageSize));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(id, cancellationToken);
            return Ok();
        }
    }
}
