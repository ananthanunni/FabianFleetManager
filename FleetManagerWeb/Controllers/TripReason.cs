namespace FleetManagerWeb.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class TripReasonController : Controller
    {
        private readonly IClsTripReason objiClsTripReason = null;

        public TripReasonController(IClsTripReason objIClsTripReason)
        {
            this.objiClsTripReason = objIClsTripReason;
        }

        public ActionResult BindTripReasonGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchTripReasonResult> lstTripReason = this.objiClsTripReason.SearchTripReason(rows, page, search, sidx + " " + sord);
                if (lstTripReason != null)
                {
                    return this.FillGridTripReason(page, rows, lstTripReason);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public JsonResult DeleteTripReason(string strTripReasonId)
        {
            try
            {
                string[] strTripReason = strTripReasonId.Split(',');
                strTripReasonId = string.Empty;
                foreach (var item in strTripReason)
                {
                    strTripReasonId += item.Decode() + ",";
                }

                strTripReasonId = strTripReasonId.Substring(0, strTripReasonId.Length - 1);
                DeleteTripReasonResult result = this.objiClsTripReason.DeleteTripReason(strTripReasonId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("Trip Reason", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("Trip Reason", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("Trip Reason", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Trip Reason", MessageType.DeleteFail));
            }
        }

        public JsonResult GetTripReason()
        {
            try
            {
                return this.Json(this.objiClsTripReason.GetAllTripReasonForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TripReason()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.TripReason);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ClsTripReason objClsTripReason = this.objiClsTripReason as ClsTripReason;
                long lgTripReasonId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsTripReason.hdniFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgTripReasonId = this.Request.QueryString.ToString().Decode().longSafe();
                        objClsTripReason = this.objiClsTripReason.GetTripReasonByTripReasonId(lgTripReasonId);
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

                return this.View(objClsTripReason);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult TripReason(ClsTripReason objTripReason)
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.TripReason);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objTripReason.lgId == 0)
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

                if (objTripReason.hdniFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                bool blExists = this.objiClsTripReason.IsTripReasonExists(objTripReason.lgId, objTripReason.strTripReasonName);
                if (blExists)
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = Functions.AlertMessage("Trip Reason", MessageType.AlreadyExist);
                }
                else
                {
                    string strErrorMsg = this.ValidateTripReason(objTripReason);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objTripReason.lgId = this.objiClsTripReason.SaveTripReason(objTripReason);
                        if (objTripReason.lgId > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("Trip Reason", MessageType.Success);
                            return this.View(objTripReason);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("Trip Reason", MessageType.Fail);
                        }
                    }
                }

                return this.View(objTripReason);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Trip Reason", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return this.View(objTripReason);
            }
        }

        public ActionResult TripReasonView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.TripReason);
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return this.View();
            }
        }

        private ActionResult FillGridTripReason(int page, int rows, List<SearchTripReasonResult> lstTripReason)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstTripReason != null && lstTripReason.Count > 0 ? lstTripReason[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objTripReason in lstTripReason
                            select new
                            {
                                TripReasonName = objTripReason.TripReasonName,
                                Id = objTripReason.Id.ToString().Encode()
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateTripReason(ClsTripReason objTripReason)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objTripReason.strTripReasonName))
                {
                    strErrorMsg += Functions.AlertMessage("Trip Reason Name", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return string.Empty;
            }
        }
    }
}