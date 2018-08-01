namespace FleetManagerWeb.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManager.Model.Common;
    using FleetManager.Model.Interaction;
    using FleetManager.Service.Auth;
    using FleetManager.Service.Interaction;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Model.Common;
    using FleetManagerWeb.Models;

    public class FleetMakesController : BaseController
    {
        private readonly IClsFleetMakes _objiClsFleetMakes = null;
	  private readonly IAlertTextProvider _alertTextProvider;
	  private readonly IMySession _mySession;
	  private readonly IPermissionChecker _permissionChecker;

	  public FleetMakesController(IClsFleetMakes objIClsFleetMakes, IAlertTextProvider alertTextProvider, IMySession mySession,IPermissionChecker permissionChecker)
        {
            _objiClsFleetMakes = objIClsFleetMakes;
		_alertTextProvider = alertTextProvider;
		_mySession = mySession;
		_permissionChecker = permissionChecker;
	  }

        public ActionResult BindFleetMakesGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchFleetMakesResult> lstFleetMakes = _objiClsFleetMakes.SearchFleetMakes(rows, page, search, sidx + " " + sord);
                if (lstFleetMakes != null)
                {
                    return FillGridFleetMakes(page, rows, lstFleetMakes);
                }
                else
                {
                    return Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, _mySession.UserId);
                return Json(string.Empty);
            }
        }

        public JsonResult DeleteFleetMakes(string strFleetMakesId)
        {
            try
            {
                string[] strFleetMakes = strFleetMakesId.Split(',');
                strFleetMakesId = string.Empty;
                foreach (var item in strFleetMakes)
                {
                    strFleetMakesId += item.Decode() + ",";
                }

                strFleetMakesId = strFleetMakesId.Substring(0, strFleetMakesId.Length - 1);
                DeleteFleetMakesResult result = _objiClsFleetMakes.DeleteFleetMakes(strFleetMakesId, _mySession.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return Json(_alertTextProvider.AlertMessage("Fleet Makes", MessageType.DeleteSuccess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return Json(_alertTextProvider.AlertMessage("Fleet Makes", MessageType.DeletePartial, result.Name));
                }

                return Json(_alertTextProvider.AlertMessage("Fleet Makes", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, _mySession.UserId);
                return Json(_alertTextProvider.AlertMessage("Fleet Makes", MessageType.DeleteFail));
            }
        }

        public JsonResult GetFleetMakes()
        {
            try
            {
                return Json(_objiClsFleetMakes.GetAllFleetMakesForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, _mySession.UserId);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FleetMakes()
        {
            try
            {
                var objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetMakes);
                if (!objPermission.IsActive)
                {
                    return RedirectToAction("Logout", "Home");
                }

                ClsFleetMakes objClsFleetMakes = _objiClsFleetMakes as ClsFleetMakes;
                long lgFleetMakesId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsFleetMakes.hdniFrame = true;
                        ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgFleetMakesId = Request.QueryString.ToString().Decode().LongSafe();
                        objClsFleetMakes = _objiClsFleetMakes.GetFleetMakesByFleetMakesId(lgFleetMakesId);
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
                objPermission = _permissionChecker.CheckPagePermission(PageMaster.User);
                if (!objPermission.Add_Right)
                {
                    blUserAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.Role);
                if (!objPermission.Add_Right)
                {
                    blRoleAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.Add_Right)
                {
                    blTrackerAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.CarFleet);
                if (!objPermission.Add_Right)
                {
                    blCarFleetAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetMakes);
                if (!objPermission.Add_Right)
                {
                    blFleetMakesAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetModels);
                if (!objPermission.Add_Right)
                {
                    blFleetModelsAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetColors);
                if (!objPermission.Add_Right)
                {
                    blFleetColorsAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.TripReason);
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

                return View(objClsFleetMakes);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, _mySession.UserId);
                return View();
            }
        }

        [HttpPost]
        public ActionResult FleetMakes(ClsFleetMakes objFleetMakes)
        {
            try
            {
                var objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetMakes);
                if (!objPermission.IsActive)
                {
                    return RedirectToAction("Logout", "Home");
                }

                if (objFleetMakes.lgId == 0)
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

                if (objFleetMakes.hdniFrame)
                {
                    ViewData["iFrame"] = "iFrame";
                }

                bool blExists = _objiClsFleetMakes.IsFleetMakesExists(objFleetMakes.lgId, objFleetMakes.strFleetMakesName);
                if (blExists)
                {
                    ViewData["Success"] = "0";
                    ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Makes", MessageType.AlreadyExists);
                }
                else
                {
                    string strErrorMsg = ValidateFleetMakes(objFleetMakes);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        ViewData["Success"] = "0";
                        ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objFleetMakes.lgId = _objiClsFleetMakes.SaveFleetMakes(objFleetMakes);
                        if (objFleetMakes.lgId > 0)
                        {
                            ViewData["Success"] = "1";
                            ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Makes", MessageType.Success);
                            return View(objFleetMakes);
                        }
                        else
                        {
                            ViewData["Success"] = "0";
                            ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Makes", MessageType.Fail);
                        }
                    }
                }

                return View(objFleetMakes);
            }
            catch (Exception ex)
            {
                ViewData["Success"] = "0";
                ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Makes", MessageType.Fail);
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, _mySession.UserId);
                return View(objFleetMakes);
            }
        }

        public ActionResult FleetMakesView()
        {
            try
            {
                var objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetMakes);
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
                objPermission = _permissionChecker.CheckPagePermission(PageMaster.User);
                if (!objPermission.Add_Right)
                {
                    blUserAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.Role);
                if (!objPermission.Add_Right)
                {
                    blRoleAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.Tracker);
                if (!objPermission.Add_Right)
                {
                    blTrackerAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.CarFleet);
                if (!objPermission.Add_Right)
                {
                    blCarFleetAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetMakes);
                if (!objPermission.Add_Right)
                {
                    blFleetMakesAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetModels);
                if (!objPermission.Add_Right)
                {
                    blFleetModelsAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetColors);
                if (!objPermission.Add_Right)
                {
                    blFleetColorsAccess = false;
                }

                objPermission = _permissionChecker.CheckPagePermission(PageMaster.TripReason);
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
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, _mySession.UserId);
                return View();
            }
        }

        private ActionResult FillGridFleetMakes(int page, int rows, List<SearchFleetMakesResult> lstFleetMakes)
        {
            try
            {
                int pageSize = rows;
                int totalRecords = lstFleetMakes != null && lstFleetMakes.Count > 0 ? lstFleetMakes[0].Total : 0;
                int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalRecords,
                    rows = (from objFleetMakes in lstFleetMakes
                            select new
                            {
                                FleetMakesName = objFleetMakes.FleetMakesName,
                                Id = objFleetMakes.Id.ToString().Encode()
                            }).ToArray()
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, _mySession.UserId);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateFleetMakes(ClsFleetMakes objFleetMakes)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objFleetMakes.strFleetMakesName))
                {
                    strErrorMsg += _alertTextProvider.AlertMessage("Fleet Makes Name", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, _mySession.UserId);
                return string.Empty;
            }
        }
    }
}