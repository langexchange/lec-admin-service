using System.Collections.Generic;

namespace LE.AdminService.Models.Requests
{
    public class SettingKeyPairRequest
    {
        public string Locale { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class SettingKeyPairUpdateRequest
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
