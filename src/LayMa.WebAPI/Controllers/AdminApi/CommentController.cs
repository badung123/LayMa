using AutoMapper;
using LayMa.Core.Domain.Commment;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Interface;
using LayMa.Core.Model;
using LayMa.Core.Model.Bank;
using LayMa.Core.Model.Campain;
using LayMa.Core.Model.Comment;
using LayMa.WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LayMa.WebAPI.Controllers.AdminApi
{
	[Route("api/admin/comment")]
	[ApiController]
	public class CommentController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;
		public CommentController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_userManager = userManager;
		}
		[HttpPost]
		[Route("create")]
		//[Authorize(ShortLinks.Create)]
		public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
		{
			if (request == null) return BadRequest("Lỗi đầu vào");
			var checknoidung = await _unitOfWork.Comments.CheckNoidung(request.Message);
            if (!checknoidung) return BadRequest("Bạn chưa nhập nội dung hoặc nội dung chưa đúng");
			var checkuser = await _unitOfWork.Comments.CheckUser(request.Account);
			if (checkuser) return BadRequest("Tài khoản đã tồn tại,Vuui òng thử lại tài khoản khác");
			var cmt = new Commments
			{
				Id = Guid.NewGuid(),
				Account = request.Account,
				Message = request.Message,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};
			_unitOfWork.Comments.Add(cmt);
			var result = await _unitOfWork.CompleteAsync();
			return result > 0 ? Ok() : BadRequest();
		}
		[HttpGet]
		[Route("pagingComment")]
		public async Task<ActionResult<PagedResult<CommentDto>>> GetCommentPaging(int pageIndex, int pageSize = 5)
		{
			var comments = await _unitOfWork.Comments.GetAllPaging(pageIndex, pageSize);
			return Ok(comments);
		}
	}
}
