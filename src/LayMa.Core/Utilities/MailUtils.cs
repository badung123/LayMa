﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Utilities
{
	public static class MailUtils
	{
		public static async Task<bool> SendMail(string _from, string _to, string _subject, string _body, SmtpClient client)
		{
			// Tạo nội dung Email
			MailMessage message = new MailMessage(
				from: _from,
				to: _to,
				subject: _subject,
				body: _body
			);
			message.BodyEncoding = System.Text.Encoding.UTF8;
			message.SubjectEncoding = System.Text.Encoding.UTF8;
			message.IsBodyHtml = true;
			message.ReplyToList.Add(new MailAddress(_from));
			message.Sender = new MailAddress(_from);


			try
			{
				await client.SendMailAsync(message);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
		public static async Task<bool> SendMailGoogleSmtp(string _to, string _subject,
															string _body)
		{
			string _from = "blockacctml36@gmail.com";
			string _gmailsend = "blockacctml36@gmail.com";
			string _gmailpassword = "qrmx dagt mckt mzqw";
			MailMessage message = new MailMessage(
				from: _from,
				to: _to,
				subject: _subject,
				body: _body
			);
			message.BodyEncoding = System.Text.Encoding.UTF8;
			message.SubjectEncoding = System.Text.Encoding.UTF8;
			message.IsBodyHtml = true;
			message.ReplyToList.Add(new MailAddress(_from));
			message.Sender = new MailAddress(_from);

			// Tạo SmtpClient kết nối đến smtp.gmail.com
			try
			{
				using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
				{
					client.Port = 587;
					client.Credentials = new NetworkCredential(_gmailsend, _gmailpassword);
					client.EnableSsl = true;
					await client.SendMailAsync(message);
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
			

		}
	}
}
