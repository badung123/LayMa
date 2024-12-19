using AutoMapper;
using LayMa.Core.Domain.Identity;
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
        public UserController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
    }
}
