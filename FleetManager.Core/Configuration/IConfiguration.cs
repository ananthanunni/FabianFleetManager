using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManager.Core.Configuration
{
    public interface IConfiguration
    {
	  T Get<T>(string key);
	  string ConnectionString { get; }
	  string CookieName { get; }
	  bool IsDebug { get; }
    }
}
