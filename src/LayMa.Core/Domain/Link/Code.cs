using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Domain.Link
{
	[Table("Code")]
	public class Code
	{
		[Key]
		public Guid Id { get; set; }
		public Guid KeySearchId { get; set; }
		public Guid CampainId { get; set; }
		public required string CodeString { get; set; }
        public bool IsUsed { get; set; }
        public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
	}
}
