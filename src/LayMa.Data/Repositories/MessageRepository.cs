using AutoMapper;
using LayMa.Core.Domain.Commment;
using LayMa.Core.Repositories;
using LayMa.Data.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Data.Repositories
{
    public class MessageRepository : RepositoryBase<Messages, Guid>, IMessageRepository
	{
		private readonly IMapper _mapper;
		public MessageRepository(LayMaContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}
	}
}
