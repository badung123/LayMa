using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.Mission
{
	public class MissionDto
	{
		public Guid Id { get; set; }
		public string? Key { get; set; }
		public string? UrlImage { get; set; }
		public string? UrlWeb { get; set; }
		public string? UrlVideo { get; set; }
		public string? Flatfrom { get; set; }
		public string? UrlFacebook { get; set; }
		public Guid CampainId { get; set; }
	}
}
