using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionCode.Lib
{
    public interface IAccountRepository
    {
        Account GetById(int accountId);
    }
}
