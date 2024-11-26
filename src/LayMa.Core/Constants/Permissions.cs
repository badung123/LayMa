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
			[Description("Xem Url")]
			public const string View = "Permissions.WithDraw.View";
			[Description("Tạo Url")]
			public const string Create = "Permissions.WithDraw.Create";
		}
	}
}