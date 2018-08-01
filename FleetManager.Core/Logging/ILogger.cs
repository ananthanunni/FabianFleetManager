using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManager.Core.Logging
{
    public interface ILogger
    {
	  void Write(Exception ex, string strProcedureName, long lgPageId, long lgUserId = 1);
    }
}
