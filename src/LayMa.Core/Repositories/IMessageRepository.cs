using LayMa.Core.Domain.Commment;
using LayMa.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayMa.Core.Repositories
{
	public interface IMessageRepository : IRepository<Messages, Guid>
	{
    }
}
