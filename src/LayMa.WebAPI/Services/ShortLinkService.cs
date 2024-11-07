using System.Security.Cryptography;

namespace LayMa.WebAPI.Services
{
	public class ShortLinkService : IShortLinkService
	{
		public string GenerateLinkToken()
		{
			var randomNumber = new byte[7];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}
	}
}
