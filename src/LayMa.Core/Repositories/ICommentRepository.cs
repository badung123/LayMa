using LayMa.Core.Domain.Bank;
using LayMa.Core.Interface;
using LayMa.Core.Model.Bank;
using LayMa.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayMa.Core.Domain.Commment;
using LayMa.Core.Model.Campain;
using LayMa.Core.Model.Comment;

namespace LayMa.Core.Repositories
{
	public interface ICommentRepository : IRepository<Commments, Guid>
	{
		Task<PagedResult<CommentDto>> GetAllPaging(int pageIndex = 1, int pageSize = 5);
		Task<string> GetNoidung();
		Task<bool> CheckNoidung(string noidung);
		Task<bool> CheckUser(string account);
	}
}
