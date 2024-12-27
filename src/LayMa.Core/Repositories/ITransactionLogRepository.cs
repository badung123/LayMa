using LayMa.Core.Domain.Bank;
using LayMa.Core.Domain.Transaction;
using LayMa.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Repositories
{
	public interface ITransactionLogRepository : IRepository<TransactionLog, Guid>
	{
		Task<long> GetHoaHongByDate(Guid userId,DateTime from,DateTime to);
	}
}
