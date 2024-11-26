using LayMa.Core.Domain.Campain;
using LayMa.Core.Interface;
using LayMa.Core.Model.Campain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Repositories
{
	public interface ICampainRepository : IRepository<Campain, Guid>
	{
		Task<CampainDto> GetCampainByKeyToken(string key);

	}
}
