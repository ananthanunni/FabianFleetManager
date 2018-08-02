using FleetManager.Core.Common;
using FleetManager.Core.Extensions;
using FleetManager.Data.Models;
using FleetManager.Model.Interaction;
using FleetManager.Service.Auth;
using FleetManager.Service.Interaction;
using FleetManagerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FleetManagerWeb.Controllers
{
    public class CarFleetController : BaseController
    {
	  private readonly IClsCarFleet _objiClsCarFleet = null;
	  private readonly IClsFleetColors _objiClsFleetColors = null;
	  private readonly IClsFleetMakes _objiClsFleetMakes = null;
	  private readonly IClsFleetModels _objiClsFleetModels = null;
	  private readonly IAlertTextProvider _alertTextProvider;
	  private readonly IMySession _mySession;
	  private readonly IPermissionChecker _permissionChecker;

	  public CarFleetController(IClsCarFleet objIClsCarFleet, IClsFleetColors objiClsFleetColors, IClsFleetMakes objiClsFleetMakes, IClsFleetModels objiClsFleetModels,
		IAlertTextProvider alertTextProvider, IMySession mySession, IPermissionChecker permissionChecker)
	  {
		_objiClsCarFleet = objIClsCarFleet;
		_objiClsFleetColors = objiClsFleetColors;
		_objiClsFleetMakes = objiClsFleetMakes;
		_objiClsFleetModels = objiClsFleetModels;

		_alertTextProvider = alertTextProvider;
		_mySession = mySession;
		_permissionChecker = permissionChecker;
	  }

	  public void BindDropDownListForCarFleet(ClsCarFleet objClsCarFleet, bool blBindDropDownFromDb)
	  {
		try
		{
		    if (blBindDropDownFromDb)
		    {
			  objClsCarFleet.lstFleetColors = _objiClsFleetColors.GetAllFleetColorsForDropDown().ToList();
			  objClsCarFleet.lstFleetMakes = _objiClsFleetMakes.GetAllFleetMakesForDropDown().ToList();
			  objClsCarFleet.lstFleetModels = _objiClsFleetModels.GetAllFleetModelsForDropDown().ToList();

		    }
		    else
		    {
			  objClsCarFleet.lstFleetColors = new List<SelectListItem>();
			  objClsCarFleet.lstFleetMakes = new List<SelectListItem>();
			  objClsCarFleet.lstFleetModels = new List<SelectListItem>();
		    }
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
		}
	  }

	  public ActionResult BindCarFleetGrid(string sidx, string sord, int page, int rows, string filters, string search, string tripstartdate, string tripenddate)
	  {
		try
		{
		    List<SearchCarFleetResult> lstCarFleet = _objiClsCarFleet.SearchCarFleet(rows, page, search, sidx + " " + sord, tripstartdate, tripenddate);
		    if (lstCarFleet != null)
		    {
			  return FillGridCarFleet(page, rows, lstCarFleet);
		    }
		    else
		    {
			  return Json(string.Empty);
		    }
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, _mySession.UserId);
		    return Json(string.Empty);
		}
	  }

	  public JsonResult DeleteCarFleet(string strCarFleetId)
	  {
		try
		{
		    string[] strCarFleet = strCarFleetId.Split(',');
		    strCarFleetId = string.Empty;
		    foreach (var item in strCarFleet)
		    {
			  strCarFleetId += item.Decode() + ",";
		    }

		    strCarFleetId = strCarFleetId.Substring(0, strCarFleetId.Length - 1);
		    DeleteCarFleetResult result = _objiClsCarFleet.DeleteCarFleet(strCarFleetId, _mySession.UserId);
		    if (result != null && result.TotalReference == 0)
		    {
			  return Json(_alertTextProvider.AlertMessage("User", MessageType.DeleteSuccess));
		    }
		    else if (result != null && result.TotalReference > 0)
		    {
			  return Json(_alertTextProvider.AlertMessage("User", MessageType.DeletePartial, result.Name));
		    }

		    return Json(_alertTextProvider.AlertMessage("User", MessageType.DeleteFail));

		    // return Json(_alertTextProvider.AlertMessage("CarFleet", MessageType.DeleteSucess));
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, _mySession.UserId);
		    return Json(_alertTextProvider.AlertMessage("CarFleet", MessageType.DeleteFail));
		}
	  }

	  public ActionResult CarFleet()
	  {
		try
		{
		    var objPermission = _permissionChecker.CheckPagePermission(PageMaster.CarFleet);
		    if (!objPermission.IsActive)
		    {
			  return RedirectToAction("Logout", "Home");
		    }

		    ClsCarFleet objClsCarFleet = _objiClsCarFleet as ClsCarFleet;
		    long lgCarFleetId = 0;
		    if (Request.QueryString.Count > 0)
		    {
			  if (Request.QueryString["iFrame"] != null)
			  {
				if (!objPermission.Add_Right)
				{
				    return RedirectToAction("PermissionRedirectPage", "Home");
				}

				objClsCarFleet.hdniFrame = true;
				ViewData["iFrame"] = "iFrame";
			  }
			  else
			  {
				if (!objPermission.Edit_Right || string.IsNullOrEmpty(Request.QueryString.ToString().Decode()))
				{
				    return RedirectToAction("PermissionRedirectPage", "Home");
				}

				lgCarFleetId = Request.QueryString.ToString().Decode().LongSafe();
				objClsCarFleet = _objiClsCarFleet.GetCarFleetByCarFleetId(lgCarFleetId);
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
			  blCarFleetAccess = false;
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

		    BindDropDownListForCarFleet(objClsCarFleet, true);

		    return View(objClsCarFleet);
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, _mySession.UserId);
		    return View();
		}
	  }

	  [HttpPost]
	  public ActionResult CarFleet(ClsCarFleet objCarFleet)
	  {
		try
		{
		    GetPagePermissionResult objPermission = _permissionChecker.CheckPagePermission(PageMaster.CarFleet);
		    if (!objPermission.IsActive)
		    {
			  return RedirectToAction("Logout", "Home");
		    }

		    if (objCarFleet.inId == 0)
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

		    if (objCarFleet.hdniFrame)
		    {
			  ViewData["iFrame"] = "iFrame";
		    }

		    string strErrorMsg = ValidateCarFleet(objCarFleet);

		    if (ModelState.IsValid)
		    {
			  if (!string.IsNullOrEmpty(strErrorMsg))
			  {
				ViewData["Success"] = "0";
				ViewData["Message"] = strErrorMsg;
			  }
			  else
			  {
				objCarFleet.inId = _objiClsCarFleet.SaveCarFleet(objCarFleet);
				if (objCarFleet.inId > 0)
				{
				    ViewData["Success"] = "1";
				    ViewData["Message"] = _alertTextProvider.AlertMessage("CarFleet", MessageType.Success);
				    BindDropDownListForCarFleet(objCarFleet, false);
				    return View(objCarFleet);
				}
				else
				{
				    ViewData["Success"] = "0";
				    ViewData["Message"] = _alertTextProvider.AlertMessage("CarFleet", MessageType.Fail);
				}
			  }
		    }

		    BindDropDownListForCarFleet(objCarFleet, true);

		    return View(objCarFleet);
		}
		catch (Exception ex)
		{
		    ViewData["Success"] = "0";
		    ViewData["Message"] = _alertTextProvider.AlertMessage("CarFleet", MessageType.Fail);
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, _mySession.UserId);
		    return View();
		}
	  }

	  public ActionResult CarFleetView()
	  {
		try
		{
		    var objPermission = _permissionChecker.CheckPagePermission(PageMaster.CarFleet);
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
			  blCarFleetAccess = false;
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
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, _mySession.UserId);
		    return View();
		}
	  }

	  private ActionResult FillGridCarFleet(int page, int rows, List<SearchCarFleetResult> lstCarFleet)
	  {
		try
		{
		    int pageSize = rows;
		    int totalRecords = lstCarFleet != null && lstCarFleet.Count > 0 ? lstCarFleet[0].Total : 0;
		    int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);
		    var jsonData = new
		    {
			  total = totalPages,
			  page,
			  records = totalRecords,
			  rows = (from objCarFleet in lstCarFleet
				    select new
				    {
					  Id = objCarFleet.Id.ToString().Encode(),
					  Owner_Id = objCarFleet.Owner_Id,
					  Last_Trip = objCarFleet.Last_Trip,
					  Code = objCarFleet.Code,
					  Reg = objCarFleet.Reg,
					  Desc = objCarFleet.Desc,
					  Color_Id = objCarFleet.Color_Id,
					  Fuel_Type = objCarFleet.Fuel_Type,
					  Last_Km = objCarFleet.Last_Km,
					  Last_Location = objCarFleet.Last_Location,
					  Make = objCarFleet.Make,
					  Model = objCarFleet.Model,
					  FleetMakes_Id = objCarFleet.FleetMakes_Id,
					  FleetModels_Id = objCarFleet.FleetModels_Id

				    }).ToArray()
		    };
		    return Json(jsonData, JsonRequestBehavior.AllowGet);
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, _mySession.UserId);
		    return Json(string.Empty, JsonRequestBehavior.AllowGet);
		}
	  }

	  private string ValidateCarFleet(ClsCarFleet objCarFleet)
	  {
		try
		{
		    string strErrorMsg = string.Empty;
		    if (string.IsNullOrEmpty(objCarFleet.strCode))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Code", MessageType.InputRequired) + "<br/>";
		    }
		    else if (string.IsNullOrEmpty(objCarFleet.strReg))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Registration", MessageType.InputRequired) + "<br/>";
		    }
		    else if (string.IsNullOrEmpty(objCarFleet.strFuel_Type.ToString()))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Fuel Type", MessageType.InputRequired) + "<br/>";
		    }
		    else if (string.IsNullOrEmpty(objCarFleet.strLast_Trip.ToString()))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Last Trip Date", MessageType.InputRequired) + "<br/>";
		    }
		    else if (string.IsNullOrEmpty(objCarFleet.inLast_Km.ToString()))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Last Km", MessageType.InputRequired) + "<br/>";
		    }
		    else if (string.IsNullOrEmpty(objCarFleet.strLast_Location.ToString()))
		    {
			  strErrorMsg += _alertTextProvider.AlertMessage("Last Location", MessageType.InputRequired) + "<br/>";
		    }

		    return strErrorMsg;
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, _mySession.UserId);
		    return string.Empty;
		}
	  }
    }
}