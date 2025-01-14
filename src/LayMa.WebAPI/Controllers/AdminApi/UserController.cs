using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Interface;
using LayMa.Core.Model;
using LayMa.Core.Model.Auth;
using LayMa.Core.Model.User;
using LayMa.WebAPI.Extensions;
using LayMa.WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LayMa.WebAPI.Controllers.AdminApi
{
    [Route("api/admin/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
		private readonly IUnitOfWork _unitOfWork;
		public UserController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
			IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("addVerify")]
        public async Task<ActionResult> VerifyUser([FromBody] VerifyUserRequest request)
        {
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return BadRequest("Tài khoản không tồn tại hoặc hết hạn đăng nhập");
            user.UserTelegram = request.Contact;
            user.Origin = request.Origin;
            user.OriginImage = request.Thumbnail;
            user.VerifyDateTime = DateTime.Now;
            user.IsVerify = false;
            await _userManager.UpdateAsync(user);
            return Ok();
        }
		[HttpPost]
		[Route("adminVerify")]
		public async Task<ActionResult> VerifyUserByAdmin([FromBody] VerifyOrLockUserRequest request)
		{
			var user = await _userManager.FindByIdAsync(request.UserId.ToString());
			if (user == null) return BadRequest("Tài khoản không tồn tại");
			user.IsVerify = true;
			await _userManager.UpdateAsync(user);
			return Ok();
		}
		[HttpPost]
		[Route("adminlockUser")]
		public async Task<ActionResult> LockUserByAdmin([FromBody] VerifyOrLockUserRequest request)
		{
			var user = await _userManager.FindByIdAsync(request.UserId.ToString());
			if (user == null) return BadRequest("Tài khoản không tồn tại");
			user.IsActive = false;
			await _userManager.UpdateAsync(user);
			return Ok();
		}
		[HttpGet]
        [Route("getInfoVerify")]
        public async Task<ActionResult<VerifyUserInfo>> GetInfoVerify()
        {
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return BadRequest("Tài khoản không tồn tại hoặc hết hạn đăng nhập");
            var ob = new VerifyUserInfo()
            {
                Origin = user.Origin,
                Contact = user.UserTelegram,
                IsVerify = user.IsVerify,
                Thumnail = user.OriginImage
            };
            return Ok(ob);
        }
		[HttpGet]
		[Route("getListAgentByUserId")]
		public async Task<ActionResult<PagedResult<AgentListDto>>> GetListAgentByUserId(int pageIndex, int pageSize = 10, string? keySearch = "")
		{
			var userId = User.GetUserId();
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user == null) return BadRequest("Tài khoản không tồn tại hoặc hết hạn đăng nhập");
            var refcode = user.RefCode != null ? user.RefCode : "";

			var listAgent = await _unitOfWork.Users.GetAllPaging(refcode, pageIndex, pageSize, keySearch);

			return Ok(listAgent);
		}
		[HttpGet]
		[Route("GetListUser")]
		public async Task<ActionResult<PagedResult<UserDtoInList>>> GetListUser(int pageIndex, int pageSize = 10, string? keySearch = "", string? isVerify = "")
		{

			var listUser = await _unitOfWork.Users.GetAllUserPaging(pageIndex, pageSize, keySearch, isVerify);

			return Ok(listUser);
		}
	}
}
