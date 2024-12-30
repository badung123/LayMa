using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Domain.Link
{
	[Table("ViewDetail")]
	public class ViewDetail
	{
		[Key]
		public Guid Id { get; set; }
		public Guid ShortLinkId { get; set; }
		public Guid CampainId { get; set; }
		public string? IPAddress { get; set; }
        public string? Device { get; set; }
		public string? DeviceScreen { get; set; }
		public string? UserAgent { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
	}
}
