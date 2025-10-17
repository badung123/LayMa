using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Transaction;
using LayMa.Core.Interface;
using LayMa.Core.Model;
using LayMa.Core.Model.Auth;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Model.User;
using LayMa.WebAPI.Extensions;
using LayMa.WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
			user.IsActive = !user.IsActive;
			await _userManager.UpdateAsync(user);
			return Ok();
		}
		[HttpPost]
		[Route("updateUserClickRate")]
		public async Task<ActionResult> UpdateUserClickRate([FromBody] UpdateClickRateRequest request)
		{
			var user = await _userManager.FindByIdAsync(request.UserId.ToString());
			if (user == null) return BadRequest("Tài khoản không tồn tại");
			user.MaxClickInDay = request.MaxClickInDay;
			user.Rate = request.Rate;
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
		public async Task<ActionResult<PagedResult<UserDtoInList>>> GetListUser(int pageIndex, int pageSize = 10, string? keySearch = "", int? statusVerify = 0)
		{

			var listUser = await _unitOfWork.Users.GetAllUserPaging(pageIndex, pageSize, keySearch, statusVerify);

			return Ok(listUser);
		}

		[HttpGet]
		[Route("getTopUsersByClicksInWeek")]
		public async Task<ActionResult<List<ThongKeViewClickByUser>>> GetTopUsersByClicksInWeek()
		{
			// Calculate this week's date range (Monday to Sunday)
			var today = DateTime.Now.Date;
			var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
			var endOfWeek = startOfWeek.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);

			var topUsers = await _unitOfWork.ViewDetails.GetTopUsersByClicks(startOfWeek, endOfWeek, 4);
			return Ok(topUsers);
		}
		[HttpGet]
		[Route("getTopUsersByClicksInMonth")]
		public async Task<ActionResult<List<ThongKeViewClickByUser>>> GetTopUsersByClicksInMonth()
		{
			// Calculate this week's date range (Monday to Sunday)
			var today = DateTime.Now.Date;
			var startOfMonth = new DateTime(today.Year, today.Month, 1);
			var endOfMonth = startOfMonth.AddMonths(1).AddSeconds(-1);

			var topUsers = await _unitOfWork.ViewDetails.GetTopUsersByClicks(startOfMonth, endOfMonth, 4);
			return Ok(topUsers);
		}
		[HttpGet]
		[Route("getTopUsersByClicksInLastWeek")]
		public async Task<ActionResult<List<ThongKeViewClickByUser>>> GetTopUsersByClicksInLastWeek()
		{
			// Calculate this week's date range (Monday to Sunday)
			var today = DateTime.Now.Date.AddDays(-7);
			var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
			var endOfWeek = startOfWeek.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);

			var topUsers = await _unitOfWork.ViewDetails.GetTopUsersByClicks(startOfWeek, endOfWeek, 4);
			return Ok(topUsers);
		}
		[HttpGet]
		[Route("getTopUsersByClicksLastMonth")]
		public async Task<ActionResult<List<ThongKeViewClickByUser>>> GetTopUsersByClicksLastMonth()
		{
			// Calculate this week's date range (Monday to Sunday)
			var today = DateTime.Now.Date.AddMonths(-1);
			var startOfMonth = new DateTime(today.Year, today.Month, 1);
			var endOfMonth = startOfMonth.AddMonths(1).AddSeconds(-1);

			var topUsers = await _unitOfWork.ViewDetails.GetTopUsersByClicks(startOfMonth, endOfMonth, 4);
			return Ok(topUsers);
		}

		[HttpPost]
		[Route("congtrutienbyadmin")]
		public async Task<ActionResult> AdjustUserBalanceByAdmin([FromBody] AdminBalanceAdjustmentRequest request)
		{
			// Get the target user
			var targetUser = await _userManager.FindByIdAsync(request.UserId.ToString());
			if (targetUser == null) return BadRequest("Tài khoản không tồn tại");

			// Get admin user info
			var adminUserId = User.GetUserId();
			var adminUser = await _userManager.FindByIdAsync(adminUserId.ToString());
			if (adminUser == null) return BadRequest("Admin không tồn tại hoặc hết hạn đăng nhập");

			// Validate amount
			if (request.Amount == 0) return BadRequest("Số tiền không được bằng 0");

			// Check if user has enough balance for subtraction
			if (request.Amount < 0 && targetUser.Balance < Math.Abs(request.Amount))
			{
				return BadRequest("Số dư không đủ để thực hiện giao dịch");
			}

			// Store old balance for transaction log
			var oldBalance = targetUser.Balance;

			// Update user balance
			await _unitOfWork.Users.UpdateBalanceCount(targetUser.Id, request.Amount);
			await _unitOfWork.CompleteAsync();

			// Create transaction log
			var transactionLog = new TransactionLog
			{
				Id = Guid.NewGuid(),
				UserId = targetUser.Id,
				UserName = targetUser.UserName,
				Amount = (long)request.Amount,
				OldBalance = (long)oldBalance,
				CreatedBy = adminUser.UserName,
				Description = request.Description,
				TranSactionType = TranSactionType.Admin,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};

			_unitOfWork.TransactionLogs.Add(transactionLog);
			var result = await _unitOfWork.CompleteAsync();

			if (result <= 0)
			{
				// Rollback balance change if transaction log fails
				await _unitOfWork.Users.UpdateBalanceCount(targetUser.Id, -request.Amount);
				await _unitOfWork.CompleteAsync();
				return BadRequest("Có lỗi xảy ra khi ghi log giao dịch");
			}

			return Ok(new { 
				message = "Cập nhật số dư thành công",
				oldBalance = oldBalance,
				newBalance = targetUser.Balance + request.Amount,
				amount = request.Amount
			});
		}
	}
}
