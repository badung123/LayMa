using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.CodeManager
{
	public class InsertCodeRequest
	{
		public required string Key { get; set; }
		public required string Code { get; set; }
	}
}
