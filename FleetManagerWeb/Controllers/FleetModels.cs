namespace FleetManagerWeb.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManager.Model.Interaction;
    using FleetManager.Service.Interaction;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class FleetModelsController : BaseController
    {
        private readonly IClsFleetModels _objiClsFleetModels = null;
	  private readonly IAlertTextProvider _alertTextProvider;

	  public FleetModelsController(IClsFleetModels objIClsFleetModels,IAlertTextProvider alertTextProvider)
        {
            _objiClsFleetModels = objIClsFleetModels;
		_alertTextProvider = alertTextProvider;
        }

        public ActionResult BindFleetModelsGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchFleetModelsResult> lstFleetModels = _objiClsFleetModels.SearchFleetModels(rows, page, search, sidx + " " + sord);
                if (lstFleetModels != null)
                {
                    return FillGridFleetModels(page, rows, lstFleetModels);
                }
                else
                {
                    return Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, mySession.Current.UserId);
                return Json(string.Empty);
            }
        }

        public JsonResult DeleteFleetModels(string strFleetModelsId)
        {
            try
            {
                string[] strFleetModels = strFleetModelsId.Split(',');
                strFleetModelsId = string.Empty;
                foreach (var item in strFleetModels)
                {
                    strFleetModelsId += item.Decode() + ",";
                }

                strFleetModelsId = strFleetModelsId.Substring(0, strFleetModelsId.Length - 1);
                DeleteFleetModelsResult result = _objiClsFleetModels.DeleteFleetModels(strFleetModelsId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return Json(_alertTextProvider.AlertMessage("Fleet Models", MessageType.DeleteSuccess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return Json(_alertTextProvider.AlertMessage("Fleet Models", MessageType.DeletePartial, result.Name));
                }

                return Json(_alertTextProvider.AlertMessage("Fleet Models", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, mySession.Current.UserId);
                return Json(_alertTextProvider.AlertMessage("Fleet Models", MessageType.DeleteFail));
            }
        }

        public JsonResult GetFleetModels()
        {
            try
            {
                return Json(_objiClsFleetModels.GetAllFleetModelsForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, mySession.Current.UserId);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FleetModels()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.FleetModels);
                if (!objPermission.IsActive)
                {
                    return RedirectToAction("Logout", "Home");
                }

                ClsFleetModels objClsFleetModels = _objiClsFleetModels as ClsFleetModels;
                long lgFleetModelsId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsFleetModels.hdniFrame = true;
                        ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgFleetModelsId = Request.QueryString.ToString().Decode().longSafe();
                        objClsFleetModels = _objiClsFleetModels.GetFleetModelsByFleetModelsId(lgFleetModelsId);
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

                return View(objClsFleetModels);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, mySession.Current.UserId);
                return View();
            }
        }

        [HttpPost]
        public ActionResult FleetModels(ClsFleetModels objFleetModels)
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.FleetModels);
                if (!objPermission.IsActive)
                {
                    return RedirectToAction("Logout", "Home");
                }

                if (objFleetModels.lgId == 0)
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

                if (objFleetModels.hdniFrame)
                {
                    ViewData["iFrame"] = "iFrame";
                }

                bool blExists = _objiClsFleetModels.IsFleetModelsExists(objFleetModels.lgId, objFleetModels.strFleetModelsName);
                if (blExists)
                {
                    ViewData["Success"] = "0";
                    ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Models", MessageType.AlreadyExists);
                }
                else
                {
                    string strErrorMsg = ValidateFleetModels(objFleetModels);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        ViewData["Success"] = "0";
                        ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objFleetModels.lgId = _objiClsFleetModels.SaveFleetModels(objFleetModels);
                        if (objFleetModels.lgId > 0)
                        {
                            ViewData["Success"] = "1";
                            ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Models", MessageType.Success);
                            return View(objFleetModels);
                        }
                        else
                        {
                            ViewData["Success"] = "0";
                            ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Models", MessageType.Fail);
                        }
                    }
                }

                return View(objFleetModels);
            }
            catch (Exception ex)
            {
                ViewData["Success"] = "0";
                ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Models", MessageType.Fail);
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, mySession.Current.UserId);
                return View(objFleetModels);
            }
        }

        public ActionResult FleetModelsView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.FleetModels);
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
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, mySession.Current.UserId);
                return View();
            }
        }

        private ActionResult FillGridFleetModels(int page, int rows, List<SearchFleetModelsResult> lstFleetModels)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstFleetModels != null && lstFleetModels.Count > 0 ? lstFleetModels[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objFleetModels in lstFleetModels
                            select new
                            {
                                FleetModelsName = objFleetModels.FleetModelsName,
                                Id = objFleetModels.Id.ToString().Encode()
                            }).ToArray()
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, mySession.Current.UserId);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateFleetModels(ClsFleetModels objFleetModels)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objFleetModels.strFleetModelsName))
                {
                    strErrorMsg += _alertTextProvider.AlertMessage("Fleet Models Name", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, mySession.Current.UserId);
                return string.Empty;
            }
        }
    }
}