using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Domain.Link
{
	[Table("ShortLink")]
	[Index(nameof(Token), IsUnique = true)]
	public class ShortLink
	{
		[Key]
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		[Required]
		[MaxLength(128)]
		public required string Link { get; set; }
		[Required]
		[MaxLength(500)]
		public required string OriginLink { get; set; }
        [MaxLength(500)]
        public string? Origin { get; set; }
        public int ViewCount { get; set; }
        public int View { get; set; }
        public required string Token { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
	}
}
