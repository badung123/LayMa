using AutoMapper;
using LayMa.Core.Model.ShortLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.KeySearch
{
	public class KeySeoDto
	{
		public Guid Id { get; set; }
		public string? Key { get; set; }
        public int MaxViewDay { get; set; }
        public int ViewedInDay { get; set; }
        public string? UrlImage { get; set; }
		public string? UrlWeb { get; set; }
		public string? UrlVideo { get; set; }
		public class AutoMapperProfiles : Profile
		{
			public AutoMapperProfiles()
			{
				CreateMap<LayMa.Core.Domain.Link.KeySearch, KeySeoDto>();
			}
		}
	}
}
