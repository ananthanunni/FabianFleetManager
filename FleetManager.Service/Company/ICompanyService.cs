using System.Collections.Generic;
using FleetManager.Data.Models;

namespace FleetManager.Service.Company
{
    public interface ICompanyService
    {
	  void Delete(int id);
	  IEnumerable<IClsCompany> Get();
	  IClsCompany Get(int id);
	  IClsCompany Save(IClsCompany company);
	  bool AssignUserToCompany(int companyId, int userId);
	  bool UnAssignAdmin(int companyId, int userId);
    }
}