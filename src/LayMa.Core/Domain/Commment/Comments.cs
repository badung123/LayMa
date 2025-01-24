using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Domain.Commment
{
	[Table("Commments")]
	public class Commments
	{
		[Key]
		public Guid Id { get; set; }
		public string? Account { get; set; }
		public string? Message { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }

	}
}
