using System.ComponentModel;

namespace LayMa.Core.Constants
{
	public static class AdminPermissions
	{
		public static class Roles
		{
			[Description("Xem quyền")]
			public const string View = "Permissions.Roles.View";
			[Description("Tạo mới quyền")]
			public const string Create = "Permissions.Roles.Create";
			[Description("Sửa quyền")]
			public const string Edit = "Permissions.Roles.Edit";
			[Description("Xóa quyền")]
			public const string Delete = "Permissions.Roles.Delete";
		}
		public static class Users
		{
			[Description("Xem người dùng")]
			public const string View = "Permissions.Users.View";
			[Description("Tạo người dùng")]
			public const string Create = "Permissions.Users.Create";
			[Description("Sửa người dùng")]
			public const string Edit = "Permissions.Users.Edit";
			[Description("Xóa người dùng")]
			public const string Delete = "Permissions.Users.Delete";
		}
		public static class Campain
		{
			[Description("Xem chiến dịch")]
			public const string View = "Permissions.Campain.View";
			[Description("Tạo mới chiến dịch")]
			public const string Create = "Permissions.Campain.Create";
			[Description("Sửa chiến dịch")]
			public const string Edit = "Permissions.Campain.Edit";
			[Description("Xóa chiến dịch")]
			public const string Delete = "Permissions.Campain.Delete";
		}
		public static class AdminWithDraw
		{
			[Description("Xem giao dịch rút tiền")]
			public const string View = "Permissions.AdminWithDraw.View";
			[Description("Sửa giao dịch rút tiền")]
			public const string Edit = "Permissions.AdminWithDraw.Edit";
			[Description("Xóa giao dịch rút tiền")]
			public const string Delete = "Permissions.AdminWithDraw.Delete";
		}
		public static class Log
		{
			[Description("Xem log")]
			public const string View = "Permissions.Log.View";
		}
	}
}
