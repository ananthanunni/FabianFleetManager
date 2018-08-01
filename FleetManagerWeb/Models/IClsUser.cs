namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsUser
    {
        ClsUser ChangePassword(long lgUserId, string strUserPwd);

        DeleteUserResult DeleteUser(string strUserId, long lgDeletedBy);

        List<SelectListItem> GetAllUserByBranchForDropDown(long lgBranchId);

        List<SelectListItem> GetAllUserForDropDown();

        List<SelectListItem> GetAllUserTypeForDropDown();

        List<GetBranchManagerByBranchIdResult> GetBranchManagerByBranchId(long lgBranchId);

        List<GetUserAllResult> GetUserAll();

        ClsUser GetUserByBranchAndUserType(long lgBranchId, long lgUserTypeId);

        ClsUser GetUserByEmailId(string strEmailId);

        ClsUser GetUserByUserId(long lgUserId);

        List<GetBranchManagerByBranchIdResult> GetUserIdByUserType();

        bool IsUserEmailExists(long lgUserId, string userEmail);

        bool IsUserExists(long lgUserId, string userName);

        long SaveUser(ClsUser objSave);

        List<SearchUserResult> SearchUser(int inRow, int inPage, string strSearch, string strSort);

        ClsUser ValidateLogin(string strUserName, string strPassword);
    }
}