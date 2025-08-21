using LayMa.Core.Model.Auth;

namespace LayMa.WebAPI.Services
{
    public interface IHCaptchaService
    {
        Task<bool> VerifyAsync(string token, string? remoteIp = null);
		bool VerifyDuPhongAsync(string token);
		Task<HCaptchaVerificationResponse> VerifyWithResponseAsync(string token, string? remoteIp = null);
    }
}
