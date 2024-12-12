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
        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            RoleManager<IdentityRole<Guid>> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _mapper = mapper;
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

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null || user.IsActive == false)
            {
                return Unauthorized();
            }

            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);
            if (!result.Succeeded)
            {
                return Unauthorized();
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
            else
            {
				var types = typeof(Permissions).GetTypeInfo().DeclaredNestedTypes;
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
            var user = new AppUser()
            {
                Id = userId,
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpper(),
                UserName = request.UserName,
                NormalizedUserName = request.UserName.ToUpper(),
                IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                DateCreated = DateTime.Now
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
    }
}