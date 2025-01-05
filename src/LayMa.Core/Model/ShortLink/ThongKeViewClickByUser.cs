using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.ShortLink
{
	public class ThongKeViewClickByUser
	{
        public Guid Id { get; set; }
        public string UserName { get; set; }
		public int Click { get; set; }
		public int View { get; set; }
		public class AutoMapperProfiles : Profile
		{
			public AutoMapperProfiles()
			{
				CreateMap<AppUser, ThongKeViewClickByUser>();
			}
		}
	}
}
