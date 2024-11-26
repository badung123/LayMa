using LayMa.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Interface
{
    public interface IUnitOfWork
    {
		IShortLinkRepository ShortLinks { get; }
		IKeySearchRepository KeySearchs { get; }
		ICodeManagerRepository CodeManagers { get; }
		IViewDetailRepository ViewDetails { get; }
        IBankTransactionRepository BankTransactions { get; }
		ICampainRepository Campains { get; }
		IMissionRepository Missions { get; }
		Task<int> CompleteAsync();
    }
}
