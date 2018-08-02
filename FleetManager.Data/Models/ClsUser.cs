using FleetManager.Core.Common;
using FleetManager.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FleetManager.Data.Models
{
    public partial class ClsUser : DataContextEntity<UserDataContext>, IClsUser
    {
	  public ClsUser() { }

	  public ClsUser(UserDataContext context) : base(context) { }

	  public bool BlIsActive { get; set; }

	  public bool BlIsLogin { get; set; }

	  public bool BlRememberMe { get; set; }

	  public bool HdniFrame { get; set; }

	  public long LgBranchId { get; set; }

	  public long LgId { get; set; }

	  public long LgRoleId { get; set; }

	  public long LgUserTypeId { get; set; }

	  public long LgVehicleDistributeId { get; set; }

	  public List<SelectListItem> LstBranch { get; set; }

	  public List<SelectListItem> LstRole { get; set; }

	  public List<ClsUser> LstUser { get; set; }

	  public List<SelectListItem> LstUserType { get; set; }

	  public string StrAddress { get; set; }

	  public string StrBranchName { get; set; }

	  public string StrEmailID { get; set; }

	  public string StrEmployeeCode { get; set; }

	  public string StrFirstName { get; set; }

	  public string StrMobileNo { get; set; }

	  public string StrPassword { get; set; }

	  public string StrRoleName { get; set; }

	  public string StrSurName { get; set; }

	  public string StrUserName { get; set; }

	  public string StrUserTypeName { get; set; }

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
			  objUserMaster.StrPassword = strUserPwd;
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
			  var result = objDataContext.Users.Where(x => x.BranchId == lgBranchId && x.UserTypeId == lgUserTypeId);
			  if (result != null)
			  {
				foreach (var item in result)
				{
				    objClsUser.LgId = item.Id;
				    objClsUser.StrFirstName = item.FirstName;
				    objClsUser.StrSurName = item.SurName;
				    objClsUser.StrMobileNo = item.MobileNo;
				    objClsUser.StrEmailID = item.EmailID;
				    objClsUser.StrUserName = item.UserName;
				    objClsUser.StrPassword = item.Password.DecryptString();
				    objClsUser.StrAddress = item.Address;
				    objClsUser.LgRoleId = item.RoleId;
				    objClsUser.LgBranchId = item.BranchId;
				    objClsUser.LgUserTypeId = item.UserTypeId;
				    objClsUser.BlIsActive = item.IsActive;
				    objClsUser.BlIsLogin = item.IsLogin;
				    objClsUser.StrEmployeeCode = item.EmployeeCode;
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
				objClsUser.LgId = item.Id;
				objClsUser.StrEmailID = item.EmailID;
				objClsUser.StrFirstName = item.FirstName;
				objClsUser.StrSurName = item.SurName;
				objClsUser.StrMobileNo = item.MobileNo;
				objClsUser.StrPassword = item.Password;
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
				objClsUser.LgId = item.Id;
				objClsUser.StrFirstName = item.FirstName;
				objClsUser.StrSurName = item.SurName;
				objClsUser.StrMobileNo = item.MobileNo;
				objClsUser.StrEmailID = item.EmailID;
				objClsUser.StrUserName = item.UserName;
				objClsUser.StrPassword = item.Password.DecryptString();
				objClsUser.StrAddress = item.Address;
				objClsUser.LgRoleId = item.RoleId;
				objClsUser.LgBranchId = item.BranchId;
				objClsUser.StrBranchName = item.UserBranchName;
				objClsUser.LgUserTypeId = item.UserTypeId;
				objClsUser.BlIsActive = item.IsActive;
				objClsUser.BlIsLogin = item.IsLogin;
				objClsUser.StrEmployeeCode = item.EmployeeCode;
				objClsUser.LgVehicleDistributeId = item.VehicleDistributeId;
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
			  if (objDataContext.Users.Where(x => x.Id != lgUserId && x.EmailID == userEmail).Count() > 0)
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
			  if (objDataContext.Users.Where(x => x.Id != lgUserId && x.UserName == userName).Count() > 0)
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
			  objSave.StrPassword = objSave.StrPassword.EncryptString();
			  var result = objDataContext.InsertOrUpdateUser(objSave.LgId, 1, 1, objSave.StrFirstName, objSave.StrSurName, objSave.StrMobileNo, objSave.StrEmailID, objSave.StrUserName, objSave.StrPassword, objSave.StrAddress, "EMP/12345", objSave.LgRoleId, objSave.BlIsActive, objSave.BlIsLogin, _mySession.UserId, PageMaster.User).FirstOrDefault();
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
			  User objUser = objDataContext.Users.Where(n => n.UserName == strUserName && n.Password == strPassword && n.IsActive == true).FirstOrDefault();
			  if (objUser != null)
			  {
				ClsUser objClsUser = new ClsUser
				{
				    LgId = objUser.Id,
				    LgBranchId = objUser.BranchId,
				    LgUserTypeId = objUser.UserTypeId,
				    StrFirstName = objUser.FirstName,
				    StrSurName = objUser.SurName,
				    StrMobileNo = objUser.MobileNo,
				    StrEmailID = objUser.EmailID,
				    StrUserName = objUser.UserName,
				    StrPassword = objUser.Password.DecryptString(),
				    StrAddress = objUser.Address,
				    LgRoleId = objUser.RoleId,
				    BlIsActive = objUser.IsActive,
				    BlIsLogin = objUser.IsLogin,
				    StrEmployeeCode = objUser.EmployeeCode
				};
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