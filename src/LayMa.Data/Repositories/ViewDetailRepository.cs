using AutoMapper;
using LayMa.Core.Domain.Link;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LayMa.Core.Constants.Permissions;

namespace LayMa.Data.Repositories
{
	public class ViewDetailRepository : RepositoryBase<ViewDetail, Guid>, IViewDetailRepository
	{
		private readonly IMapper _mapper;
		public ViewDetailRepository(LayMaContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}
		public async Task<List<ViewDetail>> GetTopListViewDetail(int top)
		{
			return new List<ViewDetail>();
		}
		public async Task<int> CountClickByDateRangeAndShortLink(DateTime start, DateTime end, Guid shortLinkId)
		{
			var count = await _context.ViewDetails.Where(x => x.ShortLinkId == shortLinkId && x.DateCreated >= start && x.DateCreated <= end).CountAsync();
			return count;
		}
		public async Task<int> CountClickByDateRangeAndUserId(DateTime start, DateTime end, Guid userId)
		{
			var count = await _context.ViewDetails.Where(x => x.UserId == userId && x.DateCreated >= start && x.DateCreated <= end).CountAsync();
			return count;
		}
		
		public async Task<int> CountClickByDateRangeAndCampainId(DateTime start, DateTime end, Guid campainId)
		{
			var count = await _context.ViewDetails.Where(x => x.CampainId == campainId && x.DateCreated >= start && x.DateCreated <= end).CountAsync();
			return count;
		}
		
		public async Task<int> CountClickByDateRange(DateTime start, DateTime end)
		{
            var count = await _context.ViewDetails.Where(x =>x.DateCreated >= start && x.DateCreated <= end).CountAsync();
            return count;
        }
		public async Task<bool> CheckIP(string ip, string screenDevice)
		{
			var date = DateTime.Now;
			var start = date.Date;
			var end = date.Date.AddDays(1);
			var isValid = await _context.ViewDetails.Where(x=> start <=x.DateCreated && x.DateCreated < end).AnyAsync(x => x.IPAddress == ip);
			return !isValid;
		}
        public async Task<bool> CheckUserAgent(string usergent, string screenDevice)
        {
            var date = DateTime.Now;
            var start = date.AddHours(-1);
            var isValid = await _context.ViewDetails.Where(x => start <= x.DateCreated && x.DateCreated < date).AnyAsync(x => x.UserAgent == usergent);
            return !isValid;
        }

    }
}
