using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.ShortLink
{
    public class UpdateNguon
    {
        public Guid ShortlinkId { get; set; }
        public required string Origin { get; set; }
		public string? Duphong { get; set; }
	}
}
