namespace FleetManager.Data.Models
{
    using FleetManager.Core.Common;
    using FleetManager.Core.Extensions;
    using FleetManager.Data.Models;
    using FleetManagerWeb.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public partial class ClsUser : DataContextEntity<UserDataContext>, IClsUser
    {
	  public ClsUser() { }

	  public ClsUser(UserDataContext context) : base(context) { }

	  public bool blIsActive { get; set; }

	  public bool blIsLogin { get; set; }

	  public bool blRememberMe { get; set; }

	  public bool hdniFrame { get; set; }

	  public long lgBranchId { get; set; }

	  public long lgId { get; set; }

	  public long lgRoleId { get; set; }

	  public long lgUserTypeId { get; set; }

	  public long lgVehicleDistributeId { get; set; }

	  public List<SelectListItem> lstBranch { get; set; }

	  public List<SelectListItem> lstRole { get; set; }

	  public List<ClsUser> lstUser { get; set; }

	  public List<SelectListItem> lstUserType { get; set; }

	  public string strAddress { get; set; }

	  public string strBranchName { get; set; }

	  public string strEmailID { get; set; }

	  public string strEmployeeCode { get; set; }

	  public string strFirstName { get; set; }

	  public string strMobileNo { get; set; }

	  public string strPassword { get; set; }

	  public string strRoleName { get; set; }

	  public string strSurName { get; set; }

	  public string strUserName { get; set; }

	  public string strUserTypeName { get; set; }

	  public static bool AppStart()
	  {
		bool bflag = false;
		try
		{
		    int inRowAffected = 0; // objDataContext.UpdateUserIsLogin(0, false);
		    if (inRowAffected > 0)
		    {
			  bflag = true;
		    }
		}
		catch
		{
		}

		return bflag;
	  }

	  public ClsUser ChangePassword(long lgUserId, string strUserPwd)
	  {
		ClsUser objUserMaster = new ClsUser(objDataContext);
		try
		{
		    objUserMaster = GetUserByUserId(lgUserId);
		    if (objUserMaster != null)
		    {
			  objUserMaster.strPassword = strUserPwd;
		    }

		    SaveUser(objUserMaster);
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		}

		return objUserMaster;
	  }

	  public DeleteUserResult DeleteUser(string strUserId, long lgDeletedBy)
	  {
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  DeleteUserResult result = objDataContext.DeleteUser(strUserId, lgDeletedBy, PageMaster.User).FirstOrDefault();
			  if (result == null)
			  {
				result = new DeleteUserResult();
			  }

			  return result;
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		    return null;
		}
	  }

	  public List<SelectListItem> GetAllUserByBranchForDropDown(long lgBranchId)
	  {
		List<SelectListItem> lstUser = new List<SelectListItem>();
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  lstUser.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
			  List<GetUserAllResult> lstUserResult = objDataContext.GetUserAll().ToList();
			  if (lstUserResult != null && lstUserResult.Count > 0)
			  {
				if (_mySession.UserTypeId == 1 || _mySession.UserTypeId == 11)
				{
				    lstUserResult = lstUserResult.ToList();
				}
				else
				{
				    lstUserResult = lstUserResult.Where(x => x.BranchId == lgBranchId).ToList();
				}

				if (lstUserResult != null && lstUserResult.Count > 0)
				{
				    foreach (var item in lstUserResult)
				    {
					  lstUser.Add(new SelectListItem { Text = item.UserName, Value = item.Id.ToString() });
				    }
				}
			  }
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		}

