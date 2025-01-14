using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Constants
{
	public static class KetoanPermissions
    {
		public static class AdminWithDraw
		{
			[Description("Xem giao dịch rút tiền")]
			public const string View = "Permissions.AdminWithDraw.View";
			[Description("Sửa giao dịch rút tiền")]
			public const string Edit = "Permissions.AdminWithDraw.Edit";
			[Description("Xóa giao dịch rút tiền")]
			public const string Delete = "Permissions.AdminWithDraw.Delete";
		}
	}
}
