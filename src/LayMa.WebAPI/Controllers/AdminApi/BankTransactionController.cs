using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Interface;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Model;
using LayMa.WebAPI.Extensions;
using LayMa.WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LayMa.Core.Model.Bank;
using LayMa.Core.Domain.Link;
using LayMa.Core.Domain.Bank;
using LayMa.Core.Domain.Transaction;
using static LayMa.Core.Constants.AdminPermissions;
using static LayMa.Core.Constants.Permissions;

namespace LayMa.WebAPI.Controllers.AdminApi
{
    [Route("api/admin/banktransaction")]
    [ApiController]
    public class BankTransactionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IShortLinkService _shortLinkService;
        public BankTransactionController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper, IShortLinkService shortLinkService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _shortLinkService = shortLinkService;
        }
        [HttpPost]
        [Route("create")]
        //[Authorize(ShortLinks.Create)]
        public async Task<IActionResult> CreateBankTransaction([FromBody] CreateBankTransactionDto request)
        {
			//check thoi gian toi thieu tao lenh rut
			if (request == null)
			{
				return BadRequest("Đầu vào không hợp lệ");
			}

			var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return BadRequest("User không tồn tại");
            //check lệnh rút trước cách bao lâu
            if (request.Money < 50000) return BadRequest("Rút tối thiểu 50.000 VNĐ");
			if (user.Balance < request.Money) return BadRequest("Số dư không đủ");
			var transaction = _mapper.Map<CreateBankTransactionDto, TransactionBank>(request);
            transaction.DateCreated = DateTime.Now;
            transaction.DateModified = DateTime.Now;
            transaction.UserId = userId;
            transaction.UserName = user.UserName;
            transaction.StatusProcess = ProcessStatus.WaitingForApproval;
            var id = Guid.NewGuid();
            transaction.Id = id;
            _unitOfWork.BankTransactions.Add(transaction);
			//trừ tiền
			await _unitOfWork.Users.UpdateBalanceCount(user.Id, -request.Money);
			var transLogUser = new TransactionLog
			{
				Id = Guid.NewGuid(),
				UserId = user.Id,
				UserName = user.UserName,
				Amount = -request.Money,
				OldBalance = Int64.Parse(user.Balance.ToString()),
				CreatedBy = "admin",
				Description = "tạo lệnh rút tiền",
				TranSactionType = TranSactionType.WithDraw,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};
			_unitOfWork.TransactionLogs.Add(transLogUser);
			var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest("Có lỗi xảy ra");
        }
        [HttpGet]
        [Route("paging")]
        //[Authorize(ShortLinks.View)]
        public async Task<ActionResult<PagedResult<BankTransactionInListDto>>> GetPostsPaging(int pageIndex, int pageSize = 10, string? keySearch = "")
        {
            var userId = User.GetUserId();
            var result = await _unitOfWork.BankTransactions.GetAllBankTransactionPaging(userId, pageIndex, pageSize, keySearch);
            return Ok(result);
        }
		[HttpGet]
		[Route("allpaging")]
		//[Authorize(ShortLinks.View)]
		public async Task<ActionResult<PagedResult<BankTransactionInListDto>>> GetAllPaging(DateTime from, DateTime to,int pageIndex, int pageSize = 10, string? userName = "", string modifyBy = "All", int type = -1)
		{
			from = from.ToLocalTime().Date;
			to = to.ToLocalTime().Date;
			var result = await _unitOfWork.BankTransactions.GetAllBankTransactionPagingAdmin(from, to, pageIndex, pageSize, userName, modifyBy,type);
			return Ok(result);
		}
		[HttpPost]
        [Route("updateprocess")]
        public async Task<ActionResult<dynamic>> UpdateProcessStatus([FromBody] UpdateStatusRequest input)
        {
			var userAdminName = input.ModifiedBy;
            if (string.IsNullOrEmpty(input.ModifiedBy))
            {
				var userId = User.GetUserId();
				if (userId == Guid.Empty) return BadRequest("Có lỗi xảy ra");
				var userAdmin = await _userManager.FindByIdAsync(userId.ToString());
				userAdminName = userAdmin.UserName;
			}
            
			int type = input.Type;
            Guid id = input.Id;
            if (type > 3 || type < 0) return BadRequest("Có lỗi xảy ra");
            
            
			if (type == 3)
			{
                //get user ID
                var user = await _userManager.FindByIdAsync(input.UserId.ToString());				
				if (user == null) return BadRequest("Tài khoản không tồn tại");
				//await _unitOfWork.Users.UpdateBalanceCount(user.Id, -input.Money);
				var transLogUser = new TransactionLog
				{
					Id = Guid.NewGuid(),
					UserId = input.UserId,
					UserName = user.UserName,
					Amount = input.Money,
					OldBalance = Int64.Parse(user.Balance.ToString()),
					CreatedBy = userAdminName,
					Description = "Rút tiền thành công",
					TranSactionType = TranSactionType.WithDraw,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};
				_unitOfWork.TransactionLogs.Add(transLogUser);
				//update modify users
			}
            if (type == 2)
            {
				var user = await _userManager.FindByIdAsync(input.UserId.ToString());
				if (user == null) return BadRequest("Tài khoản không tồn tại");
				await _unitOfWork.Users.UpdateBalanceCount(user.Id, input.Money);
				var transLogUser = new TransactionLog
				{
					Id = Guid.NewGuid(),
					UserId = input.UserId,
					UserName = user.UserName,
					Amount = input.Money,
					OldBalance = Int64.Parse(user.Balance.ToString()),
					CreatedBy = "admin",
					Description = "Rút tiền bị từ chối",
					TranSactionType = TranSactionType.WithDraw,
					DateCreated = DateTime.Now,
					DateModified = DateTime.Now
				};
				_unitOfWork.TransactionLogs.Add(transLogUser);
			}
            await _unitOfWork.BankTransactions.UpdateStatusProcess(type, id,userAdminName);
			await _unitOfWork.CompleteAsync();
            return Ok();
        }

        //update status
    }
}
