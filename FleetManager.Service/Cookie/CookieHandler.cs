using FleetManager.Core.Configuration;
using FleetManager.Core.Logging;
using FleetManager.Model.Common;
using FleetManagerWeb.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FleetManager.Service.Cookie
{
    public class CookieHandler : ICookieHandler
    {
	  private readonly ILogger _logger;
	  private readonly IMySession _mySession;
	  private readonly IConfiguration _configuration;

	  public CookieHandler(ILogger logger,IMySession mySession,IConfiguration configuration)
	  {
		_logger = logger;
		_mySession = mySession;
		_configuration = configuration;
	  }

	  public string GetCookieValue(string strKey)
	  {
		try
		{
		    if (HttpContext.Current.Request.Cookies[strKey] != null)
		    {
			  return HttpContext.Current.Request.Cookies[strKey].Value.ToString();
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
		}

		return string.Empty;
	  }

	  public string SetCookieValue(string strKey, string strValue)
	  {
		try
		{
		    HttpContext.Current.Response.Cookies[strKey].Value = strValue;
		    HttpContext.Current.Response.Cookies[strKey].Expires = DateTime.Now.AddMinutes(1); // add expiry time
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
		}

		return string.Empty;
	  }

	  public void UpdateCookies(string strUserName, string strPassword, string strUserId, string strFullName, string strRemember, string strRoleId, string strBranchId, string strUserTypeId, string strUserOneTimeLogin)
	  {
		try
		{
		    HttpCookie hcUser = new HttpCookie(_mySession.StrCookiesName);
		    hcUser.HttpOnly = true;
		    hcUser.Values["username"] = strUserName;
		    if (string.IsNullOrEmpty(strPassword))
		    {
			  strPassword = string.Empty;
		    }

		    hcUser.Values["password"] = strPassword;
		    hcUser.Values["userid"] = strUserId;
		    hcUser.Values["fullname"] = strFullName;
		    hcUser.Values["rememberme"] = strRemember;
		    hcUser.Values["roleid"] = strRoleId;
		    hcUser.Values["branchid"] = strBranchId;
		    hcUser.Values["usertypeid"] = strUserTypeId;
		    hcUser.Values["useronetimelogin"] = strUserOneTimeLogin;
		    hcUser.Expires = DateTime.Now.AddDays(1);
		    HttpContext.Current.Response.Cookies.Add(hcUser);
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
		}
	  }

	  public void SetRememberMe(bool rememberMe, string userName, string password, string strUserId, string strFullName, string strRoleId)
	  {
		try
		{
		    if (rememberMe)
		    {
			  HttpCookie hcUser = new HttpCookie(_configuration.CookieName);
			  hcUser.Values["rememberme"] = "true";
			  hcUser.Values["username"] = userName;
			  hcUser.Values["password"] = password;
			  hcUser.Values["userid"] = strUserId;
			  hcUser.Values["fullname"] = strFullName;
			  hcUser.Values["roleid"] = strRoleId;
			  hcUser.Expires = DateTime.Now.AddDays(30);
			  System.Web.HttpContext.Current.Response.Cookies.Add(hcUser);
		    }
		    else
		    {
			  if (System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(_configuration.CookieName))
			  {
				HttpCookie hcAccount = System.Web.HttpContext.Current.Request.Cookies[_configuration.CookieName];
				hcAccount.Expires = DateTime.Now.AddDays(-1);
				System.Web.HttpContext.Current.Response.Cookies.Add(hcAccount);
			  }
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
		    throw;
		}
	  }
    }
}
