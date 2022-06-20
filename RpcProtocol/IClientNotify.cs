using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpcProtocol
{
    public interface IClientNotify
    {
        void Alert(string message);
    }
}
