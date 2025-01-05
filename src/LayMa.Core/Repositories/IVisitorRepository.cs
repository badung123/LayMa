using LayMa.Core.Domain.Mission;
using LayMa.Core.Domain.Visitor;
using LayMa.Core.Interface;
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
	}
}
