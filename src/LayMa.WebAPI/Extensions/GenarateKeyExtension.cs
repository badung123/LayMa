namespace LayMa.WebAPI.Extensions
{
	public static class GenarateKeyExtension
	{
		private static Random random = new Random();
		public static string GenerateLinkToken(this String stringChar,int number)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, number)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
