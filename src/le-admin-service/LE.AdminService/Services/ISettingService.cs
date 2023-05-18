using LE.AdminService.Dtos;
using LE.AdminService.Models.Requests;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.AdminService.Services
{
    public interface ISettingService
    {
        Task<bool> SeedDataAsync(CancellationToken cancellationToken = default);
        Task AddSupportLocaleAsync(List<string> supportLocale, CancellationToken cancellationToken = default);
        Task<List<string>> GetSupportLocaleAsync(CancellationToken cancellationToken = default);
        Task<List<string>> GetSettingKeysAsync(CancellationToken cancellationToken = default);
        Task<List<LocaleSettingDto>> GetSettingsAsync(CancellationToken cancellationToken = default);
        Task CreateSettingsAsync(List<SettingDto> dtos, CancellationToken cancellationToken);
        Task UpdateSettingsAsync(List<SettingKeyPairUpdateRequest> request, CancellationToken cancellationToken);
        Task UpdateSettingAsync(string key, string value, CancellationToken cancellationToken);
    }
}
