namespace LayMa.Core.ConfigOptions
{
    public class HCaptchaSettings
    {
        public bool Enabled { get; set; } = true;
        public string SecretKey { get; set; } = string.Empty;
        public string VerifyUrl { get; set; } = "https://hcaptcha.com/siteverify";
		public string KeyDuPhong { get; set; } = string.Empty;
	}
}
