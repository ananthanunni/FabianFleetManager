using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManager.Core.Configuration
{
    public class Configuration : IConfiguration
    {
	  public Configuration()
	  {
#if DEBUG
		ConnectionString = ConfigurationManager.ConnectionStrings["FleetManagerConnectionString"].ToString();
#else
		ConnectionString = ""; // TODO: Use a production connection string
#endif
	  }

	  public string ConnectionString { get; private set; }

	  public bool IsDebug
	  {
		get
		{
#if DEBUG
		    return true;
#else
		    return false;
#endif
		}
	  }

	  public T Get<T>(string key)
	  {
		var value = ConfigurationManager.AppSettings.Get(key);
		return (T)Convert.ChangeType(value, typeof(T));
	  }
    }
}
