using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.ShortLink
{
    public class ViewDatailGroup
    {
        public Guid ShortLinkId { get; set; }
        public Guid CampainId { get; set; }
        public string? IPAddress { get; set; }
        public string? DeviceScreen { get; set; }
    }
}
