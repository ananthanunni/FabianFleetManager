namespace FleetManagerWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class RoleController : Controller
    {
        /// <summary>   Zero-based index of the cls role. </summary>
        private readonly IClsRole objiClsRole = null;

        public RoleController(IClsRole objIClsRole)
        {
            this.objiClsRole = objIClsRole;
        }

        public ActionResult BindRoleGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchRoleResult> lstRole = this.objiClsRole.SearchRole(rows, page, search, sidx + " " + sord);
                if (lstRole != null)
                {
                    return this.FillGrid(page, rows, lstRole);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return this.Json(string.Empty);
            }
        }

        public ActionResult BindRolePermissionGrid(string sidx, string sord, int page, int rows, string filters, long lgRoleId)
        {
            try
            {
                List<GetPagePermissionResult> lstRolePermission = Functions.GerRolePermissionByRoleId(lgRoleId);
                if (lstRolePermission != null)
                {
                    return this.FillRollPermissionGrid(lstRolePermission);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
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
                DeleteRoleResult result = this.objiClsRole.DeleteRole(strRoleId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Role", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Role", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Role", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return this.Json(Functions.AlertMessage("Role", MessageType.DeleteFail));
            }
        }

        public JsonResult GetRole()
        {
            try
            {
                return this.Json(this.objiClsRole.GetAllRoleForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Role()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Role);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ClsRole objClsRole = this.objiClsRole as ClsRole;
                long lgRoleId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsRole.hdniFrame = true;
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgRoleId = this.Request.QueryString.ToString().Decode().longSafe();
                        objClsRole = this.objiClsRole.GetRoleByRoleId(lgRoleId);
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

                return this.View(objClsRole);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return this.View();
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

                if (objRole.hdniFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objiClsRole.IsRoleExists(objRole.lgId, objRole.strRoleName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Role", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateRole(objRole);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        long resultId = this.objiClsRole.SaveRole(objRole);
                        if (resultId > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Role", MessageType.Success);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Role", MessageType.Fail);
                        }
                    }
                }

                return this.View(objRole);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Role", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return this.View(objRole);
            }
        }

        public ActionResult RoleView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Role);
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role);
                return this.View();
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
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
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
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateRole(ClsRole objRole)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objRole.strRoleName))
                {
                    strErrorMsg += Functions.AlertMessage("Role Name", MessageType.InputRequired) + "<br/>";
                }

                if (string.IsNullOrEmpty(objRole.strRights))
                {
                    strErrorMsg += "No Rights Are Selected." + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
                return string.Empty;
            }
        }
    }
}