using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Domain.Bank
{
    [Table("TransactionBank")]
    public class TransactionBank
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        [Required]
        [Column(TypeName = "varchar(250)")]
        public required string BankAccountName { get; set; }
        [Required]
        [Column(TypeName = "varchar(250)")]
        public required string BankAccountNumber { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public required string BankName { get; set; }
        public required long Money { get; set; }
        public ProcessStatus StatusProcess { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }


    }
    public enum ProcessStatus
    {
        WaitingForApproval = 0,
        Processing = 1,
        Rejected = 2,
        Accepted = 3
    }
}
