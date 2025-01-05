using LayMa.WebApp.Constant;

namespace LayMa.WebApp.Models
{
    public class CaptchaViewModel
    {
        public bool IsEnabled { get; set; }
        public string SiteKey { get; set; }
        public string Action { get; set; }
        public string Version { get; set; }

        public bool IsVersion2() => Version == GoogleCaptchaConstants.Version_2;
        public bool IsVersion3() => Version == GoogleCaptchaConstants.Version_3;

        public override string ToString() => $"{nameof(IsEnabled)}: {IsEnabled}, {nameof(SiteKey)}: {SiteKey}, {nameof(Action)}: {Action}, {nameof(Version)}: {Version}";
    }
}
