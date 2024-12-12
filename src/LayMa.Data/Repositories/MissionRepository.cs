using AutoMapper;
using LayMa.Core.Domain.Campain;
using LayMa.Core.Domain.Link;
using LayMa.Core.Domain.Mission;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
		public async Task<Mission> GetMissionByUserId(Guid userId,Guid shortlinkID)
		{
			return await _context.Missions.Where(x => x.UserId == userId && x.ShortLinkId == shortlinkID && x.IsActive).FirstOrDefaultAsync();
		}
		public async Task UpdateIsChange(Guid missionId)
		{
			var mission = await _context.Missions.FirstOrDefaultAsync(x => x.Id == missionId && x.IsActive);
			if (mission == null) return;
			mission.IsActive = false;
			mission.DateModified = DateTime.Now;
			_context.Missions.Update(mission);
		}
	}
}
