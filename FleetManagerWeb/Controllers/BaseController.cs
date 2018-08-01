using FleetManager.Core.Configuration;
using FleetManager.Core.Logging;
using FleetManagerWeb.Common;
using FleetManagerWeb.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FleetManagerWeb.Controllers
{
    public abstract class BaseController : Controller
    {
	  public BaseController()
	  {
		Configuration = DependencyResolver.Current.GetService<IConfiguration>();
		Logger = DependencyResolver.Current.GetService<ILogger>();
	  }

	  public IConfiguration Configuration { get; private set; }
	  public ILogger Logger { get; private set; }

	  protected string GetCookieValue(string strKey, string cookieName = null)
	  {
		if (string.IsNullOrWhiteSpace(cookieName))
		    cookieName = Configuration.CookieName;
		try
		{
		    if (System.Web.HttpContext.Current.Request.Cookies[cookieName] != null)
		    {
			  switch (strKey)
			  {
				case "password":
				    return string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[cookieName].Values["password"]) ? 
					  null : 
					  System.Web.HttpContext.Current.Request.Cookies[cookieName].Values["password"].ToString();

				case "username":
				    return string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[cookieName].Values[strKey]) ? 
					  string.Empty : 
					  System.Web.HttpContext.Current.Request.Cookies[cookieName].Values[strKey].ToString();

				case "rememberme":
				    return string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[cookieName].Values[strKey]) ? 
					  null : 
					  System.Web.HttpContext.Current.Request.Cookies[cookieName].Values[strKey].ToString();
			  }
		    }
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
		    throw;
		}

		return string.Empty;
	  }
    }
}
