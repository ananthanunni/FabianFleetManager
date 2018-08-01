namespace FleetManagerWeb
{
    using FleetManager.Core.Common;
    using FleetManager.Core.Extensions;
    using FleetManager.Data.Models;
    using FleetManager.Service.Auth;
    using FleetManager.Service.Cookie;
    using FleetManagerWeb.Controllers;
    using FleetManagerWeb.Models;
    using System;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        private readonly IClsUser _objiClsUser = null;
	  private readonly IPermissionChecker _permissionChecker;
	  private readonly IMySession _mySession;
	  private readonly ICookieHandler _cookieHandler;
	  private readonly IAuthentication _authentication;

	  public HomeController(IClsUser objIClsUser,IPermissionChecker permissionChecker,IMySession mySession, ICookieHandler cookieHandler,IAuthentication authentication)
		:base()
        {
            _objiClsUser = objIClsUser;
		_permissionChecker = permissionChecker;
		_mySession = mySession;
		_cookieHandler = cookieHandler;
		_authentication = authentication;
	  }

        public ActionResult Index()
        {
            #region Menu Access
            bool blUserAccess = true, blRoleAccess = true, blTrackerAccess = true, blCarFleetAccess = true, blFleetMakesAccess = true, blFleetModelsAccess = true, blFleetColorsAccess = true, blTripReasonAccess = true;
            GetPagePermissionResult objPermission = _permissionChecker.CheckPagePermission(PageMaster.User);
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

        public ActionResult Login()
        {
            try
            {
                ClsUser objLogin = _objiClsUser as ClsUser;
                if (GetCookieValue("rememberme") == "true")
                {
                    objLogin.strUserName = GetCookieValue("username");
                    objLogin.strPassword = GetCookieValue("password");
                    ViewBag.password = GetCookieValue("password");
                    objLogin.blRememberMe = Convert.ToBoolean(GetCookieValue("rememberme"));
                }

                return View(objLogin);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
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
                    _cookieHandler.UpdateCookies(objClsUser.strUserName, objClsUser.strPassword.EncryptString(), objClsUser.lgId.ToString(), objClsUser.strFirstName + " " + objClsUser.strSurName, objLogin.blRememberMe.ToString(), objClsUser.lgRoleId.ToString(), objClsUser.lgBranchId.ToString(), objClsUser.lgUserTypeId.ToString(), "true");

                    objClsUser.blIsLogin = true;
                    _objiClsUser.SaveUser(objClsUser);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                return View(objLogin);
            }
        }

        public ActionResult Logout()
        {
            try
            {
                ClsUser objClsUser = _objiClsUser as ClsUser;
                objClsUser = _objiClsUser.GetUserByUserId(_mySession.UserId);
                if (objClsUser != null)
                {
                    objClsUser.blIsLogin = false;
                    _objiClsUser.SaveUser(objClsUser);
                }

                _authentication.LogoutUser();
                ViewData.Clear();
                TempData.Clear();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                throw;
            }
        }

        public ActionResult PermissionRedirectPage()
        {
            if (_mySession.UserId == 0)
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
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
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
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                return Json("1111");
            }
        }

        public ActionResult ChangePassword(string strCurrentPwd, string strNewPwd)
        {
            try
            {
                if (_mySession.Password == strCurrentPwd.EncryptString())
                {
                    ClsUser objUser = _objiClsUser.ChangePassword(_mySession.UserId, strNewPwd);
                    _cookieHandler.UpdateCookies(_mySession.UserName, strNewPwd.EncryptString(), _mySession.UserId.ToString(), _mySession.Fullname, _mySession.Rememberme, _mySession.RoleId.ToString(), _mySession.BranchId.ToString(), _mySession.UserTypeId.ToString(), "false");
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("CurrentWrong", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon);
                throw;
            }
        }
    }
}