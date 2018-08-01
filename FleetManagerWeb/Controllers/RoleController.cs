namespace FleetManagerWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManager.Model.Interaction;
    using FleetManager.Service.Interaction;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Controllers;
    using FleetManagerWeb.Models;

    public class RoleController : BaseController
    {
        /// <summary>   Zero-based index of the cls role. </summary>
        private readonly IClsRole _objiClsRole = null;
	  private readonly IAlertTextProvider _alertTextProvider;

	  public RoleController(IClsRole objIClsRole,IAlertTextProvider alertTextProvider)
        {
            _objiClsRole = objIClsRole;
		_alertTextProvider = alertTextProvider;
	  }

        public ActionResult BindRoleGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchRoleResult> lstRole = _objiClsRole.SearchRole(rows, page, search, sidx + " " + sord);
                if (lstRole != null)
                {
                    return FillGrid(page, rows, lstRole);
                }
                else
                {
                    return Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return Json(string.Empty);
            }
        }

        public ActionResult BindRolePermissionGrid(string sidx, string sord, int page, int rows, string filters, long lgRoleId)
        {
            try
            {
                List<GetPagePermissionResult> lstRolePermission = Functions.GerRolePermissionByRoleId(lgRoleId);
                if (lstRolePermission != null)
                {
                    return FillRollPermissionGrid(lstRolePermission);
                }
                else
                {
                    return Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteRole(string strRoleId)
        {
            try
            {
                string[] strRole = strRoleId.Split(',');
                strRoleId = string.Empty;
                foreach (var item in strRole)
                {
                    strRoleId += item.Decode() + ",";
                }

                strRoleId = strRoleId.Substring(0, strRoleId.Length - 1);
                DeleteRoleResult result = _objiClsRole.DeleteRole(strRoleId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return Json(_alertTextProvider.AlertMessage("Role", MessageType.DeleteSuccess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return Json(_alertTextProvider.AlertMessage("Role", MessageType.DeletePartial, result.Name));
                }

                return Json(_alertTextProvider.AlertMessage("Role", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return Json(_alertTextProvider.AlertMessage("Role", MessageType.DeleteFail));
            }
        }

        public JsonResult GetRole()
        {
            try
            {
                return Json(_objiClsRole.GetAllRoleForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Role()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Role);
                if (!objPermission.IsActive)
                {
                    return RedirectToAction("Logout", "Home");
                }

                ClsRole objClsRole = _objiClsRole as ClsRole;
                long lgRoleId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsRole.hdniFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgRoleId = Request.QueryString.ToString().Decode().longSafe();
                        objClsRole = _objiClsRole.GetRoleByRoleId(lgRoleId);
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

                ViewData["UserAccess"] = blUserAccess;
                ViewData["RoleAccess"] = blRoleAccess;
                ViewData["TrackerAccess"] = blTrackerAccess;

                ViewData["CarFleetAccess"] = blCarFleetAccess;
                ViewData["FleetMakesAccess"] = blFleetMakesAccess;
                ViewData["FleetModelsAccess"] = blFleetModelsAccess;
                ViewData["FleetColorsAccess"] = blFleetColorsAccess;
                ViewData["TripReasonAccess"] = blTripReasonAccess;
                #endregion

                return View(objClsRole);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Role(ClsRole objRole)
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Role);
                if (objRole.lgId == 0)
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

                if (objRole.hdniFrame)
                {
                    ViewData["iFrame"] = "iFrame";
                }

                bool blExists = _objiClsRole.IsRoleExists(objRole.lgId, objRole.strRoleName);
                if (blExists)
                {
                    ViewData["Success"] = "0";
                    ViewData["Message"] = _alertTextProvider.AlertMessage("Role", MessageType.AlreadyExists);
                }
                else
                {
                    string strErrorMsg = ValidateRole(objRole);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        ViewData["Success"] = "0";
                        ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        long resultId = _objiClsRole.SaveRole(objRole);
                        if (resultId > 0)
                        {
                            ViewData["Success"] = "1";
                            ViewData["Message"] = _alertTextProvider.AlertMessage("Role", MessageType.Success);
                        }
                        else
                        {
                            ViewData["Success"] = "0";
                            ViewData["Message"] = _alertTextProvider.AlertMessage("Role", MessageType.Fail);
                        }
                    }
                }

                return View(objRole);
            }
            catch (Exception ex)
            {
                ViewData["Success"] = "0";
                ViewData["Message"] = _alertTextProvider.AlertMessage("Role", MessageType.Fail);
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return View(objRole);
            }
        }

        public ActionResult RoleView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Role);
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
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return View();
            }
        }

        private ActionResult FillGrid(int page, int rows, List<SearchRoleResult> lstRole)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstRole != null && lstRole.Count > 0 ? lstRole[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var pagedRoleCol = lstRole;
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objRole in pagedRoleCol
                            select new
                            {
                                id = objRole.Id.ToString().Encode(),
                                RoleName = objRole.RoleName,
                                Description = objRole.Description
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

        private ActionResult FillRollPermissionGrid(List<GetPagePermissionResult> lstRolePermission)
        {
            try
            {
                int pageSize = lstRolePermission.Count;
                int totalRecords = lstRolePermission != null ? lstRolePermission.Count : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page = 1,
                    records = totalRecords,
                    rows = (from objRole in lstRolePermission
                            select new
                            {
                                Id = objRole.Id,
                                PageId = objRole.PageId,
                                RoleId = objRole.RoleId,
                                ModuleName = objRole.ModuleName,
                                PageName = objRole.PageName,
                                DispalyName = objRole.DispalyName,
                                View_Right = objRole.View_Right,
                                Add_Right = objRole.Add_Right,
                                Edit_Right = objRole.Edit_Right,
                                Delete_Right = objRole.Delete_Right,
                                Export_Right = objRole.Export_Right
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

        private string ValidateRole(ClsRole objRole)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objRole.strRoleName))
                {
                    strErrorMsg += _alertTextProvider.AlertMessage("Role Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objRole.strRights))
                {
                    strErrorMsg += "No Rights Are Selected." + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
                return string.Empty;
            }
        }
    }
}