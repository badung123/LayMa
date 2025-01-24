using AutoMapper;
using LayMa.Core.Domain.Commment;
using LayMa.Core.Model.Campain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.Comment
{
	public class CommentDto
	{
		public Guid Id { get; set; }
		public string? Account { get; set; }
		public string? Message { get; set; }
		public DateTime DateCreated { get; set; }
		public class AutoMapperProfiles : Profile
		{
			public AutoMapperProfiles()
			{
				CreateMap<Commments, CommentDto>();
			}
		}
	}
}
