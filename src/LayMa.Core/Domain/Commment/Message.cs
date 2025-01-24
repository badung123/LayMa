using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Domain.Commment
{
	[Table("Messages")]
	public class Messages
	{
		[Key]
		public Guid Id { get; set; }
		public string? Message { get; set; }
		public bool IsUsed { get; set; }
		public DateTime? DateUsed { get; set; }
	}
}
