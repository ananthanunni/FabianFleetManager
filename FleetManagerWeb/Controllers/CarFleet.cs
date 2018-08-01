namespace FleetManagerWeb.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class CarFleetController : Controller
    {
        private readonly IClsCarFleet objiClsCarFleet = null;
        private readonly IClsFleetColors objiClsFleetColors = null;
        private readonly IClsFleetMakes objiClsFleetMakes = null;
        private readonly IClsFleetModels objiClsFleetModels = null;
        public CarFleetController(IClsCarFleet objIClsCarFleet, IClsFleetColors objiClsFleetColors, IClsFleetMakes objiClsFleetMakes, IClsFleetModels objiClsFleetModels)
        {
            this.objiClsCarFleet = objIClsCarFleet;
            this.objiClsFleetColors = objiClsFleetColors;
            this.objiClsFleetMakes = objiClsFleetMakes;
            this.objiClsFleetModels = objiClsFleetModels;
        }

        public void BindDropDownListForCarFleet(ClsCarFleet objClsCarFleet, bool blBindDropDownFromDb)
        {
            try
            {
                if (blBindDropDownFromDb)
                {
                    objClsCarFleet.lstFleetColors = this.objiClsFleetColors.GetAllFleetColorsForDropDown().ToList();
                    objClsCarFleet.lstFleetMakes = this.objiClsFleetMakes.GetAllFleetMakesForDropDown().ToList();
                    objClsCarFleet.lstFleetModels = this.objiClsFleetModels.GetAllFleetModelsForDropDown().ToList();
                 
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User);
            }
        }

        public ActionResult BindCarFleetGrid(string sidx, string sord, int page, int rows, string filters, string search, string tripstartdate, string tripenddate)
        {
            try
            {
                List<SearchCarFleetResult> lstCarFleet = this.objiClsCarFleet.SearchCarFleet(rows, page, search, sidx + " " + sord, tripstartdate, tripenddate);
                if (lstCarFleet != null)
                {
                    return this.FillGridCarFleet(page, rows, lstCarFleet);
                }
                else
                {
                    return this.Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, mySession.Current.UserId);
                return this.Json(string.Empty);
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
                DeleteCarFleetResult result = this.objiClsCarFleet.DeleteCarFleet(strCarFleetId, mySession.Current.UserId);
                if (result != null && result.TotalReference == 0)
                {
                    return this.Json(Functions.AlertMessage("User", MessageType.DeleteSucess));
                }
                else if (result != null && result.TotalReference > 0)
                {
                    return this.Json(Functions.AlertMessage("User", MessageType.DeletePartial, result.Name));
                }

                return this.Json(Functions.AlertMessage("User", MessageType.DeleteFail));

               // return this.Json(Functions.AlertMessage("CarFleet", MessageType.DeleteSucess));
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, mySession.Current.UserId);
                return this.Json(Functions.AlertMessage("CarFleet", MessageType.DeleteFail));
            }
        }

        public ActionResult CarFleet()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.CarFleet);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                ClsCarFleet objClsCarFleet = this.objiClsCarFleet as ClsCarFleet;
                long lgCarFleetId = 0;
                if (this.Request.QueryString.Count > 0)
                {
                    if (this.Request.QueryString["iFrame"] != null)
                    {
                        if (!objPermission.Add_Right)
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        objClsCarFleet.hdniFrame = true;
                        this.ViewData["iFrame"] = "iFrame";
                    }
                    else
                    {
                        if (!objPermission.Edit_Right || string.IsNullOrEmpty(this.Request.QueryString.ToString().Decode()))
                        {
                            return this.RedirectToAction("PermissionRedirectPage", "Home");
                        }

                        lgCarFleetId = this.Request.QueryString.ToString().Decode().longSafe();
                        objClsCarFleet = this.objiClsCarFleet.GetCarFleetByCarFleetId(lgCarFleetId);
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
                    blCarFleetAccess = false;
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

                this.BindDropDownListForCarFleet(objClsCarFleet, true);

                return this.View(objClsCarFleet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, mySession.Current.UserId);
                return this.View();
            }
        }

        [HttpPost]
        public ActionResult CarFleet(ClsCarFleet objCarFleet)
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.CarFleet);
                if (!objPermission.IsActive)
                {
                    return this.RedirectToAction("Logout", "Home");
                }

                if (objCarFleet.inId == 0)
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

                if (objCarFleet.hdniFrame)
                {
                    this.ViewData["iFrame"] = "iFrame";
                }

                string strErrorMsg = this.ValidateCarFleet(objCarFleet);

                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(strErrorMsg))
                    {
                        this.ViewData["Success"] = "0";
                        this.ViewData["Message"] = strErrorMsg;
                    }
                    else
                    {
                        objCarFleet.inId = this.objiClsCarFleet.SaveCarFleet(objCarFleet);
                        if (objCarFleet.inId > 0)
                        {
                            this.ViewData["Success"] = "1";
                            this.ViewData["Message"] = Functions.AlertMessage("CarFleet", MessageType.Success);
                            this.BindDropDownListForCarFleet(objCarFleet, false);
                            return this.View(objCarFleet);
                        }
                        else
                        {
                            this.ViewData["Success"] = "0";
                            this.ViewData["Message"] = Functions.AlertMessage("CarFleet", MessageType.Fail);
                        }
                    }
                }

                this.BindDropDownListForCarFleet(objCarFleet, true);

                return this.View(objCarFleet);
            }
            catch (Exception ex)
            {
                this.ViewData["Success"] = "0";
                this.ViewData["Message"] = Functions.AlertMessage("CarFleet", MessageType.Fail);
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, mySession.Current.UserId);
                return this.View();
            }
        }

        public ActionResult CarFleetView()
        {
            try
            {
                GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.CarFleet);
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
                    blCarFleetAccess = false;
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, mySession.Current.UserId);
                return this.View();
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
                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, mySession.Current.UserId);
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
        }

        private string ValidateCarFleet(ClsCarFleet objCarFleet)
        {
            try
            {
                string strErrorMsg = string.Empty;
                if (string.IsNullOrEmpty(objCarFleet.strCode))
                {
                    strErrorMsg += Functions.AlertMessage("Code", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objCarFleet.strReg))
                {
                    strErrorMsg += Functions.AlertMessage("Registration", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objCarFleet.strFuel_Type.ToString()))
                {
                    strErrorMsg += Functions.AlertMessage("Fuel Type", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objCarFleet.strLast_Trip.ToString()))
                {
                    strErrorMsg += Functions.AlertMessage("Last Trip Date", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objCarFleet.inLast_Km.ToString()))
                {
                    strErrorMsg += Functions.AlertMessage("Last Km", MessageType.InputRequired) + "<br/>";
                }
                else if (string.IsNullOrEmpty(objCarFleet.strLast_Location.ToString()))
                {
                    strErrorMsg += Functions.AlertMessage("Last Location", MessageType.InputRequired) + "<br/>";
                }

                return strErrorMsg;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, mySession.Current.UserId);
                return string.Empty;
            }
        }
    }
}