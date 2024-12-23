﻿using LayMa.Core.Domain.Link;
using LayMa.Core.Interface;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Repositories
{
	public interface IShortLinkRepository : IRepository<ShortLink,Guid>
	{
		Task UpdateViewCount(Guid id);
		Task<ShortLink> GetByTokenAsync(string token);
		Task<PagedResult<ShortLinkInListDto>> GetAllPaging(Guid currentUserId, int pageIndex = 1, int pageSize = 10, string? keySearch= "");
        Task<List<ShortLinkInListDto>> GetTopLink(Guid currentUserId,int top);
		Task<List<Guid>> GetListShortLinkIDOfUser(Guid userId);
        Task UpdateOriginOfShortLink(string origin, Guid shortlinkId);
        Task UpdateView(Guid id);
    }
}
