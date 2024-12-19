using LayMa.Core.Domain.Identity;
using LayMa.Core.Interface;
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
    }
}
