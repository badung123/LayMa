using AutoMapper;
using LayMa.Core.Domain.Bank;
using LayMa.Core.Domain.Campain;
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
			var key = await _context.Campains.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
			if (key == null) return Guid.Empty;
			return key.Id;
		}
		public async Task<string> GetFlatformByCampainId(Guid campainId)
		{
			var campain = await _context.Campains.Where(x => x.Id == campainId).FirstOrDefaultAsync();
			if (campain == null) return string.Empty;
			return campain.Flatform;
		}
	}
}
