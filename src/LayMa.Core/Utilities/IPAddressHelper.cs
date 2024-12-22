using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Globalization;

namespace LayMa.Core.Utilities
{
    public static class IPAddressHelper
    {
        public static string GetIpAddress(this HttpRequest request)
        {
            

            var ipAddress = request?.Headers?["X-Real-IP"].ToString();

            if (!string.IsNullOrEmpty(ipAddress))

            {

                return ipAddress;

            }

            ipAddress = request?.Headers?["X-Forwarded-For"].ToString();

            if (!string.IsNullOrEmpty(ipAddress))

            {

                var parts = ipAddress.Split(',');

                if (parts.Count() > 0)

                {

                    ipAddress = parts[0];

                }

                return ipAddress;

            }

            ipAddress = request?.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            if (!string.IsNullOrEmpty(ipAddress))

            {

                return ipAddress;

            }

            return string.Empty;

        }

        //public static string GetDeviceInfo(this HttpRequest request)
        //{
        //    return "";
        //}
		public static bool IsMobile(this HttpRequest request)
		{
            var userAgent = request.Headers["user-agent"].ToString();
			if (string.IsNullOrEmpty(userAgent))
				return false;
			//tablet
			if (Regex.IsMatch(userAgent, "(tablet|ipad|playbook|silk)|(android(?!.*mobile))", RegexOptions.IgnoreCase))
				return true;
			//mobile
			const string mobileRegex =
				"blackberry|iphone|mobile|windows ce|opera mini|htc|sony|palm|symbianos|ipad|ipod|blackberry|bada|kindle|symbian|sonyericsson|android|samsung|nokia|wap|motor";

			if (Regex.IsMatch(userAgent, mobileRegex, RegexOptions.IgnoreCase)) return true;
			//not mobile 
			return false;
		}

		public static bool IsValidEmail(this string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				return false;

			try
			{
				// Normalize the domain
				email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
									  RegexOptions.None, TimeSpan.FromMilliseconds(200));

				// Examines the domain part of the email and normalizes it.
				string DomainMapper(Match match)
				{
					// Use IdnMapping class to convert Unicode domain names.
					var idn = new IdnMapping();

					// Pull out and process domain name (throws ArgumentException on invalid)
					string domainName = idn.GetAscii(match.Groups[2].Value);

					return match.Groups[1].Value + domainName;
				}
			}
			catch (RegexMatchTimeoutException e)
			{
				return false;
			}
			catch (ArgumentException e)
			{
				return false;
			}

			try
			{
				return Regex.IsMatch(email,
					@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
					RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
			}
			catch (RegexMatchTimeoutException)
			{
				return false;
			}
		}
	}
}
