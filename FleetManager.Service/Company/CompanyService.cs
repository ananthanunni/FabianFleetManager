using FleetManager.Core.Common;
using FleetManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManager.Service.Company
{
    public class CompanyService : ICompanyService
    {
	  private readonly IClsCompany _company;
	  private readonly IMySession _mySession;

	  public CompanyService(IClsCompany company,IMySession mySession)
	  {
		_company = company;
		_mySession = mySession;
	  }

	  public IEnumerable<IClsCompany> Get()
	  {
		return _company.GetAll();
	  }

	  public IClsCompany Get(int id)
	  {
		return _company.Get(id);
	  }

	  public IClsCompany Save(IClsCompany company)
	  {		
		return _company.Save(company);
	  }

	  public void Delete(int id)
	  {
		_company.Delete(id);
	  }
    }
}
