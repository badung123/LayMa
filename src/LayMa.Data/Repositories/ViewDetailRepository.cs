using AutoMapper;
using LayMa.Core.Domain.Link;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LayMa.Core.Constants.Permissions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
			var count = await _context.ViewDetails
				.Where(x => x.ShortLinkId == shortLinkId && x.DateCreated >= start && x.DateCreated <= end)
				.DistinctBy(x=> new {x.CampainId,x.ShortLinkId,x.Device,x.IPAddress})
				.CountAsync();
			return count;
		}
		public async Task<int> CountClickByDateRangeAndUserId(DateTime start, DateTime end, Guid userId)
		{
			var count = await _context.ViewDetails
				.Where(x => x.UserId == userId && x.DateCreated >= start && x.DateCreated <= end)
				//.GroupBy(x => new { x.CampainId, x.ShortLinkId, x.Device, x.IPAddress })
				.CountAsync();
			return count;
		}
		public async Task<List<ThongKeClickViewInMonth>> CountClickByDateUserIdInMonth(Guid userId)
		{
			var date = DateTime.Now;
			var start = date.Date.AddDays(-30);
			var query =  _context.ViewDetails.AsQueryable();
			var list = query.Where(x => x.UserId == userId && x.DateCreated >= start && x.DateCreated < date)
				.GroupBy(x => x.DateCreated.Date)
				.Select(x => new ThongKeClickViewInMonth
				{
					Date = x.Key,
					Count = x.Count()
				}).ToList();
			return list;
		}
		//public async Task<List<dynamic>> GetTopFourView(DateTime startTime,DateTime endTime)
		//{
		//	var query = _context.ViewDetails.AsQueryable();
		//	var list = query.Where(x => x.DateCreated >= startTime && x.DateCreated < endTime)
		//		.GroupBy(x => x.UserId)
		//		.Select(x => new ThongKeClickViewInMonth
		//		{
		//			Date = x.Key,
		//			Count = x.Count()
		//		}).ToList();
		//	return list;
		//}


		public async Task<int> CountClickByDateRangeAndCampainId(DateTime start, DateTime end, Guid campainId)
		{
			var count = await _context.ViewDetails
				.Where(x => x.CampainId == campainId && x.DateCreated >= start && x.DateCreated <= end)
				//.GroupBy(x => new { x.CampainId, x.ShortLinkId, x.Device, x.IPAddress })
				.CountAsync();
			return count;
		}
		
		public async Task<int> CountClickByDateRange(DateTime start, DateTime end)
		{
            var count = await _context.ViewDetails
				.Where(x =>x.DateCreated >= start && x.DateCreated <= end)
				//.GroupBy(x => new { x.CampainId, x.ShortLinkId, x.Device, x.IPAddress })
				.CountAsync();
            return count;
        }
		public async Task<bool> CheckIP(string ip, string screenDevice)
		{
			var date = DateTime.Now;
			var start = date.Date;
			var end = date.Date.AddDays(1);
			var countIP = await _context.ViewDetails.CountAsync(x=> start <=x.DateCreated && x.DateCreated < end && x.IPAddress == ip);
			return countIP < 2 ? true : false;
		}
        public async Task<int> CountIP(string ip)
        {
            var countIP = 0;
            var date = DateTime.Now;
            var start = date.Date;
            var end = date.Date.AddDays(1);
            countIP = await _context.ViewDetails.CountAsync(x => start <= x.DateCreated && x.DateCreated < end && x.IPAddress == ip);
            return countIP;
        }
        public async Task<bool> CheckUserAgent(string usergent, string screenDevice)
        {
            var date = DateTime.Now;
            var start = date.AddHours(-1);
            var isValid = await _context.ViewDetails.Where(x => start <= x.DateCreated && x.DateCreated < date).AnyAsync(x => x.UserAgent == usergent);
            return !isValid;
        }
		public async Task<DateTime> GetTimeSuccess(Guid shortLinkId, Guid userId, string screen)
		{
			return await _context.ViewDetails.Where(x => x.UserId == userId && x.ShortLinkId == shortLinkId && x.DeviceScreen == screen).OrderByDescending(x => x.DateCreated).Select(x => x.DateCreated).FirstOrDefaultAsync();

		}

		public async Task<List<ThongKeViewClickByUser>> GetTopUsersByClicks(DateTime start, DateTime end, int top = 4)
		{
			var query = _context.ViewDetails
				.Where(x => x.DateCreated >= start && x.DateCreated <= end)
				.GroupBy(x => x.UserId)
				.Select(g => new
				{
					UserId = g.Key,
					ClickCount = g.Count()
				})
				.OrderByDescending(x => x.ClickCount)
				.Take(top);

			var topUsers = await query.ToListAsync();

			var userIds = topUsers.Select(x => x.UserId).ToList();
			var users = await _context.Users
				.Where(u => userIds.Contains(u.Id))
				.ToListAsync();

			var result = topUsers.Select(tu => new ThongKeViewClickByUser
			{
				Id = tu.UserId,
				UserName = users.FirstOrDefault(u => u.Id == tu.UserId)?.UserName ?? "Unknown",
				Click = tu.ClickCount,
				View = 0 // We can add view count later if needed
			}).ToList();

			return result;
		}

	}
}
