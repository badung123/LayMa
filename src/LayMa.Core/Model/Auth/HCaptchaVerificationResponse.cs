using Newtonsoft.Json;

namespace LayMa.Core.Model.Auth
{
    public class HCaptchaVerificationResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime? ChallengeTimestamp { get; set; }

        [JsonProperty("hostname")]
        public string? Hostname { get; set; }

        [JsonProperty("credit")]
        public bool Credit { get; set; }

        [JsonProperty("error-codes")]
        public string[]? ErrorCodes { get; set; }
    }
}
