using System;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LayMa.WebAPI.Services
{
	public class ShortLinkService : IShortLinkService
	{
		private static Random random = new Random();
		public string GenerateLinkToken(int number)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, number)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
		public string HideEndLastWord(string word,int numberHide)
		{
			return word.Substring(0, word.Length - numberHide);
		}
	}
}
