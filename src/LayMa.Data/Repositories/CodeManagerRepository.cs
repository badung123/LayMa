using AutoMapper;
using LayMa.Core.Domain.Link;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
		public async Task<bool> CheckCodeGoogle(string code, string keyToken)
		{
			var listIds = new List<Guid>();
			listIds = await _context.Campains.Where(x=> x.KeyToken == keyToken).Select(x => x.Id).ToListAsync();
			var isValid = await _context.Codes.AnyAsync(x => x.CodeString == code && !x.IsUsed && listIds.Contains(x.CampainId));
			return isValid;
		}
		public async Task<int?> UpdateIsUsed(string code, Guid keyId, Guid shorlinkId)
		{
			var codeValue = await _context.Codes.FirstOrDefaultAsync(x => x.CodeString == code && x.CampainId == keyId  && !x.IsUsed);
			if (codeValue == null) return null;
            codeValue.IsUsed = true;
			codeValue.DateModified = DateTime.Now;
			codeValue.KeySearchId = shorlinkId;
			_context.Codes.Update(codeValue);
			return codeValue.Solution == null ? null : codeValue.Solution.Value;
		}
		public async Task<int?> UpdateIsUsedGoogle(string code, string keyToken, Guid shorlinkId)
		{
			var listIds = new List<Guid>();
			listIds = await _context.Campains.Where(x => x.KeyToken == keyToken).Select(x => x.Id).ToListAsync();
			var codeValue = await _context.Codes.FirstOrDefaultAsync(x => x.CodeString == code && !x.IsUsed && listIds.Contains(x.CampainId));
			if (codeValue == null) return null;
			codeValue.IsUsed = true;
			codeValue.KeySearchId = shorlinkId;
			codeValue.DateModified = DateTime.Now;
			_context.Codes.Update(codeValue);
			return codeValue.Solution == null ? null : codeValue.Solution.Value;
		}
		public async Task<int?> GetSolution(string code, Guid keyId)
		{
			var codeV =  await _context.Codes.FirstOrDefaultAsync(x => x.CodeString == code && x.CampainId == keyId);
			return codeV.Solution;
		}
	}
}
