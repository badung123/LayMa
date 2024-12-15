﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LayMa.Core.Domain.Identity
{
    [Table("AppUsers")]
	[Index(nameof(Agent))]
	public class AppUser : IdentityUser<Guid>
    {
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public double Balance { get; set; }
        public string? UserTelegram { get; set; }
        public string? RefCode { get; set; }
		public string? Agent { get; set; }
	}
}
