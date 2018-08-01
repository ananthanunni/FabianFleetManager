namespace FleetManager.Service.Cookie
{
    public interface ICookieHandler
    {
	  void SetRememberMe(bool rememberMe, string userName, string password, string strUserId, string strFullName, string strRoleId);
	  string SetCookieValue(string strKey, string strValue);
	  void UpdateCookies(string strUserName, string strPassword, string strUserId, string strFullName, string strRemember, string strRoleId, string strBranchId, string strUserTypeId, string strUserOneTimeLogin);
	  string GetCookieValue(string strKey);
    }
}