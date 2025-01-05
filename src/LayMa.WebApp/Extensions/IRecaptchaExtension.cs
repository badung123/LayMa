namespace LayMa.WebApp.Extensions
{
    public interface IRecaptchaExtension
    {
        Task<bool> VerifyAsync(string token);
    }
}
