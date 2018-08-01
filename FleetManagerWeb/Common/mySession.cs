namespace FleetManagerWeb.Common
{
    using FleetManager.Model.Common;
    using FleetManagerWeb.Model.Common;
    using System;
    using System.Linq;
    using System.Web;

    public class MySession : IMySession
    {
        public string StrCookiesName
        {
            get
            {
                return "fmsremuser";
            }
        }

        public int BranchId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[StrCookiesName] != null)
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[StrCookiesName].Values["branchid"]))
                    {
                        return HttpContext.Current.Request.Cookies[StrCookiesName].Values["branchid"].ToString().IntSafe();
                    }
                }

                return 0;
            }
        }

        public string Fullname
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[StrCookiesName].Values["fullname"]))
                {
                    return HttpContext.Current.Request.Cookies[StrCookiesName].Values["fullname"].ToString();
                }

                return string.Empty;
            }
        }

        public string Password
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[StrCookiesName].Values["password"]))
                {
                    return HttpContext.Current.Request.Cookies[StrCookiesName].Values["password"].ToString();
                }

                return string.Empty;
            }
        }

        public string Rememberme
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[StrCookiesName].Values["rememberme"]))
                {
                    return HttpContext.Current.Request.Cookies[StrCookiesName].Values["rememberme"].ToString();
                }

                return string.Empty;
            }
        }

        public int RoleId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[StrCookiesName].Values["roleid"]))
                {
                    return HttpContext.Current.Request.Cookies[StrCookiesName].Values["roleid"].ToString().IntSafe();
                }

                return 0;
            }
        }

        public bool UserFirstTimeLogin
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[StrCookiesName].Values["useronetimelogin"]))
                {
                    return Convert.ToBoolean(HttpContext.Current.Request.Cookies[StrCookiesName].Values["useronetimelogin"].ToString());
                }

                return false;
            }
        }

        public int UserId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[StrCookiesName].Values["userid"]))
                {
                    return HttpContext.Current.Request.Cookies[StrCookiesName].Values["userid"].ToString().IntSafe();
                }

                return 0;
            }
        }

        public string UserName
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[StrCookiesName].Values["username"]))
                {
                    return HttpContext.Current.Request.Cookies[StrCookiesName].Values["username"].ToString();
                }

                return "Administrator";
            }
        }

        public int UserTypeId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[StrCookiesName].Values["usertypeid"]))
                {
                    return HttpContext.Current.Request.Cookies[StrCookiesName].Values["usertypeid"].ToString().IntSafe();
                }

                return 0;
            }
        }
    }
}