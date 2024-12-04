using AutoMapper;
using LayMa.Core.Domain.Campain;
using LayMa.Core.Domain.Mission;
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
	public class MissionRepository : RepositoryBase<Mission, Guid>, IMissionRepository
	{
		private readonly IMapper _mapper;
		public MissionRepository(LayMaContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}
		public async Task<Mission> GetMissionByUserId(Guid userId)
		{
			return await _context.Missions.Where(x => x.UserId == userId && x.IsActive).FirstOrDefaultAsync();
		}
	}
}
