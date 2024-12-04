using AutoMapper;
using LayMa.Core.Domain.Link;
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
	public class CodeManagerRepository : RepositoryBase<Code, Guid>, ICodeManagerRepository
	{
		private readonly IMapper _mapper;
		public CodeManagerRepository(LayMaContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}
		public async Task<bool> CheckCode(string code, Guid keyId)
		{
			var isValid = await _context.Codes.AnyAsync(x => x.CodeString == code && !x.IsUsed && x.CampainId == keyId);
			return isValid;
		}
		public async Task UpdateIsUsed(string code, Guid keyId)
		{
			var codeValue = await _context.Codes.FirstOrDefaultAsync(x => x.CodeString == code && x.CampainId == keyId  && !x.IsUsed);
			if (codeValue == null) return;
            codeValue.IsUsed = true;
			codeValue.DateModified = DateTime.Now;
			_context.Codes.Update(codeValue);
		}
	}
}
