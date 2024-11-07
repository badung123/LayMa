using AutoMapper;
using LayMa.Core.Domain.Link;
using LayMa.Core.Model.Bank;
using LayMa.Core.Model;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayMa.Core.Model.ShortLink;
using Microsoft.EntityFrameworkCore;
using LayMa.Core.Domain.Bank;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LayMa.Data.Repositories
{
    public class BankTransactionRepository : RepositoryBase<TransactionBank, Guid>, IBankTransactionRepository
    {
        private readonly IMapper _mapper;
        public BankTransactionRepository(LayMaContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task<PagedResult<BankTransactionInListDto>> GetAllBankTransactionPaging(Guid currentUserId, int pageIndex = 1, int pageSize = 10, string? keySearch = "")
        {
            var query = _context.TransactionBanks.AsQueryable();
            query = query.Where(x => x.UserId == currentUserId);
            if (!String.IsNullOrEmpty(keySearch))
            {
                query = query.Where(x => x.BankName.Contains(keySearch));
            }
            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PagedResult<BankTransactionInListDto>
            {
                Results = await _mapper.ProjectTo<BankTransactionInListDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }
        public async Task UpdateStatusProcess(int type, Guid bankTransactionId)
        {
            var transaction = await _context.TransactionBanks.FirstOrDefaultAsync(x => x.Id == bankTransactionId);
            if (transaction == null) return;
            transaction.StatusProcess = (ProcessStatus)type;
            transaction.DateModified = DateTime.UtcNow;
            _context.TransactionBanks.Update(transaction);
        }
    }
}
