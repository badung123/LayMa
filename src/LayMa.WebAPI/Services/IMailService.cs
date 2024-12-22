using LayMa.Core.Model.Auth;

namespace LayMa.WebAPI.Services
{
	public interface IMailService
	{
		bool SendMail(MailData Mail_Data);
	}
}
