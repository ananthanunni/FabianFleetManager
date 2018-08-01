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
	  [Obsolete("This will be removed later during refactoring. Use FleetManager.Core.IConfiguration instead. Singleton instance is available as DI")]
	  private static IConfiguration _instance = null;

	  public Configuration()
	  {
		Initialize();
		_instance = this;
	  }

	  [Obsolete("This will be removed later during refactoring. Use FleetManager.Core.IConfiguration instead. Singleton instance is available as DI")]
	  public static IConfiguration Instance
	  {
		get
		{
		    if (_instance != null) return _instance;

		    return DependencyResolver.Current.GetService<IConfiguration>();
		}
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
