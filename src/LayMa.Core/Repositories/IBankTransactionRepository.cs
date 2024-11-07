using LayMa.Core.Domain.Bank;
using LayMa.Core.Domain.Link;
using LayMa.Core.Interface;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayMa.Core.Model.Bank;

namespace LayMa.Core.Repositories
{
    public interface IBankTransactionRepository : IRepository<TransactionBank, Guid>
    {
        Task<PagedResult<BankTransactionInListDto>> GetAllBankTransactionPaging(Guid currentUserId, int pageIndex = 1, int pageSize = 10, string? keySearch = "");
        Task UpdateStatusProcess(int type, Guid bankTransactionId);
    }
}
