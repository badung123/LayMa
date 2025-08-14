namespace LayMa.Core.ConfigOptions
{
    public class HCaptchaSettings
    {
        public bool Enabled { get; set; } = true;
        public string SiteKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string VerifyUrl { get; set; } = "https://hcaptcha.com/siteverify";
    }
}
