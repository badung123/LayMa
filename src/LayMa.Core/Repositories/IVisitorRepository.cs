using LayMa.Core.Domain.Mission;
using LayMa.Core.Domain.Visitor;
using LayMa.Core.Interface;
using LayMa.Core.Model.ShortLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Repositories
{
	public  interface IVisitorRepository : IRepository<Visitor, Guid>
	{
		Task<int> CountViewByDateRangeAndShortLink(DateTime from, DateTime to, Guid shortLinkId);
		Task<int> CountViewByDateRangeAndUserId(DateTime from, DateTime to, Guid userId);
		Task<List<ThongKeClickViewInMonth>> CountViewByDateUserIdInMonth(Guid userId);
		Task<DateTime> GetStartTimeOfShortLink(Guid userId,Guid shortLinkId);
		Task<List<DateTime>> GetListTimeVisitShortLink(Guid shortLinkId, Guid userId, string screen);
	}
}
