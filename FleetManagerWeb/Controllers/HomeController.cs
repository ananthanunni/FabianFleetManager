namespace FleetManagerWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using FleetManager.Core.Configuration;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class HomeController : Controller
    {
        private readonly IClsUser _objiClsUser = null;
	  private readonly IConfiguration _configuration;

	  public HomeController(IClsUser objIClsUser)
        {
            _objiClsUser = objIClsUser;
        }

        public ActionResult Index()
        {

            #region Menu Access
            bool blUserAccess = true, blRoleAccess = true, blTrackerAccess = true, blCarFleetAccess = true, blFleetMakesAccess = true, blFleetModelsAccess = true, blFleetColorsAccess = true, blTripReasonAccess = true;
            GetPagePermissionResult objPermission = Functions.CheckPagePermission(PageMaster.User);
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

        public ActionResult Login()
        {
            try
            {
                ClsUser objLogin = _objiClsUser as ClsUser;
                if (Functions.GetRememberMe("rememberme") == "true")
                {
                    objLogin.strUserName = Functions.GetRememberMe("username");
                    objLogin.strPassword = Functions.GetRememberMe("password");
                    ViewBag.password = Functions.GetRememberMe("password");
                    objLogin.blRememberMe = Convert.ToBoolean(Functions.GetRememberMe("rememberme"));
                }

                return View(objLogin);
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                throw;
            }
        }

        [HttpPost]
        public ActionResult Login(ClsUser objLogin)
        {
            try
            {
                ClsUser objClsUser = _objiClsUser.ValidateLogin(objLogin.strUserName, objLogin.strPassword.EncryptString());
                if (objClsUser != null)
                {
                    Functions.UpdateCookies(objClsUser.strUserName, objClsUser.strPassword.EncryptString(), objClsUser.lgId.ToString(), objClsUser.strFirstName + " " + objClsUser.strSurName, objLogin.blRememberMe.ToString(), objClsUser.lgRoleId.ToString(), objClsUser.lgBranchId.ToString(), objClsUser.lgUserTypeId.ToString(), "true");

                    objClsUser.blIsLogin = true;
                    _objiClsUser.SaveUser(objClsUser);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                return View(objLogin);
            }
        }

        public ActionResult Logout()
        {
            try
            {
                ClsUser objClsUser = _objiClsUser as ClsUser;
                objClsUser = _objiClsUser.GetUserByUserId(mySession.Current.UserId);
                if (objClsUser != null)
                {
                    objClsUser.blIsLogin = false;
                    _objiClsUser.SaveUser(objClsUser);
                }

                Functions.LogoutUser();
                ViewData.Clear();
                TempData.Clear();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                throw;
            }
        }

        public ActionResult PermissionRedirectPage()
        {
            if (mySession.Current.UserId == 0)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        public ActionResult UserUnAuthorize()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                return View();
            }
        }

        [HttpPost]
        public JsonResult ValidateLogin(ClsUser objLogin)
        {
            try
            {
                ClsUser objUser = _objiClsUser.ValidateLogin(objLogin.strUserName, objLogin.strPassword.EncryptString());
                if (objUser != null)
                {
                    // if (objUser.IsLogin)
                    // {
                    //    return Json("3333");
                    // }
                    return Json(objUser.strEmailID);
                }

                return Json("2222");
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                return Json("1111");
            }
        }

        public ActionResult ChangePassword(string strCurrentPwd, string strNewPwd)
        {
            try
            {
                if (mySession.Current.Password == strCurrentPwd.EncryptString())
                {
                    ClsUser objUser = _objiClsUser.ChangePassword(mySession.Current.UserId, strNewPwd);
                    Functions.UpdateCookies(mySession.Current.UserName, strNewPwd.EncryptString(), mySession.Current.UserId.ToString(), mySession.Current.Fullname, mySession.Current.Rememberme, mySession.Current.RoleId.ToString(), mySession.Current.BranchId.ToString(), mySession.Current.UserTypeId.ToString(), "false");
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("CurrentWrong", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                throw;
            }
        }
    }
}