using AutoMapper;
using LayMa.Core.Domain.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.ShortLink
{
	public class LogShortLinkDto
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public string? UserName { get; set; }
		public long OldBalance { get; set; }
		public long Amount { get; set; }
		public string? Description { get; set; }
		public string? CreatedBy { get; set; }
		public string? DeviceScreen { get; set; }
		public string? UserAgent { get; set; }
		public string? IPAddress { get; set; }
		public string? ShortLink { get; set; }
		public TranSactionType TranSactionType { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
        public string? Flatform { get; set; }
		public int? Solution { get; set; }
		public int? TimeFinish { get; set; }
		public class AutoMapperProfiles : Profile
		{
			public AutoMapperProfiles()
			{
				CreateMap<TransactionLog, LogShortLinkDto>();
			}
		}
	}
}
