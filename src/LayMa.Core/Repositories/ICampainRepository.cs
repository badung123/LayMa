using LayMa.Core.Domain.Campain;
using LayMa.Core.Interface;
using LayMa.Core.Model;
using LayMa.Core.Model.Campain;
using LayMa.Core.Model.KeySearch;
using LayMa.Core.Model.ShortLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Repositories
{
	public interface ICampainRepository : IRepository<Campain, Guid>
	{
		Task<CampainDto> GetCampainByKeyToken(string key,string flatform);
		Task<Guid> GetCampainIdRandom();
		Task<Guid> GetCampainIdRandomByOldID(Guid oldId);
		Task<string> GetFlatformByCampainId(Guid campainId);
		Task<string> GetTokenByDomain(string domain);
		Task<Campain> GetCampainByID(Guid campainId);
		Task<Campain> GetCampainByIDNotCheckStatus(Guid campainId);
		Task<CampainInListDto> GetCampainByCampainID(Guid campainId);
		Task<PagedResult<CampainInListDto>> GetAllPaging(int pageIndex = 1, int pageSize = 10,string flatform = "google", string? keySearch = "");

		Task<ThongKeView> GetThongKeView();
		Task UpdateViewPerDayCount(Guid id, long viewCount);
        Task UpdateActive(Guid id, bool isActive);

    }
}
