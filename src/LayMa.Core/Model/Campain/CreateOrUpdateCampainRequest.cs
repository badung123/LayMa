﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Model.Campain
{
    public class CreateOrUpdateCampainRequest
    {
		public Guid CampainId { get; set; }
		public string Key { get; set; }
		public string? Domain { get; set; }
		public string? UrlWeb { get; set; }
		public required string Thumbnail { get; set; }
		public int Price { get; set; }
		public int Time { get; set; }
		public int View { get; set; }
		public int ViewPerHour { get; set; }
		public int TypeRun { get; set; }
		public required string Flatform { get; set; }
	}
}
