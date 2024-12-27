using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Model;
using LayMa.Core.Model.User;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;
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
		public async Task<PagedResult<AgentListDto>> GetAllPaging(string refcode, int pageIndex = 1, int pageSize = 10, string? keySearch = "")
		{
			var query = _context.Users.AsQueryable();
			query = query.Where(x => x.Agent == refcode);
			if (!String.IsNullOrEmpty(keySearch))
			{
				query = query.Where(x => x.UserName.Contains(keySearch));
			}
			var totalRow = await query.CountAsync();

			query = query.OrderByDescending(x => x.DateCreated)
			   .Skip((pageIndex - 1) * pageSize)
			   .Take(pageSize);
			var rs = await query.Select(x=> new AgentListDto
			{
				MemberId = x.Id,
				DateCreated = x.DateCreated,
				IsActive = x.IsActive,
				IsVerify = x.IsVerify,
				UserName = string.IsNullOrEmpty(x.UserName) ? "" : x.UserName
			}).ToListAsync();

			return new PagedResult<AgentListDto>
			{
				Results = rs,
				CurrentPage = pageIndex,
				RowCount = totalRow,
				PageSize = pageSize
			};
		}
		public async Task<PagedResult<UserDtoInList>> GetAllUserPaging(int pageIndex = 1, int pageSize = 10, string? keySearch = "")
		{
			var query = _context.Users.AsQueryable();
			if (!String.IsNullOrEmpty(keySearch))
			{
				query = query.Where(x => x.UserName.Contains(keySearch));
			}
			var totalRow = await query.CountAsync();

			query = query.OrderByDescending(x => x.DateCreated)
			   .Skip((pageIndex - 1) * pageSize)
			   .Take(pageSize);

			return new PagedResult<UserDtoInList>
			{
				Results = await _mapper.ProjectTo<UserDtoInList>(query).ToListAsync(),
				CurrentPage = pageIndex,
				RowCount = totalRow,
				PageSize = pageSize
			};
		}

		public async Task<AppUser?> GetUserAgentByRefcode(string refcode)
		{
			return await _context.Users.Where(x => x.RefCode == refcode && x.IsActive).FirstOrDefaultAsync();
		}
		public async Task UpdateBalanceCount(Guid userid, double amount)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userid);
			user.Balance += amount;
			_context.Users.Update(user);
		}
	}
}
