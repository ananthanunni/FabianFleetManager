namespace FleetManagerWeb.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Transactions;
    using System.Web;
    using Models;

    public static class Functions
    {
        public static readonly string StrdateFormat = "dd/MM/yyyy";
        public static readonly string StrdateTimeFormat = "dd/MM/yyyy hh:mm tt";

        /// <summary>   Name of the cookie. </summary>
        private static readonly string StrcookieName = "kcsremuser";

        /// <summary>   Context for the object data. </summary>
        private static CommonDataContext objDataContext = null;

        public static readonly string StrConnection = System.Configuration.ConfigurationManager.ConnectionStrings["FleetManagerConnectionString"].ToString();

        public static string AlertMessage(string tableName, MessageType msgType, string message = null)
        {
            if (msgType == MessageType.Success)
            {
                return tableName + " Submitted Successfully.";
            }
            else if (msgType == MessageType.Fail)
            {
                return tableName + " Not Submitted Successfully.";
            }
            else if (msgType == MessageType.DeleteSucess)
            {
                return tableName + "(s) Deleted Successfully.";
            }
            else if (msgType == MessageType.DeleteFail)
            {
                return tableName + "(s) Delete Failure.";
            }
            else if (msgType == MessageType.DeletePartial)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    return "Following " + tableName + "(s) Can Not Be Deleted Due To Reference.<br/>" + message;
                }
                else
                {
                    return "Some " + tableName + "(s) Can Not Be Deleted Due To Reference.";
                }
            }
            else if (msgType == MessageType.AlreadyExist)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    return message + " Already Exists.";
                }
                else
                {
                    return tableName + " Already Exists.";
                }
            }
            else if (msgType == MessageType.InputRequired)
            {
                return "Please Enter " + tableName + ".";
            }
            else if (msgType == MessageType.SelectRequired)
            {
                return "Please Select " + tableName + ".";
            }
            else if (msgType == MessageType.CanNotUpdate)
            {
                return tableName + " has been Approved. So Record Can not be Updated.";
            }
            else
            {
                return message;
            }
        }

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

        public static string ConvertDateFormat(string strDate)
        {
            string[] arrDate = null;
            if (strDate.Contains("/"))
            {
                arrDate = strDate.Split('/');
            }
            else if (strDate.Contains("-"))
            {
                arrDate = strDate.Split('-');
            }

            if (arrDate.Length > 2)
            {
                string strMonth = arrDate[1];
                if (strMonth == "1" || strMonth == "01")
                {
                    return arrDate[0] + " JAN " + arrDate[2];
                }
                else if (strMonth == "2" || strMonth == "02")
                {
                    return arrDate[0] + " FEB " + arrDate[2];
                }
                else if (strMonth == "3" || strMonth == "03")
                {
                    return arrDate[0] + " MAR " + arrDate[2];
                }
                else if (strMonth == "4" || strMonth == "04")
                {
                    return arrDate[0] + " APR " + arrDate[2];
                }
                else if (strMonth == "5" || strMonth == "05")
                {
                    return arrDate[0] + " MAY " + arrDate[2];
                }
                else if (strMonth == "6" || strMonth == "06")
                {
                    return arrDate[0] + " JUN " + arrDate[2];
                }
                else if (strMonth == "7" || strMonth == "07")
                {
                    return arrDate[0] + " JUL " + arrDate[2];
                }
                else if (strMonth == "8" || strMonth == "08")
                {
                    return arrDate[0] + " AUG " + arrDate[2];
                }
                else if (strMonth == "9" || strMonth == "09")
                {
                    return arrDate[0] + " SEP " + arrDate[2];
                }
                else if (strMonth == "10")
                {
                    return arrDate[0] + " OCT " + arrDate[2];
                }
                else if (strMonth == "11")
                {
                    return arrDate[0] + " NOV " + arrDate[2];
                }
                else if (strMonth == "12")
                {
                    return arrDate[0] + " DEC " + arrDate[2];
                }
            }

            return strDate;
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

        public static string GetRememberMe(string strKey)
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Cookies[StrcookieName] != null)
                {
                    switch (strKey)
                    {
                        case "password":
                            return string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[StrcookieName].Values["password"]) ? null : System.Web.HttpContext.Current.Request.Cookies[StrcookieName].Values["password"].ToString();
                        case "username":
                            return string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[StrcookieName].Values[strKey]) ? string.Empty : System.Web.HttpContext.Current.Request.Cookies[StrcookieName].Values[strKey].ToString();
                        case "rememberme":
                            return string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies[StrcookieName].Values[strKey]) ? null : System.Web.HttpContext.Current.Request.Cookies[StrcookieName].Values[strKey].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
                throw;
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
                    HttpCookie hcUser = new HttpCookie(StrcookieName);
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
                    if (System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(StrcookieName))
                    {
                        HttpCookie hcAccount = System.Web.HttpContext.Current.Request.Cookies[StrcookieName];
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

        public static void Write(Exception ex, string strProcedureName, long lgPageId, long lgUserId)
        {
            InsertErrorLog(ex, strProcedureName, lgPageId, lgUserId);
        }

        public static void Write(Exception ex, string strprocedureName, long lgPageId)
        {
            InsertErrorLog(ex, strprocedureName, lgPageId, 1);
        }

        private static void InsertErrorLog(Exception ex, string strMethodName, long lgPageId, long lgUserId)
        {
            try
            {
                
            }
            catch
            {
            }
        }
    }
}