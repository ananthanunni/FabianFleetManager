using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FleetManager.Core.Configuration
{
    public class Configuration : IConfiguration
    {
	  public Configuration()
	  {
		Initialize();
	  }

	  public string ConnectionString { get; private set; }
	  public string CookieName { get; set; }

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

	  private void Initialize()
	  {
#if DEBUG
		ConnectionString = ConfigurationManager.ConnectionStrings["FleetManagerConnectionString"].ToString();
		CookieName = Get<string>("CookieName");
#else
#endif
	  }
    }
}
