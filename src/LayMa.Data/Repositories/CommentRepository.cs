using AutoMapper;
using LayMa.Core.Domain.Commment;
using LayMa.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayMa.Data.SeedWorks;
using LayMa.Core.Model.Comment;
using LayMa.Core.Model;
using LayMa.Core.Domain.Link;
using LayMa.Core.Model.Campain;
using Microsoft.EntityFrameworkCore;

namespace LayMa.Data.Repositories
{
	public class CommentRepository : RepositoryBase<Commments, Guid>, ICommentRepository
	{
		private readonly IMapper _mapper;
		public CommentRepository(LayMaContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}
		public async Task<PagedResult<CommentDto>> GetAllPaging(int pageIndex = 1, int pageSize = 5)
		{
			var query = _context.Comments.AsQueryable();
			var totalRow = await query.CountAsync();

			query = query.OrderByDescending(x => x.DateCreated)
			   .Skip((pageIndex - 1) * pageSize)
			   .Take(pageSize);

			return new PagedResult<CommentDto>
			{
				Results = await _mapper.ProjectTo<CommentDto>(query).ToListAsync(),
				CurrentPage = pageIndex,
				RowCount = totalRow,
				PageSize = pageSize
			};
		}
		public async Task<string> GetNoidung()
		{
			var noidung = "";
			var mess = await _context.Messages.Where(x=> !x.IsUsed).FirstOrDefaultAsync();
			if (mess != null) noidung = mess.Message;
			return noidung;
		}
		public async Task<bool> CheckNoidung(string noidung)
		{
			return await _context.Messages.AnyAsync(x => x.IsUsed && x.Message == noidung);
		}
		public async Task<bool> CheckUser(string account)
		{
			return await _context.Comments.AnyAsync(x => x.Account == account);
		}
		
	}
}
