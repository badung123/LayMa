using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Domain.Link
{
	[Table("KeySearch")]
	public class KeySearch
	{
		[Key]
		public Guid Id { get; set; }
		[Required]
		[MaxLength(100)]
		public required string Key { get; set; }
        public required string UrlImage { get; set; }
        public required string UrlWeb { get; set; }
        public int MaxViewDay { get; set; }
        public int ViewedInDay { get; set; }
        public string? UrlVideo { get; set; }
		public bool IsActive { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
	}
}
