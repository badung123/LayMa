using AutoMapper;
using LayMa.Core.Domain.Bank;
using LayMa.Core.Domain.Campain;
using LayMa.Core.Interface;
using LayMa.Core.Model;
using LayMa.Core.Model.Campain;
using LayMa.Core.Model.KeySearch;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LayMa.Core.Constants.AdminPermissions;

namespace LayMa.Data.Repositories
{
    public class CampainRepository : RepositoryBase<Campain, Guid>, ICampainRepository
	{
		private readonly IMapper _mapper;
		public CampainRepository(LayMaContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}
		public async Task<CampainDto> GetCampainByKeyToken(string key, string flatform)
		{
			var campain = await _context.Campains.Where(x => x.KeyToken == key && x.Flatform == flatform && x.Status).FirstOrDefaultAsync();
			var campainDto = _mapper.Map<Campain,CampainDto>(campain);

			return campainDto;
		}
		public async Task<Guid> GetCampainIdRandom()
		{
			var key = await _context.Campains.Where(x=> x.Status).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
			if (key == null) return Guid.Empty;
			return key.Id;
		}
		public async Task<List<Campain>> GetListCampainAutoOff()
		{
			var campain = await _context.Campains.Where(x => x.ViewPerDay > 0 && x.ViewPerHour > 0 && !x.Status).ToListAsync();
			return campain;
		}
		public async Task<Guid> GetCampainIdRandomByOldID(Guid oldId)
		{
			var key = await _context.Campains.Where(x=> x.Id != oldId && x.Status).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();

			if (key == null) return Guid.Empty;
			return key.Id;
		}
		public async Task<string> GetFlatformByCampainId(Guid campainId)
		{
			var campain = await _context.Campains.Where(x => x.Id == campainId).FirstOrDefaultAsync();
			if (campain == null) return string.Empty;
			return campain.Flatform;
		}
		public async Task<string> GetTokenByDomain(string domain)
		{
			var campain = await _context.Campains.Where(x => x.Domain == domain).FirstOrDefaultAsync();
			if (campain == null) return string.Empty;
			return campain.KeyToken;
		}
		public async Task<Campain> GetCampainByID(Guid campainId)
		{
			var campain = await _context.Campains.Where(x => x.Id == campainId && x.Status).FirstOrDefaultAsync();
			if (campain == null) return null;
			return campain;
		}
		public async Task<Campain> GetCampainByIDNotCheckStatus(Guid campainId)
		{
			var campain = await _context.Campains.Where(x => x.Id == campainId).FirstOrDefaultAsync();
			if (campain == null) return null;
			return campain;
		}
		
		public async Task<CampainInListDto> GetCampainByCampainID(Guid campainId)
		{
			var campain = await _context.Campains.Where(x => x.Id == campainId).FirstOrDefaultAsync();
			var campainDto = _mapper.Map<Campain, CampainInListDto>(campain);
			return campainDto;
		}
		public async Task<ThongKeView> GetThongKeView()
        {
            var thongkeview = new ThongKeView();
            var query = _context.Campains.Where(x => x.Status).AsQueryable();
            thongkeview.MaxViewDay = await query.SumAsync(x => x.ViewPerDay);
			thongkeview.ViewedInDay = 0;
            return thongkeview;
        }
		public async Task<PagedResult<CampainInListDto>> GetAllPaging(int pageIndex = 1, int pageSize = 10, string flatform = "google", string? keySearch = "")
		{
			var query = _context.Campains.AsQueryable();
			query = query.Where(x => x.Flatform == flatform);
			if (!String.IsNullOrEmpty(keySearch))
			{
				query = query.Where(x => x.Url.Contains(keySearch));
			}
			var totalRow = await query.CountAsync();

			query = query.OrderByDescending(x => x.DateCreated)
			   .Skip((pageIndex - 1) * pageSize)
			   .Take(pageSize);

			return new PagedResult<CampainInListDto>
			{
				Results = await _mapper.ProjectTo<CampainInListDto>(query).ToListAsync(),
				CurrentPage = pageIndex,
				RowCount = totalRow,
				PageSize = pageSize
			};
		}
		public async Task UpdateActive(Guid id, bool isActive)
		{
            var campain = await _context.Campains.FirstOrDefaultAsync(x => x.Id == id);
			campain.Status = isActive;
            _context.Campains.Update(campain);
        }

    }
}
