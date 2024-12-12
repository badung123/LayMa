using AutoMapper;
using LayMa.Core.Domain.Bank;
using LayMa.Core.Domain.Campain;
using LayMa.Core.Interface;
using LayMa.Core.Model.Campain;
using LayMa.Core.Model.KeySearch;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Data.Repositories
{
    public class CampainRepository : RepositoryBase<Campain, Guid>, ICampainRepository
	{
		private readonly IMapper _mapper;
		public CampainRepository(LayMaContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}
		public async Task<CampainDto> GetCampainByKeyToken(string key)
		{
			var campain = await _context.Campains.Where(x => x.KeyToken == key).FirstOrDefaultAsync();
			var campainDto = _mapper.Map<Campain,CampainDto>(campain);

			return campainDto;
		}
		public async Task<Guid> GetCampainIdRandom()
		{
			var key = await _context.Campains.Where(x=> x.Status).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
			if (key == null) return Guid.Empty;
			return key.Id;
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
		public async Task<Campain> GetCampainByID(Guid campainId)
		{
			var campain = await _context.Campains.Where(x => x.Id == campainId).FirstOrDefaultAsync();
			if (campain == null) return null;
			return campain;
		}
		public async Task<ThongKeView> GetThongKeView()
        {
            var thongkeview = new ThongKeView();
            var query = _context.Campains.Where(x => x.Status).AsQueryable();
            thongkeview.MaxViewDay = await query.SumAsync(x => x.ViewPerDay);
			thongkeview.ViewedInDay = 0;
            return thongkeview;
        }
    }
}
