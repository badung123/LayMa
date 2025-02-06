using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using LayMa.Core.ConfigOptions;
using OfficeOpenXml;
using LayMa.Core.Domain.Commment;
using LayMa.Core.Interface;
using LayMa.Data.SeedWorks;

namespace TeduBlog.Api.Controllers.AdminApi
{
    [Route("api/admin/media")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly MediaSettings _settings;
		private readonly IUnitOfWork _unitOfWork;

		public MediaController(IWebHostEnvironment env, IOptions<MediaSettings> settings, IUnitOfWork unitOfWork)
        {
            _hostingEnv = env;
            _settings = settings.Value;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult UploadImage(string type)
        {
            var allowImageTypes = _settings.AllowImageFileTypes?.Split(",");

            var now = DateTime.Now;
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return null;
            }

            var file = files[0];
            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition)?.FileName?.Trim('"');
            if (allowImageTypes?.Any(x => filename?.EndsWith(x, StringComparison.OrdinalIgnoreCase) == true) == false)
            {
                throw new Exception("Không cho phép tải lên file không phải ảnh.");
            }

            var imageFolder = $@"\{_settings.ImageFolder}\images\{type}\{now:MMyyyy}";

            var folder = _hostingEnv.WebRootPath + imageFolder;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var filePath = Path.Combine(folder, filename);
            using (var fs = global::System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            var path = Path.Combine(imageFolder, filename).Replace(@"\", @"/");
            return Ok(new { path });
        }
		[HttpPost]
		[Route("uploadExcel")]
		[AllowAnonymous]
		public async Task<IActionResult> UploadExcel(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return Content("File Not Selected");

			string fileExtension = Path.GetExtension(file.FileName);
			if (fileExtension != ".xls" && fileExtension != ".xlsx")
				return Content("File Not Selected");

			var rootFolder = @"D:\Files";
			var fileName = file.FileName;
			var filePath = Path.Combine(rootFolder, fileName);
			var fileLocation = new FileInfo(filePath);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}

			if (file.Length <= 0)
				return BadRequest("FileNotFound");
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using (ExcelPackage package = new ExcelPackage(fileLocation))
			{
				ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
				//var workSheet = package.Workbook.Worksheets.First();
				int totalRows = workSheet.Dimension.Rows;

                for (int i = 1; i <= totalRows; i++)
                {
					_unitOfWork.Messages.Add(new Messages
					{
						Id = Guid.NewGuid(),
						Message = workSheet.Cells[i, 1].Value.ToString(),
						Account = workSheet.Cells[i, 2].Value == null ? ("layma_" + i) : workSheet.Cells[i, 2].Value.ToString(),
						IsUsed = false,
						DateUsed = null
					});
                    if (i%100 == 0 || i == totalRows) await _unitOfWork.CompleteAsync();
                }                               
            }
			return Ok();
		}
	}
}