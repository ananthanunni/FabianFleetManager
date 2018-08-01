using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace FleetManagerWeb.Models
{
    public abstract class DataContextEntity<T>
	  where T:DataContext
    {
	  protected T objDataContext = null;
	  public DataContextEntity(T context=null)
	  {
		objDataContext = context;
	  }
    }
}