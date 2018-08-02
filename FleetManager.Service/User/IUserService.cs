using System.Collections.Generic;
using System.Web.Mvc;
using FleetManager.Data.Models;

namespace FleetManager.Service.User
{
    public interface IUserService
    {
	  List<SearchUserResult> SearchUser(int inNoOfRows, int v1, string strSearchValue, string v2);
	  List<SearchRoleResult> SearchRole(int inNoOfRows, int v1, string strSearchValue, string v2);
	  DeleteRoleResult DeleteRole(string strRoleId, int userId);
	  List<SelectListItem> GetAllRoleForDropDown();
	  IClsRole GetRoleByRoleId(long lgRoleId);
	  bool IsRoleExists(long lgId, string strRoleName);
	  long SaveRole(IClsRole objRole);
    }
}