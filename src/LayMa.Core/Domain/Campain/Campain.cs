using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Domain.Campain
{
	[Table("Campains")]
	public class Campain
	{
		[Key]
		public Guid Id { get; set; }
        public string? KeySearch { get; set; }
		[Required]
		[MaxLength(50)]
		public required string KeyToken { get; set; }
        public string? ImageUrl { get; set; }
		public string? VideoUrl { get; set; }
		public string? Domain { get; set; }
		public required string Url { get; set; }
        public int ViewPerDay { get; set; }
		public int PricePerView { get; set; }
		public int TimeOnSitePerView { get; set; }
		public long ToTalView { get; set; }
		public long ToTalPrice { get; set; }
		public string? Decription { get; set; }
		public string? Flatform { get; set; }
		public long RemainView { get; set; }
		public bool Status { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
	}
}
