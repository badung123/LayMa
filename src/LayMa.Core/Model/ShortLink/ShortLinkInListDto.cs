using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.ShortLink
{
	public class ShortLinkInListDto
	{
        public Guid Id{ get; set; }
        public required string Link { get; set; }
		public required string OriginLink { get; set; }
        public string? Origin { get; set; }
		public string? Duphong { get; set; }
		public int ViewCount { get; set; }
		public required string Token { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
		public class AutoMapperProfiles : Profile
		{
			public AutoMapperProfiles()
			{
				CreateMap<LayMa.Core.Domain.Link.ShortLink, ShortLinkInListDto>();
			}
		}
	}
}
