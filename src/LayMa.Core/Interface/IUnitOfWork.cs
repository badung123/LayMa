using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Interface
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
    }
}
