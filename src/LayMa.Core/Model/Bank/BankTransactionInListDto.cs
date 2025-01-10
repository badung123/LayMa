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
    public class BankTransactionInListDto
    {
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public string? UserName { get; set; }
		public string? ModifiedBy { get; set; }
		public long Money { get; set; }
        public ProcessStatus StatusProcess { get; set; }
        public required string BankAccountName { get; set; }
        public required string BankAccountNumber { get; set; }
        public required string BankName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public class AutoMapperProfiles : Profile
        {

            public AutoMapperProfiles()
            {
                CreateMap<TransactionBank, BankTransactionInListDto>();
            }
        }
    }
}
