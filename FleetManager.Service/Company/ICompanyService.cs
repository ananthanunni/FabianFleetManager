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
	  bool AssignUserAsCompanyAdmin(int companyId, int userId);
	  bool UnAssignUserAsCompanyAdmin(int companyId, int userId);
	  long CreateGroup(int companyId, string groupName, string description);
	  bool SetCompanyModulePermission(int companyGroupId, int moduleId, string right, bool flag);
    }
}