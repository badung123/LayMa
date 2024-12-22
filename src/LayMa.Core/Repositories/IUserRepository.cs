using LayMa.Core.Domain.Identity;
using LayMa.Core.Interface;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Model;
using LayMa.Core.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LayMa.Core.Constants.AdminPermissions;

namespace LayMa.Core.Repositories
{
    public interface  IUserRepository : IRepository<AppUser, Guid>
    {
        Task UpdateVerifyUserInfo(VerifyUserRequest request,Guid userId);
		Task<PagedResult<AgentListDto>> GetAllPaging(string refcode, int pageIndex = 1, int pageSize = 10, string? keySearch = "");
        Task<AppUser?> GetUserAgentByRefcode(string refcode);
		Task UpdateBalanceCount(Guid userid,double amount);

	}
}
