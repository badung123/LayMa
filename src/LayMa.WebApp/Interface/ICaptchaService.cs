using LayMa.WebApp.Models;

namespace LayMa.WebApp.Interface
{
    public interface ICaptchaService
    {
        Task<CaptchaVerificationV2ResponseModel> VerifyV2Async(string token);
        Task<CaptchaVerificationV3ResponseModel> VerifyV3Async(CaptchaVerificationRequestModel request);
    }
}
