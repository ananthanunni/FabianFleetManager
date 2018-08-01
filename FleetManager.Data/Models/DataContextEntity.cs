using FleetManager.Core.Common;
using FleetManager.Core.Logging;
using System.Data.Linq;
using System.Web.Mvc;

namespace FleetManager.Data.Models
{
    public abstract class DataContextEntity<T>
	  where T:DataContext
    {
	  protected T objDataContext = null;
	  protected ILogger _logger;
	  protected IMySession _mySession;

	  public DataContextEntity(T context=null)
	  {
		_logger = DependencyResolver.Current.GetService<ILogger>() ;
		_mySession = DependencyResolver.Current.GetService<IMySession>();
	  }

	  protected T GetDataContext()
	  {
		return DependencyResolver.Current.GetService<T>();
	  }
    }
}