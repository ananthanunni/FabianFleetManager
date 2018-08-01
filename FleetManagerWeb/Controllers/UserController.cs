namespace FleetManagerWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManager.Data.Models;
    using FleetManager.Model.Common;
    using FleetManager.Model.Interaction;
    using FleetManager.Service.Auth;
    using FleetManager.Service.Interaction;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Controllers;
    using FleetManagerWeb.Model.Common;
    using FleetManagerWeb.Models;

    public class UserController : BaseController
    {
        /// <summary>   Zero-based index of the cls role. </summary>
        private readonly IClsRole _objiClsRole = null;
	  private readonly IAlertTextProvider _alertTextProvider;
	  private readonly IMySession _mySession;
	  private readonly IPermissionChecker _permissionChecker;

	  /// <summary>   Zero-based index of the cls user. </summary>
	  private readonly IClsUser _objiClsUser = null;

        public UserController(IClsUser objIClsUser, IClsRole objIClsRole, IAlertTextProvider alertTextProvider,IMySession mySession, IPermissionChecker permissionChecker)
        {
            _objiClsUser = objIClsUser;
            _objiClsRole = objIClsRole;
		_alertTextProvider = alertTextProvider;
		_mySession = mySession;
		_permissionChecker = permissionChecker;
	  }

        public void BindDropDownListForUser(ClsUser objClsUser, bool blBindDropDownFromDb)
        {
            try
            {
                if (blBindDropDownFromDb)
                {
                    objClsUser.lstRole = _objiClsRole.GetAllRoleForDropDown().ToList();
                    objClsUser.lstBranch = new List<SelectListItem>();
                    objClsUser.lstUserType = _objiClsUser.GetAllUserTypeForDropDown().ToList();
                }
                else
                {
                    objClsUser.lstRole = new List<SelectListItem>();
                    objClsUser.lstBranch = new List<SelectListItem>();
                    objClsUser.lstUserType = new List<SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
            }
        }

        public ActionResult BindUserGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchUserResult> lstUser = _objiClsUser.SearchUser(rows, page, search, sidx + " " + sord);
                if (lstUser != null)
                {
                    return FillGrid(page, rows, lstUser);
                }
                else
                {
                    return Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
                return Json(string.Empty);
            }
        }

        public JsonResult DeleteUser(string strUserId)
        {
            try
            {
                string[] strUser = strUserId.Split(',');
                strUserId = string.Empty;
                foreach (var item in strUser)
                {
                    strUserId += item.Decode() + ",";
                }

                strUserId = strUserId.Substring(0, strUserId.Length - 1);
                DeleteUserResult result = _objiClsUser.DeleteUser(strUserId, _mySession.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return Json(_alertTextProvider.AlertMessage("User", MessageType.DeleteSuccess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return Json(_alertTextProvider.AlertMessage("User", MessageType.DeletePartial, result.Name));
                }

                return Json(_alertTextProvider.AlertMessage("User", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
                return Json(_alertTextProvider.AlertMessage("User", MessageType.DeleteFail));
            }
        }

        public JsonResult GetUser()
        {
            try
            {
                return Json(_objiClsUser.GetAllUserForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public new ActionResult User()
        {
            try
            {
                GetPagePermissionResult objPermission =_permissionChecker.CheckPagePermission(PageMaster.User);
                if (!objPermission.IsActive)
                {
                    return RedirectToAction("Logout", "Home");
                }

                ClsUser objClsUser = _objiClsUser as ClsUser;
                long lgUserId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsUser.hdniFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgUserId = Request.QueryString.ToString().Decode().LongSafe();
                        objClsUser = _objiClsUser.GetUserByUserId(lgUserId);
                        ViewBag.Password = objClsUser.strPassword;
                    }
                }
                else
                {
                    if (!objPermission.Add_Right)
                    {
                        return RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                #region Menu Access
                bool blUserAccess = true, blRoleAccess = true, blTrackerAccess = true, blCarFleetAccess = true, blFleetMakesAccess = true, blFleetModelsAccess = true, blFleetColorsAccess = true, blTripReasonAccess = true;
                objPermission =_permissionChecker.CheckPagePermission(PageMaster.User);
                if (!objPermission.Add_Right)
                {
                    blUserAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.Role);
                if (!objPermission.Add_Right)
                {
                    blRoleAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.Add_Right)
                {
                    blTrackerAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.CarFleet);
                if (!objPermission.Add_Right)
                {
                    blCarFleetAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.FleetMakes);
                if (!objPermission.Add_Right)
                {
                    blFleetMakesAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.FleetModels);
                if (!objPermission.Add_Right)
                {
                    blFleetModelsAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.FleetColors);
                if (!objPermission.Add_Right)
                {
                    blFleetColorsAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.TripReason);
                if (!objPermission.Add_Right)
                {
                    blTripReasonAccess = false;
                }

                ViewData["UserAccess"] = blUserAccess;
                ViewData["RoleAccess"] = blRoleAccess;
                ViewData["TrackerAccess"] = blTrackerAccess;

                ViewData["CarFleetAccess"] = blCarFleetAccess;
                ViewData["FleetMakesAccess"] = blFleetMakesAccess;
                ViewData["FleetModelsAccess"] = blFleetModelsAccess;
                ViewData["FleetColorsAccess"] = blFleetColorsAccess;
                ViewData["TripReasonAccess"] = blTripReasonAccess;
                #endregion

                BindDropDownListForUser(objClsUser, true);
                return View(objClsUser);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
                return View();
            }
        }

        [HttpPost]
        public new ActionResult User(ClsUser objUser)
        {
            try
            {
                ////bool blEmailFlag = false;
                GetPagePermissionResult objPermission =_permissionChecker.CheckPagePermission(PageMaster.User);
                if (!objPermission.IsActive)
                {
                    return RedirectToAction("Logout", "Home");
                }

                if (objUser.lgId == 0)
                {
                    if (!objPermission.Add_Right)
                    {
                        return RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (!objPermission.Edit_Right)
                    {
                        return RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objUser.hdniFrame)
                {
                    ViewData["iFrame"] = "iFrame";
                }

                bool blExists = _objiClsUser.IsUserExists(objUser.lgId, objUser.strUserName);
                bool blExists1 = _objiClsUser.IsUserEmailExists(objUser.lgId, objUser.strEmailID);
                if (blExists)
                {
                    ViewData["Success"] = "0";
                    ViewData["Message"] = _alertTextProvider.AlertMessage("User", MessageType.AlreadyExists);
                }
                else if (blExists1)
                {
                    ViewData["Success"] = "0";
                    ViewData["Message"] = _alertTextProvider.AlertMessage("Email Address", MessageType.AlreadyExists);
                }
                else
                {
                    string strErrorMsg = ValidateUser(objUser);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        ViewData["Success"] = "0";
                        ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        long resultId = _objiClsUser.SaveUser(objUser);
                        if (resultId > 0)
                        {
                            ViewData["Success"] = "1";
                            ViewData["Message"] = _alertTextProvider.AlertMessage("User", MessageType.Success);
                            BindDropDownListForUser(objUser, false);
                            return View(objUser);
                        }
                        else
                        {
                            ViewData["Success"] = "0";
                            ViewData["Message"] = _alertTextProvider.AlertMessage("User", MessageType.Fail);
                        }
                    }
                }

                BindDropDownListForUser(objUser, true);
                return View(objUser);
            }
            catch (Exception ex)
            {
                ViewData["Success"] = "0";
                ViewData["Message"] = _alertTextProvider.AlertMessage("User", MessageType.Fail);
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
                return View(objUser);
            }
        }

        public ActionResult UserView()
        {
            try
            {
                GetPagePermissionResult objPermission =_permissionChecker.CheckPagePermission(PageMaster.User);
                if (!objPermission.IsActive)
                {
                    return RedirectToAction("Logout", "Home");
                }

                if (!objPermission.View_Right)
                {
                    return RedirectToAction("PermissionRedirectPage", "Home");
                }

                ViewData["blAddRights"] = objPermission.Add_Right;
                ViewData["blEditRights"] = objPermission.Edit_Right;
                ViewData["blDeleteRights"] = objPermission.Delete_Right;
                ViewData["blExportRights"] = objPermission.Export_Right;

                #region Menu Access
                bool blUserAccess = true, blRoleAccess = true, blTrackerAccess = true, blCarFleetAccess = true, blFleetMakesAccess = true, blFleetModelsAccess = true, blFleetColorsAccess = true, blTripReasonAccess = true;
                objPermission =_permissionChecker.CheckPagePermission(PageMaster.User);
                if (!objPermission.Add_Right)
                {
                    blUserAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.Role);
                if (!objPermission.Add_Right)
                {
                    blRoleAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.Add_Right)
                {
                    blTrackerAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.CarFleet);
                if (!objPermission.Add_Right)
                {
                    blCarFleetAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.FleetMakes);
                if (!objPermission.Add_Right)
                {
                    blFleetMakesAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.FleetModels);
                if (!objPermission.Add_Right)
                {
                    blFleetModelsAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.FleetColors);
                if (!objPermission.Add_Right)
                {
                    blFleetColorsAccess = false;
                }

                objPermission =_permissionChecker.CheckPagePermission(PageMaster.TripReason);
                if (!objPermission.Add_Right)
                {
                    blTripReasonAccess = false;
                }

                ViewData["UserAccess"] = blUserAccess;
                ViewData["RoleAccess"] = blRoleAccess;
                ViewData["TrackerAccess"] = blTrackerAccess;

                ViewData["CarFleetAccess"] = blCarFleetAccess;
                ViewData["FleetMakesAccess"] = blFleetMakesAccess;
                ViewData["FleetModelsAccess"] = blFleetModelsAccess;
                ViewData["FleetColorsAccess"] = blFleetColorsAccess;
                ViewData["TripReasonAccess"] = blTripReasonAccess;
                #endregion

                return View();
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
                return View();
            }
        }

        private ActionResult FillGrid(int page, int rows, List<SearchUserResult> lstUser)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstUser != null && lstUser.Count > 0 ? lstUser[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedUserCol = lstUser;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objUser in pagedUserCol
                            select new
                            {
                                id = objUser.Id.ToString().Encode(),
                                EmployeeCode = objUser.EmployeeCode,
                                FirstName = objUser.FirstName,
                                SurName = objUser.SurName,
                                MobileNo = objUser.MobileNo,
                                EmailID = objUser.EmailID,
                                UserName = objUser.UserName,
                                Address = objUser.Address,
                                RoleName = objUser.RoleName,
                                BranchName = objUser.BranchName,
                                UserTypeName = objUser.UserTypeName,
                                IsActive = objUser.IsActive ? "Active" : "Inactive"
                            }).ToArray()
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateUser(ClsUser objUser)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objUser.strFirstName))
                {
                    strErrorMsg += _alertTextProvider.AlertMessage("First Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strSurName))
                {
                    strErrorMsg += _alertTextProvider.AlertMessage("Surname", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strMobileNo))
                {
                    strErrorMsg += _alertTextProvider.AlertMessage("Mobile No", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strEmailID))
                {
                    strErrorMsg += _alertTextProvider.AlertMessage("Email Id", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strUserName))
                {
                    strErrorMsg += _alertTextProvider.AlertMessage("User Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strPassword))
                {
                    strErrorMsg += _alertTextProvider.AlertMessage("Password", MessageType.InputRequired) + "<br/>";
                }

                if (objUser.lgRoleId == 0)
                {
                    strErrorMsg += _alertTextProvider.AlertMessage("Role", MessageType.SelectRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
                return string.Empty;
            }
        }
    }
}