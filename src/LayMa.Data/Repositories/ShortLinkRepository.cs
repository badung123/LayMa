using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Model;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Data.Repositories
{
	public class ShortLinkRepository : RepositoryBase<ShortLink,Guid>,IShortLinkRepository
	{
		private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;
		public ShortLinkRepository(LayMaContext context, IMapper mapper,
			UserManager<AppUser> userManager) : base(context)
		{
			_mapper = mapper;
			_userManager = userManager;
		}
		public async Task<PagedResult<ShortLinkInListDto>> GetAllPaging(Guid currentUserId, int pageIndex = 1, int pageSize = 10, string? keySearch = "")
		{
			var query = _context.ShortLinks.AsQueryable();
			query = query.Where(x => x.UserId == currentUserId);
            if (!String.IsNullOrEmpty(keySearch))
            {
				query = query.Where(x => x.OriginLink.Contains(keySearch));
			}
            var totalRow = await query.CountAsync();

			query = query.OrderByDescending(x => x.DateCreated)
			   .Skip((pageIndex - 1) * pageSize)
			   .Take(pageSize);

			return new PagedResult<ShortLinkInListDto>
			{
				Results = await _mapper.ProjectTo<ShortLinkInListDto>(query).ToListAsync(),
				CurrentPage = pageIndex,
				RowCount = totalRow,
				PageSize = pageSize
			};
		}
		public async Task<ShortLink> GetByTokenAsync(string token)
		{
			var shortLink = await _context.ShortLinks.FirstOrDefaultAsync(x=> x.Token == token);
			return shortLink;
		}
		public async Task UpdateViewCount(Guid id)
		{
			var link = await _context.ShortLinks.FirstOrDefaultAsync(x => x.Id == id);
			link.ViewCount += 1;
			_context.ShortLinks.Update(link);
		}
        public async Task<List<ShortLinkInListDto>> GetTopLink(Guid currentUserId, int top) {
            var query = _context.ShortLinks.AsQueryable();
            query = query.Where(x => x.UserId == currentUserId);
			query = query.OrderByDescending(x => x.DateCreated).Take(top);
			return await _mapper.ProjectTo<ShortLinkInListDto>(query).ToListAsync();
        }
		public async Task<List<Guid>> GetListShortLinkIDOfUser(Guid userId)
		{
            var query = await _context.ShortLinks.Where(x => x.UserId == userId).Select(x=> x.Id).ToListAsync();
			return query;
        }
		public async Task UpdateOriginOfShortLink(string origin, Guid shortlinkId)
		{
            var link = await _context.ShortLinks.FirstOrDefaultAsync(x => x.Id == shortlinkId);
            link.Origin = origin;
            _context.ShortLinks.Update(link);
        }
        public async Task UpdateView(Guid id)
        {
            var link = await _context.ShortLinks.FirstOrDefaultAsync(x => x.Id == id);
            link.View += 1;
            _context.ShortLinks.Update(link);
        }
    }
}
