using System.ComponentModel;

namespace LayMa.Core.Constants
{
	public static class Permissions
	{
		public static class Dashboard
		{
			[Description("Xem dashboard")]
			public const string View = "Permissions.Dashboard.View";
		}		
		public static class ShortLinks
		{
			[Description("Xem Url")]
			public const string View = "Permissions.ShortLink.View";
			[Description("Tạo Url")]
			public const string Create = "Permissions.ShortLink.Create";
			[Description("Sửa Url")]
			public const string Edit = "Permissions.ShortLink.Edit";
			[Description("Xóa Url")]
			public const string Delete = "Permissions.ShortLink.Delete";
		}
		public static class WithDraw
		{
			[Description("Xem rút tiền")]
			public const string View = "Permissions.WithDraw.View";
			[Description("Tạo lệnh rút tiền")]
			public const string Create = "Permissions.WithDraw.Create";
		}
		public static class Agent
		{
			[Description("Xem thống kê đại lý")]
			public const string View = "Permissions.Agent.View";
		}
        public static class VerifyAccount
        {
            [Description("Xác minh tài khoản")]
            public const string View = "Permissions.VerifyAccount.Create";
        }
    }
}