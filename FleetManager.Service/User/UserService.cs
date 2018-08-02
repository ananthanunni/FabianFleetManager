using FleetManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FleetManager.Service.User
{
    public class UserService:IUserService
    {
	  private readonly IClsUser _user;
	  private readonly IClsRole _role;

	  public UserService(IClsUser user,IClsRole role)
	  {
		_user = user;
		_role = role;
	  }

	  public DeleteRoleResult DeleteRole(string strRoleId, int userId)
	  {
		return _role.DeleteRole(strRoleId, userId);
	  }

	  public List<SelectListItem> GetAllRoleForDropDown()
	  {
		return _role.GetAllRoleForDropDown();
	  }

	  public IClsRole GetRoleByRoleId(long lgRoleId)
	  {
		return _role.GetRoleByRoleId(lgRoleId);
	  }

	  public bool IsRoleExists(long lgId, string strRoleName)
	  {
		return _role.IsRoleExists(lgId, strRoleName);
	  }

	  public long SaveRole(IClsRole objRole)
	  {
		return _role.SaveRole(objRole);
	  }

	  public List<SearchRoleResult> SearchRole(int inNoOfRows, int v1, string strSearchValue, string v2)
	  {
		return _role.SearchRole(inNoOfRows, v1, strSearchValue, v2);
	  }

	  public List<SearchUserResult> SearchUser(int inNoOfRows, int v1, string strSearchValue, string v2)
	  {
		return _user.SearchUser(inNoOfRows, v1, strSearchValue, v2);
	  }
    }
}
