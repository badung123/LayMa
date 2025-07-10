using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.User
{
    public class UpdateClickRateRequest
    {
		public required Guid UserId { get; set; }
		public required int MaxClickInDay { get; set; }
		public required int Rate { get; set; }
	}
}
