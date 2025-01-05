namespace LayMa.WebApp.Models
{
    public class CaptchaVerificationRequestModel
    {
        public required string Token { get; set; }
        public string? RemoteIp { get; set; }
        public bool HasRemoteIp()
        {
            return RemoteIp != null;
        }
    }
}
