using AutoMapper;
using LayMa.Core.Domain.Bank;
using LayMa.Core.Domain.Transaction;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Data.Repositories
{
	public class TransactionLogRepository : RepositoryBase<TransactionLog, Guid>, ITransactionLogRepository
	{
		private readonly IMapper _mapper;
		public TransactionLogRepository(LayMaContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}
		public async Task<long> GetHoaHongByDate(Guid userId, DateTime from, DateTime to)
		{
			return await _context.TransactionLogs.Where(x => x.UserId == userId && from <= x.DateCreated && to >= x.DateCreated && x.TranSactionType == TranSactionType.Commission).SumAsync(x => x.Amount);
		}
	}
}
