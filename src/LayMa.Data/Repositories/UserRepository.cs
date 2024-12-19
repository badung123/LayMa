using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Model.User;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Data.Repositories
{
    public class UserRepository : RepositoryBase<AppUser, Guid>, IUserRepository
    {
        private readonly IMapper _mapper;
        public UserRepository(LayMaContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        public async Task UpdateVerifyUserInfo(VerifyUserRequest request, Guid userId)
        {
            
        }

    }
}
