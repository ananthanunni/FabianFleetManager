namespace FleetManagerWeb.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class TrackerController : Controller
    {
        private readonly IClsTracker objiClsTracker = null;

        public TrackerController(IClsTracker objIClsTracker)
        {
            this.objiClsTracker = objIClsTracker;
        }

        public ActionResult BindTrackerGrid(string sidx, string sord, int page, int rows, string filters, string search, string tripstartdate, string tripenddate, string locationstart, string locationend)
        {
            try
            {
                List<SearchTrackerResult> lstTracker = this.objiClsTracker.SearchTracker(rows, page, search, sidx + " " + sord, tripstartdate, tripenddate, locationstart, locationend);
                if (lstTracker != null)
                {
                    return this.FillGridTracker(page, rows, lstTracker);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.Json(string.Empty);
            }
        }

        public JsonResult DeleteTracker(string strTrackerId)
        {
            try
            {
                string[] strTracker = strTrackerId.Split(',');
                strTrackerId = string.Empty;
                foreach (var item in strTracker)
                {
                    strTrackerId += item.Decode() + ",";
                }

                strTrackerId = strTrackerId.Substring(0, strTrackerId.Length - 1);
                return this.Json(Functions.AlertMessage("Tracker", MessageType.DeleteSucess));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("Tracker", MessageType.DeleteFail));
            }
        }

        public ActionResult Tracker()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ClsTracker objClsTracker = this.objiClsTracker as ClsTracker;
                long lgTrackerId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsTracker.hdniFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgTrackerId = this.Request.QueryString.ToString().Decode().longSafe();
                        objClsTracker = this.objiClsTracker.GetTrackerByTrackerId(lgTrackerId);
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

                return this.View(objClsTracker);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult Tracker(ClsTracker objTracker)
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objTracker.inId == 0)
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

                if (objTracker.hdniFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                string strErrorMsg = this.ValidateTracker(objTracker);
                if (!string.IsNullOrEmpty(strErrorMsg))
                {
                    this.ViewData["Success"] = "0";
                    this.ViewData["Message"] = strErrorMsg;
                }
                else
                {
                    objTracker.inId = this.objiClsTracker.SaveTracker(objTracker);
                    if (objTracker.inId > 0)
                    {
                        this.ViewData["Success"] = "1";
                        this.ViewData["Message"] = Functions.AlertMessage("Tracker", MessageType.Success);
                        return this.View(objTracker);
                    }
                    else
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = Functions.AlertMessage("Tracker", MessageType.Fail);
                    }
                }

                return this.View(objTracker);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("Tracker", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.View(objTracker);
            }
        }

        public ActionResult TrackerView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.Tracker);
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.View();
            }
        }

        private ActionResult FillGridTracker(int page, int rows, List<SearchTrackerResult> lstTracker)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstTracker != null && lstTracker.Count > 0 ? lstTracker[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objTracker in lstTracker
                            select new
                            {
                                TripStartDate = objTracker.TripStartDate,
                                TripEndDate = objTracker.TripEndDate,
                                LocationStart = objTracker.LocationStart,
                                LocationEnd = objTracker.LocationEnd,
                                ReasonRemarks = objTracker.ReasonRemarks,
                                KmStart = objTracker.KmStart,
                                KmEnd = objTracker.KmEnd,
                                KmDriven = objTracker.KmDriven,
                                FuelStart = objTracker.FuelStart,
                                FuelEnd = objTracker.FuelEnd,
                                Username = objTracker.Username,
                                EntryDatetime = objTracker.EntryDatetime,
                                EntryMethod = objTracker.EntryMethod,
                                Editable = objTracker.Editable,
                                Active = objTracker.Active,
                                Id = objTracker.Id.ToString().Encode()
                            }).ToArray()
                };
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateTracker(ClsTracker objTracker)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objTracker.strTripStart))
                {
                    strErrorMsg += Functions.AlertMessage("Trip Start Date", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objTracker.strTripEnd))
                {
                    strErrorMsg += Functions.AlertMessage("Trip End Date", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objTracker.inKmStart.ToString()))
                {
                    strErrorMsg += Functions.AlertMessage("Km Start", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objTracker.inKmEnd.ToString()))
                {
                    strErrorMsg += Functions.AlertMessage("Km End", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objTracker.inFuelStart.ToString()))
                {
                    strErrorMsg += Functions.AlertMessage("Fuel Start", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objTracker.inFuelEnd.ToString()))
                {
                    strErrorMsg += Functions.AlertMessage("Fuel End", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, mySession.Current.UserId);
                return string.Empty;
            }
        }
    }
}