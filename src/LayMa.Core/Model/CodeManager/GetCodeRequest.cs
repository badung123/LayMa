using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.CodeManager
{
	public class GetCodeRequest
	{
        public string TrafficId { get; set; }
        public string Solution { get; set; }
        public string? HCaptchaToken { get; set; }
		public string? HCaptchaTokenDuPhong { get; set; }
		//public dynamic TrafficId { get; set; }
	}
}
