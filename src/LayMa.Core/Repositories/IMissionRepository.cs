using LayMa.Core.Domain.Campain;
using LayMa.Core.Domain.Mission;
using LayMa.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Repositories
{
	public interface IMissionRepository : IRepository<Mission, Guid>
	{
	}
}
