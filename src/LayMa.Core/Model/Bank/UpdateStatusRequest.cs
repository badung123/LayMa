using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.Bank
{
	public class UpdateStatusRequest
	{
        public int Type { get; set; }
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public long Money { get; set; }
	}
}
