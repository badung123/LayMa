﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.Auth
{
    public class AuthenticatedResult
    {
		public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
