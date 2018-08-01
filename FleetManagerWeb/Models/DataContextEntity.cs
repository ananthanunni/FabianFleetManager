using FleetManager.Core.Logging;
using FleetManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FleetManagerWeb.Models
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