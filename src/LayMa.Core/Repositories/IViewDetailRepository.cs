using LayMa.Core.Domain.Link;
using LayMa.Core.Interface;
using LayMa.Core.Model.ShortLink;
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
		Task<int> CountClickByDateRangeAndUserId(DateTime start, DateTime end, Guid userId);
		Task<List<ThongKeClickViewInMonth>> CountClickByDateUserIdInMonth(Guid userId);
		Task<int> CountClickByDateRange(DateTime start, DateTime end);
		Task<int> CountClickByDateRangeAndCampainId(DateTime start, DateTime end, Guid campainId);
		//Task<int> CountClickByDateRangeAndUserId(DateTime start, DateTime end, Guid userId);
		Task<bool> CheckIP(string ip, string screenDevice);
        Task<bool> CheckUserAgent(string usergent, string screenDevice);
		Task<DateTime> GetTimeSuccess(Guid shortLinkId, Guid userId, string screen);
		Task<List<ThongKeViewClickByUser>> GetTopUsersByClicks(DateTime start, DateTime end, int top = 4);
		Task<int> CountIP(string ip);

    }
}
