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
	  private readonly IClsRole _role;
	  private readonly IMySession _mySession;
	  private readonly IClsUser _user;

	  public CompanyService(IClsCompany company, IClsRole role,IMySession mySession,IClsUser user)
	  {
		_company = company;
		_role = role;
		_mySession = mySession;
		_user = user;
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
		if (!IsSysAdmin())
		    throw new UnauthorizedAccessException("You are not authorized to create/edit company information.");

		return _company.Save(company);
	  }

	  public void Delete(int id)
	  {
		if (!IsSysAdmin())
		    throw new UnauthorizedAccessException("You are not authorized to delete company.");

		_company.Delete(id);
	  }

	  public bool AssignUserAsCompanyAdmin(int companyId, int userId)
	  {
		if (!IsSysAdmin())
		    throw new UnauthorizedAccessException("You are not authorized to assign admins to company.");

		if (!IsValidUser(userId))
		    throw new InvalidOperationException("Invalid user.");

		return _company.AssignUserToCompany(companyId, userId,true);
	  }

	  public bool UnAssignUserAsCompanyAdmin(int companyId, int userId)
	  {
		if (!IsSysAdmin())
		    throw new UnauthorizedAccessException("You are not authorized to unassign admins from company.");

		if (!IsValidUser(userId))
		    throw new InvalidOperationException("Invalid user.");

		return _company.UnAssignUserToCompany(companyId, userId);
	  }

	  public long CreateGroup(int companyId, string groupName, string description)
	  {
		if (!IsCompanyAdmin(companyId))
		    throw new UnauthorizedAccessException("You are not authorized to create company groups.");

		var company = _company.Get(companyId);

		if (company == null)
		    throw new InvalidOperationException("Invalid company");

		return _company.CreateGroup(companyId, groupName,description);
	  }

	  public bool SetCompanyModulePermission(int companyGroupId, int moduleId, string right, bool flag)
	  {
		if(!IsCompanyAdmin(companyGroupId))
		    throw new UnauthorizedAccessException("You are not authorized to create company groups.");

		return _company.SetCompanyModulePermission(companyGroupId, moduleId, right, flag);
	  }

	  public bool DeleteGroup(int groupId)
	  {
		var company = _company.GetCompanyByGroup(groupId);

		if (company == null)
		    throw new InvalidOperationException("Invalid group.");

		if (!IsCompanyAdmin((int)company.Id))
		    throw new UnauthorizedAccessException("You are not authorized to delete group.");

		return _company.DeleteGroup(groupId);
	  }

	  private bool IsSysAdmin()
	  {
		return (_role.GetRoleByRoleId(_mySession.RoleId) as ClsRole).strRoleName.Equals("SYSADMIN");
	  }

	  private bool IsValidUser(int userId)
	  {
		var user = _user.GetUserByUserId(userId);
		return user != null && user.BlIsActive;
	  }

	  private bool IsCompanyAdmin(int companyId)
	  {
		if (IsValidUser(_mySession.UserId)) return false;

		var company = _company.Get(companyId);

		if (company == null) return false;

		return company.CompanyUsers.Any(t => t.Company.Id == companyId && t.IsAdmin == true && t.User_Id==_mySession.UserId);
	  }
    }
}
