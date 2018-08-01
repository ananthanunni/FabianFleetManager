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

    public class FleetColorsController : BaseController
    {
        private readonly IClsFleetColors _objiClsFleetColors = null;
	  private readonly IAlertTextProvider _alertTextProvider;
	  private readonly IPermissionChecker _permissionChecker;
	  private readonly IMySession _mySession;

	  public FleetColorsController(IClsFleetColors objIClsFleetColors, IAlertTextProvider alertTextProvider,IPermissionChecker permissionChecker,IMySession mySession)
        {
            _objiClsFleetColors = objIClsFleetColors;
		_alertTextProvider = alertTextProvider;
		_permissionChecker = permissionChecker;
		_mySession = mySession;
	  }

        public ActionResult BindFleetColorsGrid(string sidx, string sord, int page, int rows, string filters, string search)
        {
            try
            {
                List<SearchFleetColorsResult> lstFleetColors = _objiClsFleetColors.SearchFleetColors(rows, page, search, sidx + " " + sord);
                if (lstFleetColors != null)
                {
                    return FillGridFleetColors(page, rows, lstFleetColors);
                }
                else
                {
                    return Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, _mySession.UserId);
                return Json(string.Empty);
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
                DeleteFleetColorsResult result = _objiClsFleetColors.DeleteFleetColors(strFleetColorsId, _mySession.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return Json(_alertTextProvider.AlertMessage("Fleet Colors", MessageType.DeleteSuccess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return Json(_alertTextProvider.AlertMessage("Fleet Colors", MessageType.DeletePartial, result.Name));
                }

                return Json(_alertTextProvider.AlertMessage("Fleet Colors", MessageType.DeleteFail));
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, _mySession.UserId);
                return Json(_alertTextProvider.AlertMessage("Fleet Colors", MessageType.DeleteFail));
            }
        }

        public JsonResult GetFleetColors()
        {
            try
            {
                return Json(_objiClsFleetColors.GetAllFleetColorsForDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, _mySession.UserId);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FleetColors()
        {
            try
            {
                var objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetColors);
                if (!objPermission.IsActive)
                {
                    return RedirectToAction("Logout", "Home");
                }

                ClsFleetColors objClsFleetColors = _objiClsFleetColors as ClsFleetColors;
                long lgFleetColorsId = 0;
                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsFleetColors.hdniFrame = true;
                        ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
                        {
                            return RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgFleetColorsId = Request.QueryString.ToString().Decode().LongSafe();
                        objClsFleetColors = _objiClsFleetColors.GetFleetColorsByFleetColorsId(lgFleetColorsId);
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

                return View(objClsFleetColors);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, _mySession.UserId);
                return View();
            }
        }

        [HttpPost]
        public ActionResult FleetColors(ClsFleetColors objFleetColors)
        {
            try
            {
                var objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetColors);
                if (!objPermission.IsActive)
                {
                    return RedirectToAction("Logout", "Home");
                }

                if (objFleetColors.lgId == 0)
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

                if (objFleetColors.hdniFrame)
                {
                    ViewData["iFrame"] = "iFrame";
                }

                bool blExists = _objiClsFleetColors.IsFleetColorsExists(objFleetColors.lgId, objFleetColors.strFleetColorsName);
                if (blExists)
                {
                    ViewData["Success"] = "0";
                    ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Colors", MessageType.AlreadyExists);
                }
                else
                {
                    string strErrorMsg = ValidateFleetColors(objFleetColors);
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        ViewData["Success"] = "0";
                        ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objFleetColors.lgId = _objiClsFleetColors.SaveFleetColors(objFleetColors);
                        if (objFleetColors.lgId > 0)
                        {
                            ViewData["Success"] = "1";
                            ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Colors", MessageType.Success);
                            return View(objFleetColors);
                        }
                        else
                        {
                            ViewData["Success"] = "0";
                            ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Colors", MessageType.Fail);
                        }
                    }
                }

                return View(objFleetColors);
            }
            catch (Exception ex)
            {
                ViewData["Success"] = "0";
                ViewData["Message"] = _alertTextProvider.AlertMessage("Fleet Colors", MessageType.Fail);
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, _mySession.UserId);
                return View(objFleetColors);
            }
        }

        public ActionResult FleetColorsView()
        {
            try
            {
                var objPermission = _permissionChecker.CheckPagePermission(PageMaster.FleetColors);
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
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, _mySession.UserId);
                return View();
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
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, _mySession.UserId);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateFleetColors(ClsFleetColors objFleetColors)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objFleetColors.strFleetColorsName))
                {
                    strErrorMsg += _alertTextProvider.AlertMessage("Fleet Colors Name", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, _mySession.UserId);
                return string.Empty;
            }
        }
    }
}