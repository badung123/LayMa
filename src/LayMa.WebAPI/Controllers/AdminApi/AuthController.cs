using LayMa.Api.Extensions;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Model.Auth;
using LayMa.WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using LayMa.Core.Model.System;
using LayMa.Core.Constants;
using AutoMapper;
using YamlDotNet.Core.Tokens;
using LayMa.WebAPI.Extensions;
using LayMa.Core.Utilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using LayMa.Core.Domain.Link;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using LayMa.Core.ConfigOptions;
using Microsoft.Extensions.Options;

namespace LayMa.Api.Controllers.AdminApi
{
    [Route("api/admin/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IMapper _mapper;
		private readonly NotiSettings _noti;
		private IMailService _mailService = null;
		public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            RoleManager<IdentityRole<Guid>> roleManager,
            IMapper mapper,
			IMailService mailService,
			IOptions<NotiSettings> noti)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _mapper = mapper;
			_mailService = mailService;
            _noti = noti.Value;
		}

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthenticatedResult>> Login([FromBody] LoginRequest request)
        {
            //Authentication
            if (request == null)
            {
                return BadRequest("Invalid request");
            }
            var isEmail = request.UserName.IsValidEmail();
            var user = new AppUser();
            if (isEmail) {
				user = await _userManager.FindByEmailAsync(request.UserName);
				if (user == null || user.IsActive == false)
				{
					return BadRequest("Email không tồn tại.Vui lòng thử lại");
				}
			}
            else
            {
				user = await _userManager.FindByNameAsync(request.UserName);
				if (user == null || user.IsActive == false)
				{
					return BadRequest("Tên đăng nhập không tồn tại");
				}
			}
            

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, true);
            if (!result.Succeeded)
            {
				return BadRequest("Mật khẩu không đúng.Vui lòng thử lại");
			}

            //Authorization
            var roles = await _userManager.GetRolesAsync(user);
            var permissions = await this.GetPermissionsByUserIdAsync(user.Id.ToString());
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(UserClaims.Id, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Name, user.UserName),
					new Claim(UserClaims.UserName, user.UserName),
					new Claim(UserClaims.Code,user.RefCode == null ? "":user.RefCode),
                    new Claim(UserClaims.Roles, string.Join(";", roles)),
                    new Claim(UserClaims.Permissions, JsonSerializer.Serialize(permissions)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(30);
            await _userManager.UpdateAsync(user);

            return Ok(new AuthenticatedResult()
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }

        private async Task<List<string>> GetPermissionsByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var permissions = new List<string>();

            var allPermissions = new List<RoleClaimsDto>();
            if (roles.Contains(Roles.Admin))
            {
                var types = typeof(AdminPermissions).GetTypeInfo().DeclaredNestedTypes;
                foreach (var type in types)
                {
                    allPermissions.GetPermissions(type);
                }
                permissions.AddRange(allPermissions.Select(x => x.Value));
            }
            else if (roles.Contains(Roles.User))
            {
				var types = typeof(Permissions).GetTypeInfo().DeclaredNestedTypes;
				foreach (var type in types)
				{
					allPermissions.GetPermissions(type);
				}
				permissions.AddRange(allPermissions.Select(x => x.Value));
			}
			else
			{
				var types = typeof(KetoanPermissions).GetTypeInfo().DeclaredNestedTypes;
				foreach (var type in types)
				{
					allPermissions.GetPermissions(type);
				}
				permissions.AddRange(allPermissions.Select(x => x.Value));
			}
			return permissions.Distinct().ToList();
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegisterRequest request)
        {
            var frefix = "layma_";
            //Authentication
            if (request == null)
            {
                var listErr = new List<string>();
                listErr.Add("Invalid request");
                return BadRequest(new RegistrationResponse { Errors = listErr });
            }

            //check User exist
            var userExist = await _userManager.FindByNameAsync(request.UserName);

            if (userExist != null) { 
                var listErr = new List<string>();
                listErr.Add("Tên đăng nhập đã tồn tại");
                return BadRequest(new RegistrationResponse { Errors = listErr }); 
            }
            
                //var user = _mapper.Map<RegisterRequest, AppUser>(request1);

            var passwordHasher = new PasswordHasher<AppUser>();
            var userId = Guid.NewGuid();
            string code = "";
            code = code.GenerateLinkToken(8);
            var md5content = request.UserName + "_" + request.Email;
            var apiUserToken = Helper.MD5(md5content);

			var user = new AppUser()
            {
                Id = userId,
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                UserName = request.UserName,
                Agent = request.Refcode != "" ? request.Refcode : string.Empty,
                RefCode = frefix + code,
				NormalizedUserName = request.UserName.ToUpper(),
                ApiUserToken = apiUserToken,
                IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now,
                MaxClickInDay = 500,
                Rate = 600
            };
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
            var rs = await _userManager.CreateAsync(user, request.ConfirmPassword);


            if (!rs.Succeeded)
            {
                var err = rs.Errors.Select(a => a.Description);
                return BadRequest(new RegistrationResponse { Errors = err });
            }
			var res = await _userManager.AddToRoleAsync(user, "User");
			if (!res.Succeeded)
			{
				var err = res.Errors.Select(a => a.Description);
				return BadRequest(new RegistrationResponse { Errors = err });
			}

			return Ok(new RegistrationResponse
            {
                IsSuccessful = true
            });
        }
		[HttpPost]
		[Route("forgotPassword")]
		public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
		{
			if (request == null) return BadRequest("Dữ liệu truyền lên không đúng");
            if (!request.Email.IsValidEmail()) return BadRequest("Email không đúng định dạng");

			//check User exist
			var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null) return BadRequest("Tài khoản không tồn tại");
            await SendForgotPasswordEmail(request.Email,user);
            //var mailData = new MailData
            //{
            //    EmailToId = request.Email,
            //    EmailToName = "test",
            //    EmailBody = "test1",
            //    EmailSubject = "test2"
            //};
            //_mailService.SendMail(mailData);

			return Ok();
		}
		private async Task SendForgotPasswordEmail(string? email, AppUser? user)
		{
			// Generate the reset password token
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
			// Build the password reset link which must include the Callback URL
			// Build the password reset link
			//var passwordResetLink = Url.Action("ResetPassword", "Account",
			//new { Email = email, Token = token }, protocol: HttpContext.Request.Scheme);
			var passwordResetLink = "https://quanly.layma.net/#/auth/resetPassword?email=" + email + "&token="+token;
			//Send the Confirmation Email to the User Email Id
			await MailUtils.SendMailGoogleSmtp(email, "Làm Mới Mật Khẩu", $"Làm mới mật khẩu của bạn bằng cách nhấn vào <a href='{HtmlEncoder.Default.Encode(passwordResetLink)}'>đường dẫn</a>. ở đây");
		}
		[HttpPost]
		[Route("resetPassword")]
		public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
		{
			if (request == null) return BadRequest("Dữ liệu truyền lên không đúng");

			if (!request.Email.IsValidEmail()) return BadRequest("Email không đúng định dạng");

			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null) return BadRequest("Tài khoản không tồn tại");
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
			var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
			if (!result.Succeeded) return BadRequest("Đổi mật khẩu chưa thành công");

			return Ok();
		}
        [HttpGet]
		[Route("getNoti")]
        public async Task<ActionResult<List<NotiSettings>>> GetNotiSetting()
        {
            var listNoti = new List<NotiSettings>();
            listNoti.Add(_noti);
            return listNoti;
        }
	}
}