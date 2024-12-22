using AutoMapper;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Interface;
using LayMa.Core.Repositories;
using LayMa.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LayMaContext _context;

        public UnitOfWork(LayMaContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            ShortLinks = new ShortLinkRepository(context,mapper,userManager);
            KeySearchs = new KeySearchRepository(context, mapper);
            CodeManagers = new CodeManagerRepository(context, mapper);
            ViewDetails = new ViewDetailRepository(context, mapper);
            BankTransactions = new BankTransactionRepository(context, mapper);
            Campains = new CampainRepository(context, mapper);
            Missions = new MissionRepository(context, mapper);
            Users = new UserRepository(context, mapper);
            Visitors = new VisitorRepository(context, mapper);
            TransactionLogs = new TransactionLogRepository(context, mapper);
		}
        public IShortLinkRepository ShortLinks { get; private set; }
		public IKeySearchRepository KeySearchs { get; private set; }
		public ICodeManagerRepository CodeManagers { get; private set; }
		public IViewDetailRepository ViewDetails { get; private set; }
        public IBankTransactionRepository BankTransactions { get; private set; }
		public ICampainRepository Campains { get; private set; }
		public IMissionRepository Missions { get; private set; }
        public IUserRepository Users { get; private set; }
		public IVisitorRepository Visitors { get; private set; }
		public ITransactionLogRepository TransactionLogs { get; private set; }
		public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
