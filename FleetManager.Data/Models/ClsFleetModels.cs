using FleetManager.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace FleetManager.Data.Models
{
    public partial class ClsFleetModels : DataContextEntity<FleetModelsDataContext>, IClsFleetModels
    {
	  public ClsFleetModels()
	  {

	  }
	  public ClsFleetModels(FleetModelsDataContext context) : base(context)
	  {
	  }

	  public bool hdniFrame { get; set; }

	  public long lgId { get; set; }

	  public List<ClsFleetModels> listFleetModels { get; set; }

	  public string strFleetModelsName { get; set; }

	  public DeleteFleetModelsResult DeleteFleetModels(string strFleetModelsIdList, long lgDeletedBy)
	  {
		DeleteFleetModelsResult result = new DeleteFleetModelsResult();
		try
		{
		    using (TransactionScope scope = new TransactionScope())
		    {
			  using (objDataContext = GetDataContext())
			  {
				result = this.objDataContext.DeleteFleetModels(strFleetModelsIdList, lgDeletedBy, PageMaster.FleetModels).ToList().FirstOrDefault();
			  }

			  objDataContext.SubmitChanges();
			  scope.Complete();
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, _mySession.UserId);
		}

		return result;
	  }

	  public List<SelectListItem> GetAllFleetModelsForDropDown()
	  {
		List<SelectListItem> lstFleetModels = new List<SelectListItem>();
		try
		{
		    using (objDataContext = GetDataContext())
		    {
			  lstFleetModels.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
			  List<GetFleetModelsAllResult> lstFleetModelsResult = this.objDataContext.GetFleetModelsAll().ToList();
			  if (lstFleetModelsResult != null && lstFleetModelsResult.Count > 0)
			  {
				foreach (var item in lstFleetModelsResult)
				{
				    lstFleetModels.Add(new SelectListItem { Text = item.FleetModelsName, Value = item.Id.ToString() });
				}
			  }
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, _mySession.UserId);
		}

		return lstFleetModels;
	  }

	  public List<GetFleetModelsAllResult> GetFleetModelsAll()
	  {
		try
		{
		    using (this.objDataContext = GetDataContext())
		    {
			  List<GetFleetModelsAllResult> lstFleetModelsAll = this.objDataContext.GetFleetModelsAll().ToList();
			  return lstFleetModelsAll;
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, _mySession.UserId);
		    return null;
		}
	  }

	  public ClsFleetModels GetFleetModelsByFleetModelsId(long lgFleetModelsId)
	  {
		ClsFleetModels objClsFleetModels = new ClsFleetModels();
		try
		{
		    using (this.objDataContext = GetDataContext())
		    {
			  GetFleetModelsByIdResult item = this.objDataContext.GetFleetModelsById(lgFleetModelsId).ToList().FirstOrDefault();
			  if (item != null)
			  {
				objClsFleetModels.lgId = item.Id;
				objClsFleetModels.strFleetModelsName = item.FleetModelsName;
			  }
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, _mySession.UserId);
		}

		return objClsFleetModels;
	  }

	  public bool IsFleetModelsExists(long lgFleetModelsId, string strFleetModelsName)
	  {
		try
		{
		    using (this.objDataContext = GetDataContext())
		    {
			  if (this.objDataContext.FleetModels.Where(x => x.Id != lgFleetModelsId && x.Model == strFleetModelsName && x.IsDeleted == false).Count() > 0)
			  {
				return true;
			  }
		    }

		    return false;
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, _mySession.UserId);
		    return false;
		}
	  }

	  public long SaveFleetModels(ClsFleetModels objSave)
	  {
		try
		{
		    using (TransactionScope scope = new TransactionScope())
		    {
			  using (this.objDataContext = GetDataContext())
			  {
				var result = this.objDataContext.InsertOrUpdateFleetModels(objSave.lgId, objSave.strFleetModelsName, _mySession.UserId, PageMaster.FleetModels).FirstOrDefault();
				if (result != null)
				{
				    objSave.lgId = result.InsertedId;
				}
			  }

			  objDataContext.SubmitChanges();
			  scope.Complete();
		    }

		    return objSave.lgId;
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, _mySession.UserId);
		    return 0;
		}
	  }

	  public List<SearchFleetModelsResult> SearchFleetModels(int inRow, int inPage, string strSearch, string strSort)
	  {
		try
		{
		    using (this.objDataContext = GetDataContext())
		    {
			  List<SearchFleetModelsResult> lstSearchFleetModels = this.objDataContext.SearchFleetModels(inRow, inPage, strSearch, strSort).ToList();
			  return lstSearchFleetModels;
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetModels, _mySession.UserId);
		    return null;
		}
	  }
    }
}