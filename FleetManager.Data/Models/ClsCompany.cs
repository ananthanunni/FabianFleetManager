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

	  public IEnumerable<IClsCompany> GetAll()
	  {
		using (objDataContext = GetDataContext())
		{
		    return objDataContext.Companies.ToList().Select(t => Convert<Company, ClsCompany>(t));
		}
	  }

	  public IClsCompany Get(int id)
	  {
		using (objDataContext = GetDataContext())
		{
		    var entity = objDataContext.Companies.SingleOrDefault(t => t.Id == id);

		    if (entity == null) return null;

		    return Convert<Company, ClsCompany>(entity);
		}
	  }

	  public IClsCompany Save(IClsCompany companyVm)
	  {
		using (var tran = new TransactionScope())
		{
		    var company = Convert<IClsCompany, Company>(companyVm);
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
		    }

		    tran.Complete();

		    return Convert<Company, ClsCompany>(company);
		}
	  }

	  public void Delete(int id)
	  {
		using(var tran=new TransactionScope())
		{
		    using (objDataContext = GetDataContext())
		    {
			  var entity = objDataContext.Companies.SingleOrDefault(t => t.Id == id);

			  if (entity != null)
			  {
				entity.DeletedOn = DateTime.Now;
				entity.IsDeleted = true;
			  }
		    }

		    tran.Complete();
		}
	  }
    }
}
