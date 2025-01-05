using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.Campain
{
    public class TurnOffOrOnCampainRequest
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
    }
}
