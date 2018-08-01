using FleetManager.Data.Models;

namespace FleetManager.Service.Auth
{
    public interface IAuthentication
    {
	  IClsUser CurrentUser { get; }
	  IClsUser CheckCredentials(string userName, string password);
	  void LoginUser(string userName, string password,string rememberMe);
	  void LogoutUser();
    }
}