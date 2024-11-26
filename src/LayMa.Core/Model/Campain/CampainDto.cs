using AutoMapper;
using LayMa.Core.Domain.Campain;
using LayMa.Core.Model.KeySearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.Campain
{
	public class CampainDto
	{
		public Guid Id { get; set; }
		public string KeyToken { get; set; }
		public class AutoMapperProfiles : Profile
		{			
			public AutoMapperProfiles()
			{
				CreateMap<LayMa.Core.Domain.Campain.Campain, CampainDto>();
			}
		}
	}
}
