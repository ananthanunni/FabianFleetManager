namespace FleetManagerWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class UserController : Controller
    {
        /// <summary>   Zero-based index of the cls role. </summary>
        private readonly IClsRole objiClsRole = null;

        /// <summary>   Zero-based index of the cls user. </summary>
        private readonly IClsUser objiClsUser = null;

        public UserController(IClsUser objIClsUser, IClsRole objIClsRole)
        {
            this.objiClsUser = objIClsUser;
            this.objiClsRole = objIClsRole;
        }

        public void BindDropDownListForUser(ClsUser objClsUser, bool blBindDropDownFromDb)
        {
            try
            {
                if (blBindDropDownFromDb)
                {
                    objClsUser.lstRole = this.objiClsRole.GetAllRoleForDropDown().ToList();
                    objClsUser.lstBranch = new List<SelectListItem>();
                    objClsUser.lstUserType = this.objiClsUser.GetAllUserTypeForDropDown().ToList();
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
            }
        }

        public ActionResult BindUserGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchUserResult> lstUser = this.objiClsUser.SearchUser(rows, page, search, sidx + " " + sord);
                if (lstUser != null)
                {
                    return this.FillGrid(page, rows, lstUser);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.Json(string.Empty);
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
                DeleteUserResult result = this.objiClsUser.DeleteUser(strUserId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("User", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("User", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("User", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("User", MessageType.DeleteFail));
            }
        }

        public JsonResult GetUser()
        {
            try
            {
                return this.Json(this.objiClsUser.GetAllUserForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public new ActionResult User()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.User);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ClsUser objClsUser = this.objiClsUser as ClsUser;
                long lgUserId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsUser.hdniFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgUserId = this.Request.QueryString.ToString().Decode().longSafe();
                        objClsUser = this.objiClsUser.GetUserByUserId(lgUserId);
                        this.ViewBag.Password = objClsUser.strPassword;
                    }
                }
                else
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                #region Menu Access
                bool blUserAccess = true, blRoleAccess = true, blTrackerAccess = true, blCarFleetAccess = true, blFleetMakesAccess = true, blFleetModelsAccess = true, blFleetColorsAccess = true, blTripReasonAccess = true;
                objPermission = Functions.CheckPagePermission(PageMaster.User);
                if (!objPermission.Add_Right)
                {
                    blUserAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.Role);
                if (!objPermission.Add_Right)
                {
                    blRoleAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.Add_Right)
                {
                    blTrackerAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.CarFleet);
                if (!objPermission.Add_Right)
                {
                    blCarFleetAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.FleetMakes);
                if (!objPermission.Add_Right)
                {
                    blFleetMakesAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.FleetModels);
                if (!objPermission.Add_Right)
                {
                    blFleetModelsAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.FleetColors);
                if (!objPermission.Add_Right)
                {
                    blFleetColorsAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.TripReason);
                if (!objPermission.Add_Right)
                {
                    blTripReasonAccess = false;
                }

                this.ViewData["UserAccess"] = blUserAccess;
                this.ViewData["RoleAccess"] = blRoleAccess;
                this.ViewData["TrackerAccess"] = blTrackerAccess;

                this.ViewData["CarFleetAccess"] = blCarFleetAccess;
                this.ViewData["FleetMakesAccess"] = blFleetMakesAccess;
                this.ViewData["FleetModelsAccess"] = blFleetModelsAccess;
                this.ViewData["FleetColorsAccess"] = blFleetColorsAccess;
                this.ViewData["TripReasonAccess"] = blTripReasonAccess;
                #endregion

                this.BindDropDownListForUser(objClsUser, true);
                return this.View(objClsUser);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult User(ClsUser objUser)
        {
            try
            {
                ////bool blEmailFlag = false;
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.User);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objUser.lgId == 0)
                {
                    if (!objPermission.Add_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }
                else
                {
                    if (!objPermission.Edit_Right)
                    {
                        return this.RedirectToAction("PermissionRedirectPage", "Home");
                    }
                }

                if (objUser.hdniFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objiClsUser.IsUserExists(objUser.lgId, objUser.strUserName);
                bool blExists1 = this.objiClsUser.IsUserEmailExists(objUser.lgId, objUser.strEmailID);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.AlreadyExist);
                }
                else if (blExists1)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Email Address", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateUser(objUser);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        long resultId = this.objiClsUser.SaveUser(objUser);
                        if (resultId > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.Success);
                            this.BindDropDownListForUser(objUser, false);
                            return this.View(objUser);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.Fail);
                        }
                    }
                }

                this.BindDropDownListForUser(objUser, true);
                return this.View(objUser);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("User", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.View(objUser);
            }
        }

        public ActionResult UserView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.User);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (!objPermission.View_Right)
                {
                    return this.RedirectToAction("PermissionRedirectPage", "Home");
                }

                this.ViewData["blAddRights"] = objPermission.Add_Right;
                this.ViewData["blEditRights"] = objPermission.Edit_Right;
                this.ViewData["blDeleteRights"] = objPermission.Delete_Right;
                this.ViewData["blExportRights"] = objPermission.Export_Right;

                #region Menu Access
                bool blUserAccess = true, blRoleAccess = true, blTrackerAccess = true, blCarFleetAccess = true, blFleetMakesAccess = true, blFleetModelsAccess = true, blFleetColorsAccess = true, blTripReasonAccess = true;
                objPermission = Functions.CheckPagePermission(PageMaster.User);
                if (!objPermission.Add_Right)
                {
                    blUserAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.Role);
                if (!objPermission.Add_Right)
                {
                    blRoleAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.Add_Right)
                {
                    blTrackerAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.CarFleet);
                if (!objPermission.Add_Right)
                {
                    blCarFleetAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.FleetMakes);
                if (!objPermission.Add_Right)
                {
                    blFleetMakesAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.FleetModels);
                if (!objPermission.Add_Right)
                {
                    blFleetModelsAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.FleetColors);
                if (!objPermission.Add_Right)
                {
                    blFleetColorsAccess = false;
                }

                objPermission = Functions.CheckPagePermission(PageMaster.TripReason);
                if (!objPermission.Add_Right)
                {
                    blTripReasonAccess = false;
                }

                this.ViewData["UserAccess"] = blUserAccess;
                this.ViewData["RoleAccess"] = blRoleAccess;
                this.ViewData["TrackerAccess"] = blTrackerAccess;

                this.ViewData["CarFleetAccess"] = blCarFleetAccess;
                this.ViewData["FleetMakesAccess"] = blFleetMakesAccess;
                this.ViewData["FleetModelsAccess"] = blFleetModelsAccess;
                this.ViewData["FleetColorsAccess"] = blFleetColorsAccess;
                this.ViewData["TripReasonAccess"] = blTripReasonAccess;
                #endregion

                return this.View();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return this.View();
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
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateUser(ClsUser objUser)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objUser.strFirstName))
                {
                    strErrorMsg += Functions.AlertMessage("First Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strSurName))
                {
                    strErrorMsg += Functions.AlertMessage("Surname", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strMobileNo))
                {
                    strErrorMsg += Functions.AlertMessage("Mobile No", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strEmailID))
                {
                    strErrorMsg += Functions.AlertMessage("Email Id", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strUserName))
                {
                    strErrorMsg += Functions.AlertMessage("User Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objUser.strPassword))
                {
                    strErrorMsg += Functions.AlertMessage("Password", MessageType.InputRequired) + "<br/>";
                }

                if (objUser.lgRoleId == 0)
                {
                    strErrorMsg += Functions.AlertMessage("Role", MessageType.SelectRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, mySession.Current.UserId);
                return string.Empty;
            }
        }
    }
}