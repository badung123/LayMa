﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.CodeManager
{
	public class CheckCodeRequest
	{
		public required string Code { get; set; }
		public required string Token { get; set; }
		public required string CampainId { get; set; }
		
	}
}
