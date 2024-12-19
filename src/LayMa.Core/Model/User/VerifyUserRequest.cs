using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.User
{
    public class VerifyUserRequest
    {
        public required string Origin { get; set; }
        public required string Contact { get; set; }
        public required string Thumbnail { get; set; }
    }
}
