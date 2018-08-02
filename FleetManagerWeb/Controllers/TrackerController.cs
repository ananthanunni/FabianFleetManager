using FleetManager.Core.Common;
using FleetManager.Core.Extensions;
using FleetManager.Data.Models;
using FleetManager.Model.Interaction;
using FleetManager.Service.Auth;
using FleetManager.Service.Interaction;
using FleetManager.Service.Tracking;
using FleetManager.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FleetManagerWeb.Controllers
{
    public class TrackerController : BaseController
    {
	  private readonly ITrackerService _trackerService = null;
	  private readonly IAlertTextProvider _alertTextProvider;
	  private readonly IMySession _mySession;
	  private readonly IPermissionChecker _permissionChecker;

	  public TrackerController(ITrackerService trackerService, IAlertTextProvider alertTextProvider, IMySession mySession, IPermissionChecker permissionChecker)
	  {
		_trackerService = trackerService;
		_alertTextProvider = alertTextProvider;
		_mySession = mySession;
		_permissionChecker = permissionChecker;
	  }

	  public ActionResult BindTrackerGrid(string sidx, string sord, int page, int rows, string filters, string search, string tripstartdate, string tripenddate, string locationstart, string locationend)
	  {
		try
		{
		    List<SearchTrackerResult> lstTracker = _trackerService.SearchTracker(rows, page, search, sidx + " " + sord, tripstartdate, tripenddate, locationstart, locationend);
		    if (lstTracker != null)
		    {
			  return FillGridTracker(page, rows, lstTracker);
		    }
		    else
		    {
			  return Json(string.Empty);
		    }
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, _mySession.UserId);
		    return Json(string.Empty);
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
		    return Json(_alertTextProvider.AlertMessage("Tracker", MessageType.DeleteSuccess));
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, _mySession.UserId);
		    return Json(_alertTextProvider.AlertMessage("Tracker", MessageType.DeleteFail));
		}
	  }

	  public ActionResult Tracker()
	  {
		try
		{
		    var objPermission = _permissionChecker.CheckPagePermission(PageMaster.Tracker);
		    if (!objPermission.IsActive)
		    {
			  return RedirectToAction("Logout", "Home");
		    }

		    ClsTracker objClsTracker = _trackerService as ClsTracker;
		    long lgTrackerId = 0;
		    if (Request.QueryString.Count > 0)
		    {
			  if (Request.QueryString["iFrame"] != null)
			  {
				if (!objPermission.Add_Right)
				{
				    return RedirectToAction("PermissionRedirectPage", "Home");
				}

				objClsTracker.hdniFrame = true;
				ViewData["iFrame"] = "iFrame";
			  }
			  else
			  {
				if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
				{
				    return RedirectToAction("PermissionRedirectPage", "Home");
				}

				lgTrackerId = Request.QueryString.ToString().Decode().LongSafe();
				objClsTracker = _trackerService.GetTrackerByTrackerId(lgTrackerId) as ClsTracker;
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

		    return View(objClsTracker);
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, _mySession.UserId);
		    return View();
		}
	  }

	  [HttpPost]
	  public ActionResult Tracker(ClsTracker objTracker)
	  {
		try
		{
		    var objPermission = _permissionChecker.CheckPagePermission(PageMaster.Tracker);
		    if (!objPermission.IsActive)
		    {
			  return RedirectToAction("Logout", "Home");
		    }

		    if (objTracker.inId == 0)
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

		    if (objTracker.hdniFrame)
		    {
			  ViewData["iFrame"] = "iFrame";
		    }

		    string strErrorMsg = ValidateTracker(objTracker);
		    if (!string.IsNullOrEmpty(strErrorMsg))
		    {
			  ViewData["Success"] = "0";
			  ViewData["Message"] = strErrorMsg;
		    }
		    else
		    {
			  objTracker.inId = _trackerService.SaveTracker(objTracker);
			  if (objTracker.inId > 0)
			  {
				ViewData["Success"] = "1";
				ViewData["Message"] = _alertTextProvider.AlertMessage("Tracker", MessageType.Success);
				return View(objTracker);
			  }
			  else
			  {
				ViewData["Success"] = "0";
				ViewData["Message"] = _alertTextProvider.AlertMessage("Tracker", MessageType.Fail);
			  }
		    }

		    return View(objTracker);
		}
		catch (Exception ex)
		{
		    ViewData["Success"] = "0";
		    ViewData["Message"] = _alertTextProvider.AlertMessage("Tracker", MessageType.Fail);
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, _mySession.UserId);
		    return View(objTracker);
		}
	  }

	  public ActionResult TrackerView()
	  {
		try
		{
		    var objPermission = _permissionChecker.CheckPagePermission(PageMaster.Tracker);
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
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, _mySession.UserId);
		    return View();
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
		    return Json(jsonData, JsonRequestBehavior.AllowGet);
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, _mySession.UserId);
		    return Json(string.Empty, JsonRequestBehavior.AllowGet);
		}
	  }

	  private string ValidateTracker(ClsTracker objTracker)
	  {
		try
		{
		    string strErrorMsg = string.Empty;
		    if (string.IsNullOrEmpty(objTracker.strTripStart))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Trip Start Date", MessageType.InputRequired) + "<br/>";
		    }
		    else if (string.IsNullOrEmpty(objTracker.strTripEnd))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Trip End Date", MessageType.InputRequired) + "<br/>";
		    }
		    else if (string.IsNullOrEmpty(objTracker.inKmStart.ToString()))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Km Start", MessageType.InputRequired) + "<br/>";
		    }
		    else if (string.IsNullOrEmpty(objTracker.inKmEnd.ToString()))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Km End", MessageType.InputRequired) + "<br/>";
		    }
		    else if (string.IsNullOrEmpty(objTracker.inFuelStart.ToString()))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Fuel Start", MessageType.InputRequired) + "<br/>";
		    }
		    else if (string.IsNullOrEmpty(objTracker.inFuelEnd.ToString()))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Fuel End", MessageType.InputRequired) + "<br/>";
		    }

		    return strErrorMsg;
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, _mySession.UserId);
		    return string.Empty;
		}
	  }
    }
}