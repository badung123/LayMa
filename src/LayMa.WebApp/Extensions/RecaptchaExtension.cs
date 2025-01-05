using LayMa.WebApp.Models;
using Newtonsoft.Json;

namespace LayMa.WebApp.Extensions
{
    public class RecaptchaExtension : IRecaptchaExtension
    {
        private IConfiguration _configuration { get; }
        private static string GoogleSecretKey { get; set; }
        private static string GoogleRecaptchaVerifyApi { get; set; }
        private static decimal RecaptchaThreshold { get; set; }
        public RecaptchaExtension(IConfiguration configuration)
        {
            _configuration = configuration;

            GoogleRecaptchaVerifyApi = _configuration.GetSection("GoogleRecaptcha").GetSection("VefiyAPIAddress").Value ?? "";
            GoogleSecretKey = _configuration.GetSection("GoogleRecaptcha").GetSection("Secretkey").Value ?? "";

            RecaptchaThreshold = (decimal)0.5;
        }
        public async Task<bool> VerifyAsync(string token)
        {
            if (String.IsNullOrEmpty(token))
            {
                throw new Exception("Token cannot be null!");
            }
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync($"{GoogleRecaptchaVerifyApi}?secret={GoogleSecretKey}&response={token}");
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(response);
                if (!tokenResponse.Success)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
