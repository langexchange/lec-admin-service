using LE.AdminService.Services;
using LE.Library.Kernel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.AdminService.Controllers
{
    [Route("admin/api/settings")]
    [ApiController]
    public class SettingManagementController : ControllerBase
    {
        private readonly IRequestHeader _requestHeader;
        private readonly ISettingService _settingService;

        public SettingManagementController(IRequestHeader requestHeader, ISettingService settingService)
        {
            _requestHeader = requestHeader;
            _settingService = settingService;
        }

        [HttpPost("seed-data")]
        public async Task<IActionResult> SeedDataAsync(CancellationToken cancellationToken)
        {
            await _settingService.SeedDataAsync(cancellationToken);
            return Ok();
        }

        [HttpPost("support-locale")]
        public async Task<IActionResult> AddSettingSupportLocaleAsync(List<string> locale, CancellationToken cancellationToken)
        {
            await _settingService.AddSupportLocaleAsync(locale, cancellationToken);
            return Ok();
        }
    }
}
