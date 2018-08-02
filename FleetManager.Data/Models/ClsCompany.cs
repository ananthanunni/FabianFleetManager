using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FleetManager.Data.Models
{
    public class ClsCompany : DataContextEntity<CompanyDataContext>, IClsCompany
    {
	  public ClsCompany(CompanyDataContext context = null) : base()
	  {

	  }

	  public long Id { get; set; }

	  public string ShortName { get; set; }

	  public string FullName { get; set; }

	  public string Address1 { get; set; }

	  public string Address2 { get; set; }

	  public string Address3 { get; set; }

	  public int? VAT { get; set; }

	  public string Email { get; set; }

	  public string Person { get; set; }

	  public string Contact { get; set; }

	  public string Phone { get; set; }

	  public DateTime? CreatedOn { get; set; }

	  public bool? IsDeleted { get; set; }

	  public DateTime? DeletedOn { get; set; }

	  public ICollection<CompanyUser> CompanyUsers { get; set; }

	  public IEnumerable<IClsCompany> GetAll()
	  {
		using (objDataContext = GetDataContext())
		{
		    return objDataContext.Companies.Where(t => t.IsDeleted != true).ToList().Select(t => TranslateTypes<Company, ClsCompany>(t));
		}
	  }

	  public IClsCompany Get(int id)
	  {
		using (objDataContext = GetDataContext())
		{
		    var entity = objDataContext.Companies.SingleOrDefault(t => t.IsDeleted != true && t.Id == id);

		    if (entity == null) return null;

		    return TranslateTypes<Company, ClsCompany>(entity);
		}
	  }

	  public IClsCompany Save(IClsCompany companyVm)
	  {
		using (var tran = new TransactionScope())
		{
		    var company = TranslateTypes<IClsCompany, Company>(companyVm);
		    using (objDataContext = GetDataContext())
		    {
			  if (companyVm.Id == default(long))
			  {
				company.CreatedOn = DateTime.Now;
				objDataContext.Companies.InsertOnSubmit(company);
			  }
			  else
			  {
				var companyEntity = objDataContext.Companies.Single(t => t.Id == companyVm.Id);
				companyEntity = company;
			  }

			  objDataContext.SubmitChanges();
			  tran.Complete();
		    }

		    return TranslateTypes<Company, ClsCompany>(company);
		}
	  }

	  public void Delete(int id)
	  {
		using (var tran = new TransactionScope())
		{
		    using (objDataContext = GetDataContext())
		    {
			  var entity = objDataContext.Companies.SingleOrDefault(t => t.Id == id);

			  if (entity != null)
			  {
				entity.DeletedOn = DateTime.Now;
				entity.IsDeleted = true;
			  }

			  objDataContext.SubmitChanges();
			  tran.Complete();
		    }
		}
	  }

	  public bool AssignUserToCompany(int companyId, int userId, bool assignAsAdmin = false)
	  {
		using (var tran = new TransactionScope())
		{
		    using (objDataContext = GetDataContext())
		    {
			  var company = objDataContext.Companies.Single(t => t.Id == companyId);

			  objDataContext.CompanyUsers.InsertOnSubmit(new CompanyUser
			  {
				Company_Id = companyId,
				User_Id = userId,
				IsAdmin = assignAsAdmin
			  });
			  objDataContext.SubmitChanges();
			  tran.Complete();
		    }

		}

		return true;
	  }

	  public bool UnAssignUserToCompany(int companyId, int userId)
	  {
		using (var tran = new TransactionScope())
		{
		    using (objDataContext = GetDataContext())
		    {
			  var companyUser = objDataContext.CompanyUsers.SingleOrDefault(t => t.Company_Id == companyId && t.User_Id == userId);

			  if (companyUser == null) return false;

			  objDataContext.CompanyUsers.DeleteOnSubmit(companyUser);

			  tran.Complete();
		    }
		}

		return true;
	  }

	  public long CreateGroup(int companyId, string groupName, string description)
	  {
		using (var tran = new TransactionScope())
		{
		    using (objDataContext = GetDataContext())
		    {
			  var newGroup = new CompanyGroup
			  {
				Company_Id = companyId,
				Description = description,
				GroupName = groupName
			  };

			  objDataContext.CompanyGroups.InsertOnSubmit(newGroup);

			  objDataContext.SubmitChanges();
			  tran.Complete();

			  return newGroup.Id;
		    }
		}
	  }

	  public bool SetCompanyModulePermission(int companyGroupId, int moduleId, string right, bool flag)
	  {
		using (var tran = new TransactionScope())
		{
		    using (objDataContext = GetDataContext())
		    {
			  var permission = objDataContext.CompanyGroupModulePermissions.Single(t => t.IsDeleted != null && t.CompanyGroup_Id == companyGroupId && t.Module_Id == moduleId);

			  if (permission == null)
			  {
				var companyGroup = objDataContext.CompanyGroups.Single(t => t.Id == companyGroupId && t.IsDeleted != true);

				permission = new CompanyGroupModulePermission
				{
				    CompanyGroup_Id = companyGroupId,
				    Module_Id = moduleId
				};

				objDataContext.CompanyGroups.InsertOnSubmit(companyGroup);
			  }

			  // TODO: REFACTOR LATER WITH VIEWMODEL BASED IMPLEMENTATION. CURRENT IMPLEMENTATION ALLOWS ONLY ONE CHANGE AT A TIME. VERY IMPORTANT TO CHANGE LATER.
			  switch (right.ToUpper())
			  {
				case "VIEW":
				    permission.View_Right = flag;
				    break;

				case "ADD":
				    permission.Add_Right = flag;
				    break;

				case "EDIT":
				    permission.Edit_Right = flag;
				    break;

				case "DELETE":
				    permission.Delete_Right = flag;
				    break;

				default:
				    throw new ArgumentException("Use any of the four - VIEW, ADD, EDIT, DELETE", nameof(right));
			  }

			  objDataContext.SubmitChanges();
			  tran.Complete();

			  return true;
		    }
		}
	  }

	  public IClsCompany GetCompanyByGroup(int groupId)
	  {
		using (objDataContext = GetDataContext())
		{
		    var companyGroup = objDataContext.CompanyGroups.FirstOrDefault(t => t.IsDeleted != null && t.Id == groupId);

		    if (companyGroup != null)
			  return TranslateTypes<Company, ClsCompany>(companyGroup.Company);

		    return null;
		}
	  }

	  public bool DeleteGroup(int groupId)
	  {
		using (var tran = new TransactionScope())
		{
		    using (objDataContext = GetDataContext())
		    {
			  var group = objDataContext.CompanyGroups.Single(t => t.Id == groupId);

			  group.IsDeleted = true;
			  group.DeletedOn = DateTime.Now;

			  objDataContext.SubmitChanges();

			  tran.Complete();

			  return true;
		    }
		}
	  }

	  public bool AddUserToGroup(int groupId, int userId)
	  {
		using (var tran = new TransactionScope())
		{
		    using (objDataContext = GetDataContext())
		    {			  
			  var companyUser = objDataContext.CompanyUsers.Single(t => t.User_Id == userId);

			  if (companyUser == null)
				throw new InvalidOperationException("This user does not belong to the company. Add first.");

			  objDataContext.CompanyGroupCompanyUsers.InsertOnSubmit(new CompanyGroupCompanyUser
			  {
				CompanyGroup_Id = groupId,
				CompanyUser = companyUser
			  });

			  objDataContext.SubmitChanges();

			  tran.Complete();

			  return true;
		    }
		}
	  }
    }
}
