﻿using LayMa.Core.Domain.Link;
using LayMa.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Repositories
{
	public interface IViewDetailRepository : IRepository<ViewDetail, Guid>
	{
		Task<List<ViewDetail>> GetTopListViewDetail(int top);
		Task<int> CountClickByDateRangeAndShortLink(DateTime start, DateTime end, Guid shortLinkId);

        Task<int> CountClickByDateRange(DateTime start, DateTime end);
		Task<int> CountClickByDateRangeAndCampainId(DateTime start, DateTime end, Guid campainId);
		Task<bool> CheckIP(string ip, string screenDevice);
        Task<bool> CheckUserAgent(string usergent, string screenDevice);
    }
}
