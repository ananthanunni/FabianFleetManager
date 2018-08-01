namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public partial class ClsUser : IClsUser
    {
        /// <summary>   Context for the object data. </summary>
        private UserDataContext objDataContext = null;

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
            ClsUser objUserMaster = new ClsUser();
            try
            {
                objUserMaster = this.GetUserByUserId(lgUserId);
                if (objUserMaster != null)
                {
                    objUserMaster.strPassword = strUserPwd;
                }

                this.SaveUser(objUserMaster);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return objUserMaster;
        }

        public DeleteUserResult DeleteUser(string strUserId, long lgDeletedBy)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    DeleteUserResult result = this.objDataContext.DeleteUser(strUserId, lgDeletedBy, PageMaster.User).FirstOrDefault();
                    if (result == null)
                    {
                        result = new DeleteUserResult();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return null;
            }
        }

        public List<SelectListItem> GetAllUserByBranchForDropDown(long lgBranchId)
        {
            List<SelectListItem> lstUser = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    lstUser.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetUserAllResult> lstUserResult = this.objDataContext.GetUserAll().ToList();
                    if (lstUserResult != null && lstUserResult.Count > 0)
                    {
                        if (mySession.Current.UserTypeId == 1 || mySession.Current.UserTypeId == 11)
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return lstUser;
        }

        public List<SelectListItem> GetAllUserForDropDown()
        {
            List<SelectListItem> lstUser = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    lstUser.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetUserAllResult> lstUserResult = this.objDataContext.GetUserAll().ToList();
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return lstUser;
        }

        public List<SelectListItem> GetAllUserTypeForDropDown()
        {
            List<SelectListItem> lstUser = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    lstUser.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetAllUserTypeForDropDownResult> lstUserResult = this.objDataContext.GetAllUserTypeForDropDown().ToList();
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return lstUser;
        }

        public List<GetBranchManagerByBranchIdResult> GetBranchManagerByBranchId(long lgBranchId)
        {
            try
            {
                this.objDataContext = new UserDataContext(Functions.StrConnection);
                return this.objDataContext.GetBranchManagerByBranchId(lgBranchId).ToList();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return null;
        }

        public List<GetUserAllResult> GetUserAll()
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    List<GetUserAllResult> lstUserAll = this.objDataContext.GetUserAll().ToList();
                    return lstUserAll;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return null;
            }
        }

        public ClsUser GetUserByBranchAndUserType(long lgBranchId, long lgUserTypeId)
        {
            ClsUser objClsUser = new ClsUser();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    var result = this.objDataContext.User.Where(x => x.BranchId == lgBranchId && x.UserTypeId == lgUserTypeId);
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return objClsUser;
        }

        public ClsUser GetUserByEmailId(string strEmailId)
        {
            ClsUser objClsUser = new ClsUser();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    GetUserByEmailIdResult item = this.objDataContext.GetUserByEmailId(strEmailId).FirstOrDefault();
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return objClsUser;
        }

        public ClsUser GetUserByUserId(long lgUserId)
        {
            ClsUser objClsUser = new ClsUser();
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    GetUserByIdResult item = this.objDataContext.GetUserById(lgUserId).FirstOrDefault();
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return objClsUser;
        }

        public List<GetBranchManagerByBranchIdResult> GetUserIdByUserType()
        {
            try
            {
                this.objDataContext = new UserDataContext(Functions.StrConnection);
                var objResult = this.objDataContext.GetUserIdByUseType("Admin,Management").ToList();
                List<GetBranchManagerByBranchIdResult> result = new List<GetBranchManagerByBranchIdResult>();
                foreach (var item in objResult)
                {
                    result.Add(new GetBranchManagerByBranchIdResult { Id = item.Id, UserName = item.UserName });
                }

                return result;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
            }

            return null;
        }
        
        public bool IsUserEmailExists(long lgUserId, string userEmail)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    if (this.objDataContext.User.Where(x => x.Id != lgUserId && x.EmailID == userEmail).Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return false;
            }
        }

        public bool IsUserExists(long lgUserId, string userName)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    if (this.objDataContext.User.Where(x => x.Id != lgUserId && x.UserName == userName).Count() > 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return false;
            }
        }

        public long SaveUser(ClsUser objSave)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    objSave.strPassword = objSave.strPassword.EncryptString();
                    var result = this.objDataContext.InsertOrUpdateUser(objSave.lgId, 1, 1, objSave.strFirstName, objSave.strSurName, objSave.strMobileNo, objSave.strEmailID, objSave.strUserName, objSave.strPassword, objSave.strAddress, "EMP/12345", objSave.lgRoleId, objSave.blIsActive, objSave.blIsLogin, mySession.Current.UserId, PageMaster.User).FirstOrDefault();
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return 0;
            }
        }

        public List<SearchUserResult> SearchUser(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    return this.objDataContext.SearchUser(inRow, inPage, strSearch, strSort).ToList();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return null;
            }
        }

        public ClsUser ValidateLogin(string strUserName, string strPassword)
        {
            try
            {
                using (this.objDataContext = new UserDataContext(Functions.StrConnection))
                {
                    User objUser = this.objDataContext.User.Where(n => n.UserName == strUserName && n.Password == strPassword && n.IsActive == true).FirstOrDefault();
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return null;
            }
        }
    }
}