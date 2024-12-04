namespace LayMa.WebAPI.Services
{
	public interface IShortLinkService
	{
		string GenerateLinkToken(int number);
	}
}
