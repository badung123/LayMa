using LayMa.Core.Interface;
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

        public UnitOfWork(LayMaContext context)
        {
            _context = context;
        }
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
