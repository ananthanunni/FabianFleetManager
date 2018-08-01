using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManager.Core.Logging
{
    public class Logger:ILogger
    {
	  public void Write(Exception ex, string strProcedureName, long lgPageId, long lgUserId=1)
	  {
		InsertErrorLog(ex, strProcedureName, lgPageId, lgUserId);
	  }
	  
	  private void InsertErrorLog(Exception ex, string strMethodName, long lgPageId, long lgUserId)
	  {
		try
		{

		}
		catch
		{
		}
	  }
    }
}
