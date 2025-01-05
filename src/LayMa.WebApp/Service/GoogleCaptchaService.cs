using LayMa.WebApp.Constant;
using LayMa.WebApp.Interface;
using LayMa.WebApp.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LayMa.WebApp.Service
{
    public class GoogleCaptchaService : ICaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly GoogleCaptchaOptions _options;

        public GoogleCaptchaService(HttpClient httpClient, IOptions<GoogleCaptchaOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<CaptchaVerificationV2ResponseModel> VerifyV2Async(string token)
        {
            string verification = await _httpClient.GetStringAsync($"recaptcha/api/siteverify?secret={_options.Secret}&response={token}");

            return JsonConvert.DeserializeObject<CaptchaVerificationV2ResponseModel>(verification);
        }

        public async Task<CaptchaVerificationV3ResponseModel> VerifyV3Async(CaptchaVerificationRequestModel request)
        {
            var requestInput = new Dictionary<string, string>
        {
            { "secret", _options.Secret },
            { "response", request.Token }
        };

            if (request.HasRemoteIp())
                requestInput.Add("remoteip", request.RemoteIp);

            HttpResponseMessage response = await _httpClient.PostAsync("recaptcha/api/siteverify", new FormUrlEncodedContent(requestInput));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CaptchaVerificationV3ResponseModel>();
        }
    }
}
