namespace FleetManager.Core.Common
{
    public interface IMySession
    {
	  int BranchId { get; }
	  string Fullname { get; }
	  string Password { get; }
	  string Rememberme { get; }
	  int RoleId { get; }
	  string StrCookiesName { get; }
	  bool UserFirstTimeLogin { get; }
	  int UserId { get; }
	  string UserName { get; }
	  int UserTypeId { get; }
    }
}