using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.ShortLink
{
	public class CreateShortLinkDto
	{
		public required string Url { get; set; }
		public class AutoMapperProfiles : Profile
		{
			public AutoMapperProfiles()
			{
				CreateMap<CreateShortLinkDto, LayMa.Core.Domain.Link.ShortLink>();
			}
		}
	}
	
}
