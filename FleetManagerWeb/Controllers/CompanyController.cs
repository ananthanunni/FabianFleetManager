using FleetManager.Core.Common;
using FleetManager.Data.Models;
using FleetManager.Service.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FleetManagerWeb.Controllers
{
    /// <summary>
    /// Tring to be RESTFul
    /// </summary>
    public class CompanyController : BaseController
    {
	  private readonly ICompanyService _companyService;
	  private readonly IMySession _mySession;

	  public CompanyController(ICompanyService companyService, IMySession mySession)
		: base()
	  {
		_companyService = companyService;
		_mySession = mySession;
	  }

	  protected override void OnException(ExceptionContext filterContext)
	  {
		base.OnException(filterContext);
		if (filterContext.Exception is UnauthorizedAccessException)
		    filterContext.Result = new HttpUnauthorizedResult(filterContext.Exception.Message);
	  }

	  [HttpGet]
	  public ActionResult Get() => Json(_companyService.Get(), JsonRequestBehavior.AllowGet);

	  [HttpGet]
	  public ActionResult Get(int id) => Json(_companyService.Get(id), JsonRequestBehavior.AllowGet);

	  [HttpPost]
	  public ActionResult Post(IClsCompany viewModel) => Json(_companyService.Save(viewModel));

	  [HttpPut]
	  public ActionResult Put(int id, [System.Web.Http.FromBody]IClsCompany viewModel)
	  {
		viewModel.Id = id;
		return Json(_companyService.Save(viewModel));
	  }

	  // NON REST METHODS
	  // POST AssignAdmin?companyId?{int}&userId={userId}
	  [HttpPost]
	  public ActionResult AssignAdmin(int companyId, int userId)
	  {
		return Json(_companyService.AssignUserToCompany(companyId, userId));
	  }

	  [HttpPost]
	  // POST UnAssignAdmin?companyId?{int}&userId={userId}
	  public ActionResult UnAssignAdmin(int companyId,int userId)
	  {
		return Json(_companyService.UnAssignAdmin(companyId, userId));
	  }
    }
}
