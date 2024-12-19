using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LayMa.Core.Model.User
{
    public class VerifyUserInfo
    {
        public string Contact { get; set; }
        public string Origin { get; set; }
        public string Thumnail { get; set; }
        public bool IsVerify { get; set; }
        public bool IsWaitingVerify { get { return !IsVerify && Thumnail != null ? true : false; } }
    }
}
