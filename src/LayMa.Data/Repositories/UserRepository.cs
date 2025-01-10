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
using LayMa.Core.Domain.Visitor;

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
        public async Task<AppUser?> GetUserByUserToken(string token)
        {
            return await _context.Users.Where(x => x.ApiUserToken == token && x.IsActive).FirstOrDefaultAsync();
        }
        
        public async Task UpdateBalanceCount(Guid userid, double amount)
		{
			var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userid);
			user.Balance += amount;
			_context.Users.Update(user);
		}
		public async Task<List<ThongKeViewClickByUser>> GetAllUser(string? userName = "")
		{
			var query = _context.Users.AsQueryable();
			if (!String.IsNullOrEmpty(userName))
			{
				query = query.Where(x => x.UserName.Contains(userName));
			}
			//query = query.Join(x=> )
			return await _mapper.ProjectTo<ThongKeViewClickByUser>(query).ToListAsync();
		}
        public async Task<PagedResult<ThongKeViewClickByUser>> GetAllUserTHongKe(DateTime from, DateTime to, int pageIndex = 1, int pageSize = 10,string? userName = "")
        {
            var query = _context.Users.AsQueryable();
            if (!String.IsNullOrEmpty(userName))
            {
                query = query.Where(x => x.UserName==userName);
            }
			
			var a = query.GroupJoin(_context.Visitors, u => u.Id, v => v.UserId, (u, v) => new ObjectThongKeJoin { u = u, v = v.Where(x=> x.DateCreated >= from && x.DateCreated < to).ToList() }).Select(x=> new ThongKeViewClickByUser
			{
				Id = x.u.Id,
				UserName = x.u.UserName,
				Click = 0,
				View = x.v.Count()
			}).Where(x=>x.View > 0);
            var totalRow = a.Count();
            a = a.Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PagedResult<ThongKeViewClickByUser>
            {
                Results = await a.ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }
		public class ObjectThongKeJoin()
		{
            public AppUser u { get; set; }
            public List<Visitor> v { get; set; }
        }
    }
}
