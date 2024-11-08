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
            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return BadRequest("User không tồn tại");
            var transaction = _mapper.Map<CreateBankTransactionDto, TransactionBank>(request);
            transaction.DateCreated = DateTime.Now;
            transaction.DateModified = DateTime.Now;
            transaction.UserId = userId;
            transaction.UserName = user.UserName;
            transaction.StatusProcess = ProcessStatus.WaitingForApproval;
            var id = Guid.NewGuid();
            transaction.Id = id;
            _unitOfWork.BankTransactions.Add(transaction);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
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
        [HttpPost]
        [Route("updateprocess")]
        public async Task<ActionResult<dynamic>> UpdateProcessStatus([FromBody] dynamic input)
        {
            var userId = User.GetUserId();
            int type = input.type;
            Guid id = input.id;
            if (type > 3 || type < 0)
            {
                return BadRequest("Có lỗi xảy ra");
            }
            await _unitOfWork.BankTransactions.UpdateStatusProcess(type, id);
            return Ok();
        }

        //update status
    }
}
