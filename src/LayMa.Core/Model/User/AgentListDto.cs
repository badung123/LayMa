using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.User
{
	public class AgentListDto
	{
		public Guid MemberId { get; set; }
		public required string UserName { get; set; }
		public DateTime DateCreated { get; set; }
		public bool IsActive { get; set; }
		public bool IsVerify { get; set; }
	}
}
