using LayMa.Core.ConfigOptions;
using LayMa.Core.Model.Auth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LayMa.WebAPI.Services
{
    public class HCaptchaService : IHCaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly HCaptchaSettings _settings;

        public HCaptchaService(HttpClient httpClient, IOptions<HCaptchaSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<bool> VerifyAsync(string token, string? remoteIp = null)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            if (!_settings.Enabled)
                return true; // Skip verification if disabled

            try
            {
                var response = await VerifyWithResponseAsync(token, remoteIp);
                return response.Success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<HCaptchaVerificationResponse> VerifyWithResponseAsync(string token, string? remoteIp = null)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token cannot be null or empty", nameof(token));

            if (!_settings.Enabled)
                return new HCaptchaVerificationResponse { Success = true }; // Skip verification if disabled

            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("secret", _settings.SecretKey),
                new KeyValuePair<string, string>("response", token)
            };

            if (!string.IsNullOrEmpty(remoteIp))
            {
                formData.Add(new KeyValuePair<string, string>("remoteip", remoteIp));
            }

            var content = new FormUrlEncodedContent(formData);
            var response = await _httpClient.PostAsync(_settings.VerifyUrl, content);
            
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HCaptchaVerificationResponse>(responseContent) 
                   ?? new HCaptchaVerificationResponse { Success = false };
        }
    }
}
