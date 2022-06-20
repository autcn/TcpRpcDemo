using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpcProtocol
{
    public interface ITicketService
    {
        bool HasTicket(string trainNo);
    }
}
