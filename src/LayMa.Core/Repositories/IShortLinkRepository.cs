using LayMa.Core.Domain.Link;
using LayMa.Core.Interface;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayMa.Core.Domain.Transaction;

namespace LayMa.Core.Repositories
{
	public interface IShortLinkRepository : IRepository<ShortLink,Guid>
	{
		Task UpdateViewCount(Guid id);
		Task<ShortLink> GetByTokenAsync(string token);
		Task<PagedResult<ShortLinkInListDto>> GetAllPaging(Guid currentUserId, int pageIndex = 1, int pageSize = 10, string? keySearch= "");
        Task<List<ShortLinkInListDto>> GetTopLink(Guid currentUserId,int top);
		Task<List<Guid>> GetListShortLinkIDOfUser(Guid userId);
        Task UpdateOriginOfShortLink(string origin, Guid shortlinkId,string duphong = "");
        Task UpdateView(Guid id);
		//Task UpdateLockShortLink(Guid id,bool isLock);
		Task<PagedResult<LogShortLinkDto>> GetAllLogPaging(DateTime from, DateTime to,int pageIndex = 1, int pageSize = 10, string? userName = "",int type = -1, string? userAgent = "", string? shortLink = "", string? screen = "", string? ip = "", string? flatform = "",int? solution = 0);
	}
}
