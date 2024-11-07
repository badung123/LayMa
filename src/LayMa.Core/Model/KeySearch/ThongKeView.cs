using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.KeySearch
{
    public class ThongKeView
    {
        public int MaxViewDay { get; set; }
        public int ViewedInDay { get; set; }
        public int ViewConLaiTrongNgay
        {
            get
            {
                return MaxViewDay - ViewedInDay;

            }
        }
    }
}
