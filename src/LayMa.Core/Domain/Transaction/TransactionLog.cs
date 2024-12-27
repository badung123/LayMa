using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Domain.Transaction
{
	[Table("TransactionLog")]
	public class TransactionLog
	{
		[Key]
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public string? UserName { get; set; }
        public long OldBalance { get; set; }
		public long Amount { get; set; }
		public string? Description { get; set; }
		public string? CreatedBy { get; set; }
		public string? DeviceScreen { get; set; }
		public string? UserAgent { get; set; }
		public string? IPAddress { get; set; }
		public string? ShortLink { get; set; }
		public TranSactionType TranSactionType { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateModified { get; set; }
		
	}
	public enum TranSactionType
	{
		Commission = 0,
		Admin = 1,
		ClickCode = 2,
		WithDraw = 3
	}
}
