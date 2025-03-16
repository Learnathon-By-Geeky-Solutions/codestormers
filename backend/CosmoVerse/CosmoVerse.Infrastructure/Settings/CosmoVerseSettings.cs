using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmoVerse.Infrastructure.Settings
{
    public class CosmoVerseSettings
    {
        public const string SettingName = "AppSettings";
        public string Token { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

    }
}
