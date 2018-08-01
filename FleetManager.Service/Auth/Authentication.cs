using FleetManager.Core.Common;
using System;
using System.Web;

namespace FleetManager.Service.Auth
{
    public class Authentication : IAuthentication
    {
	  private readonly IMySession _mySession;

	  public Authentication(IMySession mySession)
	  {
		_mySession = mySession;
	  }

	  public void LogoutUser()
	  {
		HttpCookie hcUser = HttpContext.Current.Request.Cookies[_mySession.StrCookiesName];
		if (hcUser != null)
		{
		    hcUser.Expires = DateTime.Now.AddDays(-1);
		    HttpContext.Current.Response.Cookies.Add(hcUser);
		}

		HttpContext.Current.Session.Clear();
		HttpContext.Current.Session.Contents.RemoveAll();
		HttpContext.Current.Session.Abandon();
		HttpContext.Current.Response.Expires = 60;
		HttpContext.Current.Response.AddHeader("pragma", "no-cache");
		HttpContext.Current.Response.AddHeader("cache-control", "private");
		HttpContext.Current.Response.CacheControl = "no-cache";
	  }
    }
}
