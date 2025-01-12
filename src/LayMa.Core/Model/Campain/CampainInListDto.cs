using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.Campain
{
	public class CampainInListDto
	{
		public Guid Id { get; set; }
		public string? KeySearch { get; set; }
		public string KeyToken { get; set; }
		public string Flatform { get; set; }
		public string ImageUrl { get; set; }
		public string Decription { get; set; }
		public string Url { get; set; }
		public string? Domain { get; set; }
		public int ViewPerDay { get; set; }
		public long ToTalView { get; set; }
		public int PricePerView { get; set; }
		public int TimeOnSitePerView { get; set; }
		public bool Status { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
		public class AutoMapperProfiles : Profile
		{
			public AutoMapperProfiles()
			{
				CreateMap<LayMa.Core.Domain.Campain.Campain, CampainInListDto>();
			}
		}
	}
}
