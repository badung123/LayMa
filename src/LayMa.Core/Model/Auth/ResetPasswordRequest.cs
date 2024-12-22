using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.Auth
{
	public class ResetPasswordRequest
	{
        public required string Email { get; set; }
		public required string Token { get; set; }
		public required string Password { get; set; }
	}
}
