namespace FleetManagerWeb.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Transactions;
    using System.Web;
    using FleetManager.Core.Configuration;
    using FleetManager.Model.Interaction;
    using Models;

    [Obsolete("Will be removed after moving functionalities into organized classes.")]
    public static class Functions
    {
        public static readonly string StrdateFormat = "dd/MM/yyyy";
        public static readonly string StrdateTimeFormat = "dd/MM/yyyy hh:mm tt";

        /// <summary>   Context for the object data. </summary>
        private static CommonDataContext objDataContext = null;

        public static readonly string StrConnection = System.Configuration.ConfigurationManager.ConnectionStrings["FleetManagerConnectionString"].ToString();

        public static GetPagePermissionResult CheckPagePermission(long lgPageId)
        {
            try
            {
                using (objDataContext = new CommonDataContext(StrConnection))
                {
                    GetPagePermissionResult objRights = objDataContext.GetPagePermission(lgPageId, mySession.Current.UserId, mySession.Current.RoleId).FirstOrDefault();
                    if (objRights == null)
                    {
                        objRights = new GetPagePermissionResult();
                    }

                    return objRights;
                }
            }
            catch (Exception ex)
            {
                Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
                return new GetPagePermissionResult();
            }
        }
	  
        public static string CreateRootDirectory(string pageName, string dirPath)
        {
            try
            {
                using (objDataContext = new CommonDataContext(StrConnection))
                {
                    AAAAConfigSetting objSetting = objDataContext.AAAAConfigSettings.Where(x => x.KeyName == "DocRootFolderPath").FirstOrDefault();
                    if (objSetting != null)
                    {
                        dirPath = objSetting.KeyValue + dirPath;
                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                    }

                    return dirPath;
                }
            }
            catch (Exception ex)
            {
                Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
                return null;
            }
        }

        public static List<GetPagePermissionResult> GerRolePermissionByRoleId(long lgRoleId)
        {
            try
            {
                using (objDataContext = new CommonDataContext(StrConnection))
                {
                    return objDataContext.GetPagePermission(0, 0, lgRoleId).ToList();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
                return null;
            }
        }

        public static string GetCookieValue(string strKey)
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
                Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
            }

            return string.Empty;
        }
	  
        public static string GetRootDirectory(string pageName)
        {
            try
            {
                using (objDataContext = new CommonDataContext(StrConnection))
                {
                    string strDocPath = objDataContext.AAAAConfigSettings.Where(a => a.KeyName == "DocRootFolderPath").FirstOrDefault().KeyValue.ToString();
                    string strKeyValue = objDataContext.AAAAConfigSettings.Where(a => a.KeyName == pageName).FirstOrDefault().KeyValue.ToString();
                    return strDocPath + strKeyValue;
                }
            }
            catch (Exception ex)
            {
                Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
                return null;
            }
        }

        public static string GetSettings(string keyName)
        {
            try
            {
                using (objDataContext = new CommonDataContext(StrConnection))
                {
                    AAAAConfigSetting objSetting = objDataContext.AAAAConfigSettings.Where(x => x.KeyName == keyName).FirstOrDefault();
                    if (objSetting != null)
                    {
                        return objSetting.KeyValue;
                    }

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
                return string.Empty;
            }
        }

        public static TransactionOptions GetTransactionOptions()
        {
            TransactionOptions option = new TransactionOptions();
            option.IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted;
            option.Timeout = TransactionManager.MaximumTimeout;
            return option;
        }

        public static void LogoutUser()
        {
            HttpCookie hcUser = HttpContext.Current.Request.Cookies[mySession.Current.StrCookiesName];
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

        public static DataTable ReadExcelFile(string strFilePath)
        {
            ////FileStream stream = File.Open(strFilePath, FileMode.Open, FileAccess.Read);
            ////IExcelDataReader excelReader;
            ////string str;
            ////str = Path.GetExtension(strFilePath);
            ////if (str == ".xls")
            ////    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            ////else
            ////{
            ////    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            ////}
            ////excelReader.IsFirstRowAsColumnNames = true;
            ////DataSet result = excelReader.AsDataSet();
            ////excelReader.Close();
            DataTable dt = new DataTable();
            ////if (result != null && result.Tables.Count > 0)
            ////{
            ////    dt = result.Tables[0];
            ////}
            ////else
            ////{

            ////    dt = null;
            ////}
            return dt;
        }

        public static string SetCookieValue(string strKey, string strValue)
        {
            try
            {
                HttpContext.Current.Response.Cookies[strKey].Value = strValue;
                HttpContext.Current.Response.Cookies[strKey].Expires = DateTime.Now.AddMinutes(1); // add expiry time
            }
            catch (Exception ex)
            {
                Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
            }

            return string.Empty;
        }

        public static void SetRememberMe(bool rememberMe, string userName, string password, string strUserId, string strFullName, string strRoleId)
        {
            try
            {
                if (rememberMe)
                {
                    HttpCookie hcUser = new HttpCookie(Configuration.Instance.CookieName);
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
                    if (System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(Configuration.Instance.CookieName))
                    {
                        HttpCookie hcAccount = System.Web.HttpContext.Current.Request.Cookies[Configuration.Instance.CookieName];
                        hcAccount.Expires = DateTime.Now.AddDays(-1);
                        System.Web.HttpContext.Current.Response.Cookies.Add(hcAccount);
                    }
                }
            }
            catch (Exception ex)
            {
                Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
                throw;
            }
        }

        public static void UpdateCookies(string strUserName, string strPassword, string strUserId, string strFullName, string strRemember, string strRoleId, string strBranchId, string strUserTypeId, string strUserOneTimeLogin)
        {
            try
            {
                HttpCookie hcUser = new HttpCookie(mySession.Current.StrCookiesName);
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
                Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
            }
        }
	  
	  public static void Write(Exception ex, string procedureName, long lgPageId, long lgUserId = 1)
	  {
		new FleetManager.Core.Logging.Logger().Write(ex, procedureName, lgPageId, lgUserId);
	  }
    }
}