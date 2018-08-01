namespace FleetManagerWeb.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class FleetColorsController : Controller
    {
        private readonly IClsFleetColors objiClsFleetColors = null;

        public FleetColorsController(IClsFleetColors objIClsFleetColors)
        {
            this.objiClsFleetColors = objIClsFleetColors;
        }

        public ActionResult BindFleetColorsGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchFleetColorsResult> lstFleetColors = this.objiClsFleetColors.SearchFleetColors(rows, page, search, sidx + " " + sord);
                if (lstFleetColors != null)
                {
                    return this.FillGridFleetColors(page, rows, lstFleetColors);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public JsonResult DeleteFleetColors(string strFleetColorsId)
        {
            try
            {
                string[] strFleetColors = strFleetColorsId.Split(',');
                strFleetColorsId = string.Empty;
                foreach (var item in strFleetColors)
                {
                    strFleetColorsId += item.Decode() + ",";
                }

                strFleetColorsId = strFleetColorsId.Substring(0, strFleetColorsId.Length - 1);
                DeleteFleetColorsResult result = this.objiClsFleetColors.DeleteFleetColors(strFleetColorsId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Fleet Colors", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Fleet Colors", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Fleet Colors", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Fleet Colors", MessageType.DeleteFail));
            }
        }

        public JsonResult GetFleetColors()
        {
            try
            {
                return this.Json(this.objiClsFleetColors.GetAllFleetColorsForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FleetColors()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.FleetColors);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ClsFleetColors objClsFleetColors = this.objiClsFleetColors as ClsFleetColors;
                long lgFleetColorsId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsFleetColors.hdniFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgFleetColorsId = this.Request.QueryString.ToString().Decode().longSafe();
                        objClsFleetColors = this.objiClsFleetColors.GetFleetColorsByFleetColorsId(lgFleetColorsId);
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

                return this.View(objClsFleetColors);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult FleetColors(ClsFleetColors objFleetColors)
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.FleetColors);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objFleetColors.lgId == 0)
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

                if (objFleetColors.hdniFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objiClsFleetColors.IsFleetColorsExists(objFleetColors.lgId, objFleetColors.strFleetColorsName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Fleet Colors", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateFleetColors(objFleetColors);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objFleetColors.lgId = this.objiClsFleetColors.SaveFleetColors(objFleetColors);
                        if (objFleetColors.lgId > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Fleet Colors", MessageType.Success);
                            return this.View(objFleetColors);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Fleet Colors", MessageType.Fail);
                        }
                    }
                }

                return this.View(objFleetColors);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Fleet Colors", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return this.View(objFleetColors);
            }
        }

        public ActionResult FleetColorsView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.FleetColors);
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return this.View();
            }
        }

        private ActionResult FillGridFleetColors(int page, int rows, List<SearchFleetColorsResult> lstFleetColors)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstFleetColors != null && lstFleetColors.Count > 0 ? lstFleetColors[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objFleetColors in lstFleetColors
                            select new
                            {
                                FleetColorsName = objFleetColors.FleetColorsName,
                                Id = objFleetColors.Id.ToString().Encode()
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateFleetColors(ClsFleetColors objFleetColors)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objFleetColors.strFleetColorsName))
                {
                    strErrorMsg += Functions.AlertMessage("Fleet Colors Name", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return string.Empty;
            }
        }
    }
}