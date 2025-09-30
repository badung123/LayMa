using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.User
{
    public class AdminBalanceAdjustmentRequest
    {
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public double Amount { get; set; }
        
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}



