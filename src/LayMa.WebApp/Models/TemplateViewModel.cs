namespace LayMa.WebApp.Models
{
	public class TemplateViewModel
	{
		public Guid Id { get; set; }
		public string? Key { get; set; }
		public string? UrlImage { get; set; }
		public string? UrlWeb { get; set; }
		public string? UrlVideo { get; set; }
		public string? Flatfrom { get; set; }
		public string? UrlFacebook { get; set; }
        public Guid CampainId { get; set; }
		public bool IsHetMa { get; set; }
		public string? LinkDuPhong { get; set; }
	}
}
