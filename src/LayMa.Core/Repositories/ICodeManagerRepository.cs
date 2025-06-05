using LayMa.Core.Domain.Link;
using LayMa.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Repositories
{
	public interface ICodeManagerRepository : IRepository<Code, Guid>
	{
		Task<bool> CheckCode(string code,Guid keyId);
		Task<bool> CheckCodeGoogle(string code, string keyToken);
		Task<int?> UpdateIsUsed(string code, Guid keyId);
		Task<int?> UpdateIsUsedGoogle(string code, string keyToken);
		Task<int?> GetSolution(string code, Guid keyId);
	}
}
