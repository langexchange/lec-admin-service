using System;
using System.Collections.Generic;

namespace LE.AdminService.Dtos
{
    public class LocaleSettingDto
    {
        public LocaleSettingDto()
        {
            Settings = new List<SettingDto>();
        }

        public string Locale { get; set; }
        public List<SettingDto> Settings { get; set; }
    }
    public class SettingDto
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
