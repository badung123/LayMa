using AutoMapper;
using LayMa.Core.Domain.Bank;
using LayMa.Core.Model.ShortLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.Bank
{
    public class CreateBankTransactionDto
    {
        public class AutoMapperProfiles : Profile
        {
            public long Money { get; set; }
            public required string BankAccountName { get; set; }
            public required string BankAccountNumber { get; set; }
            public required string BankName { get; set; }
            public AutoMapperProfiles()
            {
                CreateMap<CreateBankTransactionDto, TransactionBank>();
            }
        }
    }
}
