using FleetManager.Core.Common;
using FleetManager.Core.Extensions;
using FleetManager.Data.Models;
using FleetManager.Service.Cookie;
using System;
using System.Web;

namespace FleetManager.Service.Auth
{
    public class Authentication : IAuthentication
    {
	  private readonly IMySession _mySession;
	  private readonly IClsUser _clsUserEntity;
	  private readonly ICookieHandler _cookieHandler;

	  public Authentication(IMySession mySession, IClsUser clsUser, ICookieHandler cookieHandler)
	  {
		_mySession = mySession;
		_clsUserEntity = clsUser;
		_cookieHandler = cookieHandler;
	  }

	  public IClsUser CurrentUser { get => _clsUserEntity; }

	  public IClsUser CheckCredentials(string userName, string password)
	  {
		return _clsUserEntity.ValidateLogin(userName, password.EncryptString());
	  }

	  public void LoginUser(string userName, string password, string rememberMe)
	  {
		ClsUser objClsUser = _clsUserEntity.ValidateLogin(userName, password.EncryptString());
		if (objClsUser != null)
		{
		    _cookieHandler.UpdateCookies(objClsUser.strUserName, objClsUser.strPassword.EncryptString(), objClsUser.lgId.ToString(), objClsUser.strFirstName + " " + objClsUser.strSurName, rememberMe, objClsUser.lgRoleId.ToString(), objClsUser.lgBranchId.ToString(), objClsUser.lgUserTypeId.ToString(), "true");

		    objClsUser.blIsLogin = true;
		    _clsUserEntity.SaveUser(objClsUser);
		}
	  }

	  public void LogoutUser()
	  {
		var objClsUser = _clsUserEntity.GetUserByUserId(_mySession.UserId);
		if (objClsUser != null)
		{
		    objClsUser.blIsLogin = false;
		    _clsUserEntity.SaveUser(objClsUser);
		}

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
