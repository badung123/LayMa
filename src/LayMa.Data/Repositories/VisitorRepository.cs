using AutoMapper;
using LayMa.Core.Domain.Mission;
using LayMa.Core.Domain.Visitor;
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
	}
}
