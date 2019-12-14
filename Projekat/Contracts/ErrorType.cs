using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    enum ErrorType : short
    {
        Exception = -2,
        DatabaseName = -1,
        Id = 0,
        Success = 1
    }
}
