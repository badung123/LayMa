using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Model.KeySearch;
using LayMa.Core.Model.ShortLink;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LayMa.Data.Repositories
{
	public class KeySearchRepository : RepositoryBase<KeySearch, Guid>, IKeySearchRepository
	{
		private readonly IMapper _mapper;
		public KeySearchRepository(LayMaContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}
		public async Task<KeySeoDto?> GetInfoKeySeo()
		{
			var key = await _context.KeySearches.OrderBy(x => Guid.NewGuid()).Select(x => new KeySeoDto
			{
				Id = x.Id,
				Key = x.Key,
				UrlImage = x.UrlImage,
				UrlVideo = x.UrlVideo,
				MaxViewDay = x.MaxViewDay,
				ViewedInDay	= x.ViewedInDay,
				UrlWeb = x.UrlWeb
			}).FirstOrDefaultAsync();
			return key;
		}
		public async Task<Guid?> GetKeySearchIDByKey(string key)
		{
			var rs = await _context.KeySearches.Where(x => x.IsActive && x.Key.ToLower() == key.ToLower()).FirstOrDefaultAsync();
            
			return rs?.Id;
        }
        public async Task<ThongKeView> GetThongKeView()
        {
			var thongkeview = new ThongKeView();
			var query =  _context.KeySearches.Where(x => x.IsActive).AsQueryable();
			thongkeview.MaxViewDay = await query.SumAsync(x => x.MaxViewDay);
            thongkeview.ViewedInDay = await query.SumAsync(x => x.ViewedInDay);
            return thongkeview;
        }
    }
}
