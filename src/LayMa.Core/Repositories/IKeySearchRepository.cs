using LayMa.Core.Domain.Link;
using LayMa.Core.Interface;
using LayMa.Core.Model.KeySearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Repositories
{
	public interface IKeySearchRepository : IRepository<KeySearch, Guid>
	{
		Task<KeySeoDto?> GetInfoKeySeo();
		Task<Guid?> GetKeySearchIDByKey(string key);
		Task<ThongKeView> GetThongKeView();
	}
}
