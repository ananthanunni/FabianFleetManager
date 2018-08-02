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
    }
}
