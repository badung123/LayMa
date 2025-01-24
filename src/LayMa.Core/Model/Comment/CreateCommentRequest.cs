using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.Comment
{
	public class CreateCommentRequest
	{
        public required string Account { get; set; }
		public required string Message { get; set; }
	}
}
