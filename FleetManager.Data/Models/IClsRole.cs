using System.Collections.Generic;
using System.Web.Mvc;

namespace FleetManager.Data.Models
{
    public interface IClsRole
    {
	  DeleteRoleResult DeleteRole(string strRoleIdList, long lgDeletedBy);

	  List<SelectListItem> GetAllRoleForDropDown();

	  ClsRole GetRoleByRoleId(long lgRoleId);

	  bool IsRoleExists(long lgRoleId, string strRoleName);

	  long SaveRole(ClsRole objSave);

	  List<SearchRoleResult> SearchRole(int inRow, int inPage, string strSearch, string strSort);
    }
}