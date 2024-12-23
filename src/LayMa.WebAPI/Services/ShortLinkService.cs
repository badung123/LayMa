﻿using System;
using System.Security.Cryptography;

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
	}
}
