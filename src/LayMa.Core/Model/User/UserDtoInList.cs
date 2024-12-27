using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Model.Campain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.User
{
	public class UserDtoInList
	{
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public double Balance { get; set; }
		public string? UserTelegram { get; set; }
		public string? RefCode { get; set; }
		public string? Agent { get; set; }
		public string? Origin { get; set; }
		public string? OriginImage { get; set; }
		public bool IsVerify { get; set; }
		public DateTime? VerifyDateTime { get; set; }
        public string? Email { get; set; }
		public string? UserName { get; set; }
		public class AutoMapperProfiles : Profile
		{
			public AutoMapperProfiles()
			{
				CreateMap<AppUser, UserDtoInList>();
			}
		}
	}
}
