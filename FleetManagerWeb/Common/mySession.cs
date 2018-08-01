namespace FleetManagerWeb.Common
{
    using System;
    using System.Linq;
    using System.Web;

    public class mySession
    {
        public static mySession Current
        {
            get
            {
                return new mySession();
            }
        }

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
                if (HttpContext.Current.Request.Cookies[this.StrCookiesName] != null)
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["branchid"]))
                    {
                        return HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["branchid"].ToString().intSafe();
                    }
                }

                return 0;
            }
        }

        public string Fullname
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["fullname"]))
                {
                    return HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["fullname"].ToString();
                }

                return string.Empty;
            }
        }

        public string Password
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["password"]))
                {
                    return HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["password"].ToString();
                }

                return string.Empty;
            }
        }

        public string Rememberme
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["rememberme"]))
                {
                    return HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["rememberme"].ToString();
                }

                return string.Empty;
            }
        }

        public int RoleId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["roleid"]))
                {
                    return HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["roleid"].ToString().intSafe();
                }

                return 0;
            }
        }

        public bool UserFirstTimeLogin
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["useronetimelogin"]))
                {
                    return Convert.ToBoolean(HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["useronetimelogin"].ToString());
                }

                return false;
            }
        }

        public int UserId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["userid"]))
                {
                    return HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["userid"].ToString().intSafe();
                }

                return 0;
            }
        }

        public string UserName
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["username"]))
                {
                    return HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["username"].ToString();
                }

                return "Administrator";
            }
        }

        public int UserTypeId
        {
            get
            {
                if (HttpContext.Current.Request.Cookies[this.StrCookiesName] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["usertypeid"]))
                {
                    return HttpContext.Current.Request.Cookies[this.StrCookiesName].Values["usertypeid"].ToString().intSafe();
                }

                return 0;
            }
        }
    }
}