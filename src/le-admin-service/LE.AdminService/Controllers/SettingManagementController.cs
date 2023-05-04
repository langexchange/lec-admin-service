using AutoMapper;
using LE.AdminService.Constants;
using LE.AdminService.Dtos;
using LE.AdminService.Models.Requests;
using LE.AdminService.Services;
using LE.Library.Kernel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.AdminService.Controllers
{
    [Route("admin/api/settings")]
    [Authorize(Policy = "SuperAdminOrAdminPolicy")]
    [ApiController]
    public class SettingManagementController : ControllerBase
    {
        private readonly IRequestHeader _requestHeader;
        private readonly ISettingService _settingService;
        private readonly IMapper _mapper;

        public SettingManagementController(IRequestHeader requestHeader, ISettingService settingService, IMapper mapper)
        {
            _requestHeader = requestHeader;
            _settingService = settingService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSettingsAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(_requestHeader));
            var settings = await _settingService.GetSettingsAsync(cancellationToken);
            return Ok(settings);
        }

        [HttpPost("seed-data")]
        public async Task<IActionResult> SeedDataAsync(CancellationToken cancellationToken)
        {
            await _settingService.SeedDataAsync(cancellationToken);
            return Ok();
        }

        [HttpGet("support-locale")]
        public async Task<IActionResult> GetSettingSupportLocaleAsync(CancellationToken cancellationToken)
        {
            var supportedLocale = await _settingService.GetSupportLocaleAsync(cancellationToken);
            return Ok(supportedLocale);
        }

        [HttpPost("support-locale")]
        public async Task<IActionResult> AddSettingSupportLocaleAsync(List<string> locale, CancellationToken cancellationToken)
        {
            await _settingService.AddSupportLocaleAsync(locale, cancellationToken);
            return Ok();
        }

        [HttpGet("keys")]
        public async Task<IActionResult> GetSettingKeysAsync(CancellationToken cancellationToken)
        {
            var keys = await _settingService.GetSettingKeysAsync(cancellationToken);
            return Ok(keys);
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddSettingsAsync(List<SettingKeyPairRequest> settings, CancellationToken cancellationToken)
        {
            var settingDtos = _mapper.Map<List<SettingDto>>(settings);
            await _settingService.CreateSettingsAsync(settingDtos, cancellationToken);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSettingsAsync(List<SettingKeyPairUpdateRequest> request, CancellationToken cancellationToken)
        {
            await _settingService.UpdateSettingsAsync(request, cancellationToken);
            return Ok();
        }
    }
}
