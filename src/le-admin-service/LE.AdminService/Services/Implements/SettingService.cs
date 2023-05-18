using AutoMapper;
using LE.AdminService.Constant;
using LE.AdminService.Dtos;
using LE.AdminService.Infrastructure.Infrastructure;
using LE.AdminService.Infrastructure.Infrastructure.Entities;
using LE.AdminService.Models.Requests;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.AdminService.Services.Implements
{
    public class SettingService : ISettingService
    {
        private LanggeneralDbContext _context;
        private IMapper _mapper;

        public SettingService(LanggeneralDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> SeedDataAsync(CancellationToken cancellationToken = default)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();
            if (setting != null)
                return false;
            var filename = "Jsonfiles/settingnotification.json";
            var text = File.ReadAllText(filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, NotifySetting>>(text);

            var localeSettings = dictionary.SelectMany(x =>
            {
                var values = x.Value.NotifyService;
                return values.Select(y =>
                {
                    return new KeyValuePair<string, string>($"{x.Key.ToLower()}.notify-service.{y.Key.ToLower()}", y.Value);
                }).ToDictionary(z => z.Key, z => z.Value);
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            var settings = localeSettings.Select(x => {
                var locale = x.Key.Substring(0, x.Key.IndexOf('.'));
                return new Setting
                {
                    Id = System.Guid.NewGuid(),
                    ServiceName = "notify-service",
                    SettingKey = x.Key,
                    SettingValue = x.Value,
                    Locale = locale.ToUpper(),
                };
            });
            await _context.Settings.AddRangeAsync(settings);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddSupportLocaleAsync(List<string> locale, CancellationToken cancellationToken = default)
        {
            locale = locale.Select(x => x.ToUpper()).ToList();
            var supportedLocaleSetting = await _context.Settings.FirstOrDefaultAsync(x => x.SettingKey == NotifyKey.SUPPORT_LOCALE);
            if (supportedLocaleSetting == null)
            {
                _context.Add(new Setting
                {
                    Id = System.Guid.NewGuid(),
                    ServiceName = "notify-service",
                    SettingKey = NotifyKey.SUPPORT_LOCALE,
                    SettingValue = JsonConvert.SerializeObject(locale),
                    Locale = NotifyKey.DEFAULT_SUPPORT_LOCALE.ToUpper()
                });
            }
            else
            {
                var supportLocale = JsonConvert.DeserializeObject<List<string>>(supportedLocaleSetting.SettingValue);
                supportLocale = supportLocale.Select(x => x.ToUpper()).ToList();
                supportLocale.AddRange(locale);
                supportLocale = supportLocale.Distinct().ToList();

                supportedLocaleSetting.SettingValue = JsonConvert.SerializeObject(supportLocale);
                _context.Update(supportedLocaleSetting);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetSettingKeysAsync(CancellationToken cancellationToken = default)
        {
            var keys = await _context.Settings.Where(x => x.SettingKey != NotifyKey.SUPPORT_LOCALE)
                                              .Select(x => x.SettingKey.Substring(x.SettingKey.IndexOf(".") + 1))
                                              .ToListAsync();

            return keys;
        }

        public async Task<List<string>> GetSupportLocaleAsync(CancellationToken cancellationToken = default)
        {
            var supportedLocale = await _context.Settings.FirstOrDefaultAsync(x => x.SettingKey == NotifyKey.SUPPORT_LOCALE);
            if (supportedLocale == null)
                return new List<string>();
            return JsonConvert.DeserializeObject<List<string>>(supportedLocale.SettingValue);
        }

        public async Task<List<LocaleSettingDto>> GetSettingsAsync(CancellationToken cancellationToken = default)
        {
            var settings = await _context.Settings.ToListAsync();
            var groupSettings = settings.GroupBy(x => x.Locale).Select(x => x.ToList()).ToList();

            var localeSettingDtos = new List<LocaleSettingDto>();
            foreach (var group in groupSettings)
            {
                var localeSettingDto = new LocaleSettingDto();
                localeSettingDto.Settings = _mapper.Map<List<SettingDto>>(group);
                localeSettingDto.Locale = group.FirstOrDefault()?.Locale;

                localeSettingDtos.Add(localeSettingDto);
            }
            return localeSettingDtos;
        }

        public async Task CreateSettingsAsync(List<SettingDto> dtos, CancellationToken cancellationToken)
        {
            var settingKeys = await _context.Settings.Select(x => x.SettingKey).ToListAsync();
            foreach(var dto in dtos)
            {
                if(settingKeys.Contains(dto.Key))
                    continue;
                await CreateSettingAsync(dto, cancellationToken);
            }
        }
        private async Task CreateSettingAsync(SettingDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
                return;
            var setting = _mapper.Map<Setting>(dto);
            if (setting.SettingKey.Contains("."))
            {
                setting.Locale = setting.SettingKey.Substring(0, dto.Key.IndexOf(".")).ToUpper();
                setting.ServiceName = setting.SettingKey.Substring(dto.Key.IndexOf(".") + 1, dto.Key.LastIndexOf(".")).ToUpper();
            }
            _context.Settings.Add(setting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSettingAsync(string key, string value, CancellationToken cancellationToken)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(x => x.SettingKey == key);
            if (setting == null)
                return;

            setting.SettingValue = value;
            _context.Update(setting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSettingsAsync(List<SettingKeyPairUpdateRequest> request, CancellationToken cancellationToken)
        {
           foreach(var keypair in request)
            {
                await UpdateSettingAsync(keypair.Key, keypair.Value, cancellationToken);
            }
        }
    }
}
