namespace LE.AdminService.Services
{
    public interface ISettingService
    {
        Task<bool> SeedDataAsync(CancellationToken cancellationToken = default);
        Task AddSupportLocaleAsync(List<string> supportLocale, CancellationToken cancellationToken = default);
    }
}
