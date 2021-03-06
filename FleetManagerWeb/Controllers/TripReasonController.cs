﻿using FleetManager.Core.Common;
using FleetManager.Core.Extensions;
using FleetManager.Data.Models;
using FleetManager.Model.Interaction;
using FleetManager.Service.Auth;
using FleetManager.Service.Interaction;
using FleetManager.Service.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FleetManagerWeb.Controllers
{
    public class TripReasonController : BaseController
    {
	  private readonly ITrackerService _trackerService = null;
	  private readonly IAlertTextProvider _alertTextProvider;
	  private readonly IPermissionChecker _permissionChecker;
	  private readonly IMySession _mySession;

	  public TripReasonController(ITrackerService objIClsTripReason, IAlertTextProvider alertTextProvider, IPermissionChecker permissionChecker, IMySession mySession)
	  {
		_trackerService = objIClsTripReason;
		_alertTextProvider = alertTextProvider;
		_permissionChecker = permissionChecker;
		_mySession = mySession;
	  }

	  public ActionResult BindTripReasonGrid(string sidx, string sord, int page, int rows, string filters, string search)
	  {
		try
		{
		    List<SearchTripReasonResult> lstTripReason = _trackerService.SearchTripReason(rows, page, search, sidx + " " + sord);
		    if (lstTripReason != null)
		    {
			  return FillGridTripReason(page, rows, lstTripReason);
		    }
		    else
		    {
			  return Json(string.Empty);
		    }
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
		    return Json(string.Empty);
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
		    DeleteTripReasonResult result = _trackerService.DeleteTripReason(strTripReasonId, _mySession.UserId);
		    if (result != null && result.TotalReference == 0)
		    {
			  return Json(_alertTextProvider.AlertMessage("Trip Reason", MessageType.DeleteSuccess));
		    }
		    else if (result != null && result.TotalReference > 0)
		    {
			  return Json(_alertTextProvider.AlertMessage("Trip Reason", MessageType.DeletePartial, result.Name));
		    }

		    return Json(_alertTextProvider.AlertMessage("Trip Reason", MessageType.DeleteFail));
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
		    return Json(_alertTextProvider.AlertMessage("Trip Reason", MessageType.DeleteFail));
		}
	  }

	  public JsonResult GetTripReason()
	  {
		try
		{
		    return Json(_trackerService.GetAllTripReasonForDropDown(), JsonRequestBehavior.AllowGet);
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
		    return Json(string.Empty, JsonRequestBehavior.AllowGet);
		}
	  }

	  public ActionResult TripReason()
	  {
		try
		{
		    var objPermission = _permissionChecker.CheckPagePermission(PageMaster.TripReason);
		    if (!objPermission.IsActive)
		    {
			  return RedirectToAction("Logout", "Home");
		    }

		    ClsTripReason objClsTripReason = DependencyResolver.Current.GetService<IClsTripReason>() as ClsTripReason;
		    long lgTripReasonId = 0;
		    if (Request.QueryString.Count > 0)
		    {
			  if (Request.QueryString["iFrame"] != null)
			  {
				if (!objPermission.Add_Right)
				{
				    return RedirectToAction("PermissionRedirectPage", "Home");
				}

				objClsTripReason.hdniFrame = true;
				ViewData["iFrame"] = "iFrame";
			  }
			  else
			  {
				if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
				{
				    return RedirectToAction("PermissionRedirectPage", "Home");
				}

				lgTripReasonId = Request.QueryString.ToString().Decode().LongSafe();
				objClsTripReason = _trackerService.GetTripReasonByTripReasonId(lgTripReasonId) as ClsTripReason;
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

		    return View(objClsTripReason);
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
		    return View();
		}
	  }

	  [HttpPost]
	  public ActionResult TripReason(ClsTripReason objTripReason)
	  {
		try
		{
		    var objPermission = _permissionChecker.CheckPagePermission(PageMaster.TripReason);
		    if (!objPermission.IsActive)
		    {
			  return RedirectToAction("Logout", "Home");
		    }

		    if (objTripReason.lgId == 0)
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

		    if (objTripReason.hdniFrame)
		    {
			  ViewData["iFrame"] = "iFrame";
		    }

		    bool blExists = _trackerService.IsTripReasonExists(objTripReason.lgId, objTripReason.strTripReasonName);
		    if (blExists)
		    {
			  ViewData["Success"] = "0";
			  ViewData["Message"] = _alertTextProvider.AlertMessage("Trip Reason", MessageType.AlreadyExists);
		    }
		    else
		    {
			  string strErrorMsg = ValidateTripReason(objTripReason);
			  if (!string.IsNullOrEmpty(strErrorMsg))
			  {
				ViewData["Success"] = "0";
				ViewData["Message"] = strErrorMsg;
			  }
			  else
			  {
				objTripReason.lgId = _trackerService.SaveTripReason(objTripReason);
				if (objTripReason.lgId > 0)
				{
				    ViewData["Success"] = "1";
				    ViewData["Message"] = _alertTextProvider.AlertMessage("Trip Reason", MessageType.Success);
				    return View(objTripReason);
				}
				else
				{
				    ViewData["Success"] = "0";
				    ViewData["Message"] = _alertTextProvider.AlertMessage("Trip Reason", MessageType.Fail);
				}
			  }
		    }

		    return View(objTripReason);
		}
		catch (Exception ex)
		{
		    ViewData["Success"] = "0";
		    ViewData["Message"] = _alertTextProvider.AlertMessage("Trip Reason", MessageType.Fail);
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
		    return View(objTripReason);
		}
	  }

	  public ActionResult TripReasonView()
	  {
		try
		{
		    var objPermission = _permissionChecker.CheckPagePermission(PageMaster.TripReason);
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
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
		    return View();
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
		    return Json(jsonData, JsonRequestBehavior.AllowGet);
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
		    return Json(string.Empty, JsonRequestBehavior.AllowGet);
		}
	  }

	  private string ValidateTripReason(ClsTripReason objTripReason)
	  {
		try
		{
		    string strErrorMsg = string.Empty;
		    if (string.IsNullOrEmpty(objTripReason.strTripReasonName))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Trip Reason Name", MessageType.InputRequired) + "<br/>";
		    }

		    return strErrorMsg;
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
		    return string.Empty;
		}
	  }
    }
}