using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Domain.Visitor
{
	[Table("Visitors")]
	public class Visitor
	{
		[Key]
		public Guid Id { get; set; }
		public Guid? UserId { get; set; }
		public Guid ShortLinkId { get; set; }
		public Guid CampainId { get; set; }
        public required string VisitorId { get; set; }
		public string? Country { get; set; }
		public string? Region { get; set; }
		public string? City { get; set; }
		public string? DeviceType { get; set; }
		public string? DeviceScreen { get; set; }
		public string? DeviceBrand { get; set; }
		public string? DeviceModel { get; set; }
		public string? OS { get; set; }
		public string? OSVersion { get; set; }
		public string? Browser { get; set; }
		public string? BrowserVersion { get; set; }
		public string? BrowserMajorVersion { get; set; }
        public bool Mobile { get; set; }
		public bool Cookies { get; set; }
		public string? Referer { get; set; }
		public string? RefererHost { get; set; }
		public string? UserAgent { get; set; }
		public string? Code { get; set; }
		public DateTime? FinishedAt { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }

	}
}
