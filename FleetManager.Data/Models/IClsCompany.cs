﻿using System;
using System.Collections.Generic;

namespace FleetManager.Data.Models
{
    public interface IClsCompany
    {
	  string Address1 { get; set; }
	  string Address2 { get; set; }
	  string Address3 { get; set; }
	  string Contact { get; set; }
	  DateTime? CreatedOn { get; set; }
	  DateTime? DeletedOn { get; set; }
	  string Email { get; set; }
	  string FullName { get; set; }
	  long Id { get; set; }
	  bool? IsDeleted { get; set; }
	  string Person { get; set; }
	  string Phone { get; set; }
	  string ShortName { get; set; }
	  int? VAT { get; set; }

	  void Delete(int id);
	  IClsCompany Get(int id);
	  IEnumerable<IClsCompany> GetAll();
	  IClsCompany Save(IClsCompany companyVm);
    }
}