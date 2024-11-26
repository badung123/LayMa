using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Domain.Mission
{
	[Table("Missions")]
	public class Mission
	{
		[Key]
		public Guid Id { get; set; }
		public Guid CampainId { get; set; }
		public Guid UserId { get; set; }
		public Guid ShortLinkId { get; set; }
		[Required]
		[MaxLength(50)]
		public required string TokenUrl { get; set; }
		public  string? ShortLink { get; set; }
		public bool IsActive { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
        
    }
}