		return lstUser;
	  }

	  public List<SelectListItem> GetAllUserForDropDown()
	  {
		List<SelectListItem> lstUser = new List<SelectListItem>();
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  lstUser.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
			  List<GetUserAllResult> lstUserResult = objDataContext.GetUserAll().ToList();
			  if (lstUserResult.Count > 0)
			  {
				foreach (var item in lstUserResult)
				{
				    lstUser.Add(new SelectListItem { Text = item.UserName, Value = item.Id.ToString() });
				}
			  }
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		}

		return lstUser;
	  }

	  public List<SelectListItem> GetAllUserTypeForDropDown()
	  {
		List<SelectListItem> lstUser = new List<SelectListItem>();
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  lstUser.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
			  List<GetAllUserTypeForDropDownResult> lstUserResult = objDataContext.GetAllUserTypeForDropDown().ToList();
			  if (lstUserResult.Count > 0)
			  {
				foreach (var item in lstUserResult)
				{
				    lstUser.Add(new SelectListItem { Text = item.UserTypeName, Value = item.Id.ToString() });
				}
			  }
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		}

		return lstUser;
	  }

	  public List<GetBranchManagerByBranchIdResult> GetBranchManagerByBranchId(long lgBranchId)
	  {
		try
		{
		    objDataContext = GetDataContext();
		    return objDataContext.GetBranchManagerByBranchId(lgBranchId).ToList();
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		}

		return null;
	  }

	  public List<GetUserAllResult> GetUserAll()
	  {
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  List<GetUserAllResult> lstUserAll = objDataContext.GetUserAll().ToList();
			  return lstUserAll;
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		    return null;
		}
	  }

	  public ClsUser GetUserByBranchAndUserType(long lgBranchId, long lgUserTypeId)
	  {
		ClsUser objClsUser = new ClsUser(objDataContext);
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  var result = objDataContext.User.Where(x => x.BranchId == lgBranchId && x.UserTypeId == lgUserTypeId);
			  if (result != null)
			  {
				foreach (var item in result)
				{
				    objClsUser.lgId = item.Id;
				    objClsUser.strFirstName = item.FirstName;
				    objClsUser.strSurName = item.SurName;
				    objClsUser.strMobileNo = item.MobileNo;
				    objClsUser.strEmailID = item.EmailID;
				    objClsUser.strUserName = item.UserName;
				    objClsUser.strPassword = item.Password.DecryptString();
				    objClsUser.strAddress = item.Address;
				    objClsUser.lgRoleId = item.RoleId;
				    objClsUser.lgBranchId = item.BranchId;
				    objClsUser.lgUserTypeId = item.UserTypeId;
				    objClsUser.blIsActive = item.IsActive;
				    objClsUser.blIsLogin = item.IsLogin;
				    objClsUser.strEmployeeCode = item.EmployeeCode;
				}
			  }
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		}

		return objClsUser;
	  }

	  public ClsUser GetUserByEmailId(string strEmailId)
	  {
		ClsUser objClsUser = new ClsUser();
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  GetUserByEmailIdResult item = objDataContext.GetUserByEmailId(strEmailId).FirstOrDefault();
			  if (item != null)
			  {
				objClsUser.lgId = item.Id;
				objClsUser.strEmailID = item.EmailID;
				objClsUser.strFirstName = item.FirstName;
				objClsUser.strSurName = item.SurName;
				objClsUser.strMobileNo = item.MobileNo;
				objClsUser.strPassword = item.Password;
			  }
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		}

		return objClsUser;
	  }

	  public ClsUser GetUserByUserId(long lgUserId)
	  {
		ClsUser objClsUser = new ClsUser();
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  GetUserByIdResult item = objDataContext.GetUserById(lgUserId).FirstOrDefault();
			  if (item != null)
			  {
				objClsUser.lgId = item.Id;
				objClsUser.strFirstName = item.FirstName;
				objClsUser.strSurName = item.SurName;
				objClsUser.strMobileNo = item.MobileNo;
				objClsUser.strEmailID = item.EmailID;
				objClsUser.strUserName = item.UserName;
				objClsUser.strPassword = item.Password.DecryptString();
				objClsUser.strAddress = item.Address;
				objClsUser.lgRoleId = item.RoleId;
				objClsUser.lgBranchId = item.BranchId;
				objClsUser.strBranchName = item.UserBranchName;
				objClsUser.lgUserTypeId = item.UserTypeId;
				objClsUser.blIsActive = item.IsActive;
				objClsUser.blIsLogin = item.IsLogin;
				objClsUser.strEmployeeCode = item.EmployeeCode;
				objClsUser.lgVehicleDistributeId = item.VehicleDistributeId;
			  }
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		}

		return objClsUser;
	  }

	  public List<GetBranchManagerByBranchIdResult> GetUserIdByUserType()
	  {
		try
		{
		    objDataContext = GetDataContext();
		    var objResult = objDataContext.GetUserIdByUseType("Admin,Management").ToList();
		    List<GetBranchManagerByBranchIdResult> result = new List<GetBranchManagerByBranchIdResult>();
		    foreach (var item in objResult)
		    {
			  result.Add(new GetBranchManagerByBranchIdResult { Id = item.Id, UserName = item.UserName });
		    }

		    return result;
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		}

		return null;
	  }

	  public bool IsUserEmailExists(long lgUserId, string userEmail)
	  {
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  if (objDataContext.User.Where(x => x.Id != lgUserId && x.EmailID == userEmail).Count() > 0)
			  {
				return true;
			  }

			  return false;
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		    return false;
		}
	  }

	  public bool IsUserExists(long lgUserId, string userName)
	  {
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  if (objDataContext.User.Where(x => x.Id != lgUserId && x.UserName == userName).Count() > 0)
			  {
				return true;
			  }

			  return false;
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		    return false;
		}
	  }

	  public long SaveUser(ClsUser objSave)
	  {
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  objSave.strPassword = objSave.strPassword.EncryptString();
			  var result = objDataContext.InsertOrUpdateUser(objSave.lgId, 1, 1, objSave.strFirstName, objSave.strSurName, objSave.strMobileNo, objSave.strEmailID, objSave.strUserName, objSave.strPassword, objSave.strAddress, "EMP/12345", objSave.lgRoleId, objSave.blIsActive, objSave.blIsLogin, _mySession.UserId, PageMaster.User).FirstOrDefault();
			  if (result != null)
			  {
				return result.InsertedId;
			  }
			  else
			  {
				return 0;
			  }
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		    return 0;
		}
	  }

	  public List<SearchUserResult> SearchUser(int inRow, int inPage, string strSearch, string strSort)
	  {
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  return objDataContext.SearchUser(inRow, inPage, strSearch, strSort).ToList();
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		    return null;
		}
	  }

	  public ClsUser ValidateLogin(string strUserName, string strPassword)
	  {
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  User objUser = objDataContext.User.Where(n => n.UserName == strUserName && n.Password == strPassword && n.IsActive == true).FirstOrDefault();
			  if (objUser != null)
			  {
				ClsUser objClsUser = new ClsUser();
				objClsUser.lgId = objUser.Id;
				objClsUser.lgBranchId = objUser.BranchId;
				objClsUser.lgUserTypeId = objUser.UserTypeId;
				objClsUser.strFirstName = objUser.FirstName;
				objClsUser.strSurName = objUser.SurName;
				objClsUser.strMobileNo = objUser.MobileNo;
				objClsUser.strEmailID = objUser.EmailID;
				objClsUser.strUserName = objUser.UserName;
				objClsUser.strPassword = objUser.Password.DecryptString();
				objClsUser.strAddress = objUser.Address;
				objClsUser.lgRoleId = objUser.RoleId;
				objClsUser.blIsActive = objUser.IsActive;
				objClsUser.blIsLogin = objUser.IsLogin;
				objClsUser.strEmployeeCode = objUser.EmployeeCode;
				return objClsUser;
			  }

			  return null;
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
		    return null;
		}
	  }
    }
}