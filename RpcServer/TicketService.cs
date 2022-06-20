using RpcProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpcServer
{
    public class TicketService : ITicketService
    {
        public bool HasTicket(string trainNo)
        {
            if (trainNo == "G21")
            {
                return true;
            }
            if (trainNo == "K508")
            {
                return false;
            }
            throw new Exception("不知道！");
        }
    }
}
