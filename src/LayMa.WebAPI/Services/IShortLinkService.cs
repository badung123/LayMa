namespace LayMa.WebAPI.Services
{
	public interface IShortLinkService
	{
		string GenerateLinkToken(int number);
		string HideEndLastWord(string word,int number);
	}
}
