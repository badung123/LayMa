using Newtonsoft.Json;

namespace LayMa.WebApp.Models
{
    public class CaptchaVerificationV2ResponseModel
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime Date { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        public override string ToString() => $"{nameof(Success)}: {Success}, {nameof(Date)}: {Date}, {nameof(Hostname)}: {Hostname}";
    }
    public class CaptchaVerificationV3ResponseModel : CaptchaVerificationV2ResponseModel
    {
        [JsonProperty("score")]
        public decimal Score { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        public override string ToString() => $"{base.ToString()}, {nameof(Score)}: {Score}, {nameof(Action)}: {Action}";
    }
}
