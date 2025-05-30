using AutoMapper;
using LayMa.Core.Domain.Mission;
using LayMa.Core.Domain.Visitor;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Data.Repositories
{
	public class VisitorRepository : RepositoryBase<Visitor, Guid>, IVisitorRepository
	{
		private readonly IMapper _mapper;
		public VisitorRepository(LayMaContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}
		public async Task<int> CountViewByDateRangeAndShortLink(DateTime from, DateTime to, Guid shortLinkId)
		{
			var count = await _context.Visitors.Where(x => x.ShortLinkId == shortLinkId && x.DateCreated >= from && x.DateCreated <= to).CountAsync();
			return count;
		}		
		public async Task<int> CountViewByDateRangeAndUserId(DateTime from, DateTime to, Guid userId)
		{
			var count = await _context.Visitors.Where(x => x.UserId == userId && x.DateCreated >= from && x.DateCreated <= to).CountAsync();
			return count;
		}
		public async Task<List<ThongKeClickViewInMonth>> CountViewByDateUserIdInMonth(Guid userId)
		{
			var date = DateTime.Now;
			var start = date.Date.AddDays(-30);
			var query = _context.Visitors.AsQueryable();
			var list = query.Where(x => x.UserId == userId && x.DateCreated >= start && x.DateCreated < date)
				.GroupBy(x => x.DateCreated.Date)
				.Select(x => new ThongKeClickViewInMonth
				{
					Date = x.Key,
					Count = x.Count()
				}).ToList();
			return list;
		}
		public async Task<DateTime> GetStartTimeOfShortLink(Guid userId, Guid shortLinkId)
		{
			var visitor = await _context.Visitors.Where(x => x.UserId == userId && x.ShortLinkId == shortLinkId).OrderByDescending(x => x.DateCreated).FirstOrDefaultAsync();
			return visitor.DateCreated;
		}
		public async Task<List<DateTime>> GetListTimeVisitShortLink(Guid shortLinkId, Guid userId, string screen)
		{
			return await _context.Visitors.Where(x => x.UserId == userId && x.ShortLinkId == shortLinkId && x.DeviceScreen == screen).OrderByDescending(x => x.DateCreated).Select(x => x.DateCreated).ToListAsync();
			
		}
	}
}
