using FleetManager.Core.Common;
using FleetManager.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FleetManager.Data.Models
{
    public partial class ClsTracker : DataContextEntity<TrackerDataContext>, IClsTracker
    {
	  public ClsTracker()
	  {

	  }
	  public ClsTracker(TrackerDataContext context) : base(context)
	  {
	  }

	  public bool hdniFrame { get; set; }

	  public int inId { get; set; }

	  public List<ClsTracker> listTracker { get; set; }

	  public string strTripStart { get; set; }

	  public string strTripEnd { get; set; }

	  public string strLocationStart { get; set; }

	  public string strLocationEnd { get; set; }

	  public string strReasonRemarks { get; set; }

	  public int inKmStart { get; set; }

	  public int inKmEnd { get; set; }

	  public int inKmDriven { get; set; }

	  public int inFuelStart { get; set; }

	  public int inFuelEnd { get; set; }

	  public int inUserId { get; set; }

	  public string strEntryDatetime { get; set; }

	  public string strEntryMethod { get; set; }

	  public bool blEditable { get; set; }

	  public bool blActive { get; set; }



	  public List<GetTrackerAllResult> GetTrackerAll()
	  {
		try
		{
		    using (this.objDataContext = GetDataContext())
		    {
			  List<GetTrackerAllResult> lstTrackerAll = this.objDataContext.GetTrackerAll().ToList();
			  return lstTrackerAll;
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, _mySession.UserId);
		    return null;
		}
	  }

	  public ClsTracker GetTrackerByTrackerId(long lgTrackerId)
	  {
		ClsTracker objClsTracker = new ClsTracker();
		try
		{
		    using (this.objDataContext = GetDataContext())
		    {
			  GetTrackerByIdResult item = this.objDataContext.GetTrackerById(lgTrackerId).ToList().FirstOrDefault();
			  if (item != null)
			  {
				objClsTracker.inId = item.ID;
				objClsTracker.strTripStart = item.Trip_Start.Replace(' ', '/');
				objClsTracker.strTripEnd = item.Trip_End.Replace(' ', '/');
				objClsTracker.strLocationStart = item.Location_Start;
				objClsTracker.strLocationEnd = item.Location_End;
				objClsTracker.strReasonRemarks = item.Reason_Remarks;
				objClsTracker.inKmStart = item.Km_Start;
				objClsTracker.inKmEnd = item.Km_End;
				objClsTracker.inKmDriven = item.Km_Driven;
				objClsTracker.inFuelStart = item.Fuel_Start;
				objClsTracker.inFuelEnd = item.Fuel_End;
				objClsTracker.inUserId = item.User_Id;
				objClsTracker.strEntryDatetime = item.Entry_Datetime.Replace(' ', '/');
				objClsTracker.strEntryMethod = item.Entry_Method;
				objClsTracker.blEditable = item.Editable;
				objClsTracker.blActive = item.Active;
			  }
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, _mySession.UserId);
		}

		return objClsTracker;
	  }

	  public int SaveTracker(ClsTracker objSave)
	  {
		try
		{
		    using (TransactionScope scope = new TransactionScope())
		    {
			  using (this.objDataContext = GetDataContext())
			  {
				var result = this.objDataContext.InsertOrUpdateTracker(objSave.inId, objSave.strTripStart, objSave.strTripEnd, objSave.strLocationStart, objSave.strLocationEnd, objSave.strReasonRemarks, objSave.inKmStart, objSave.inKmEnd, objSave.inKmDriven, objSave.inFuelStart, objSave.inFuelEnd, _mySession.UserId, objSave.strEntryDatetime, objSave.strEntryMethod, true, true, PageMaster.Tracker.ToString().IntSafe(), null, null, null, null).FirstOrDefault();
				if (result != null)
				{
				    objSave.inId = result.InsertedId;
				}
			  }

			  scope.Complete();
		    }

		    return objSave.inId;
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, _mySession.UserId);
		    return 0;
		}
	  }

	  public List<SearchTrackerResult> SearchTracker(int inRow, int inPage, string strSearch, string strSort, string strTripStartDate, string strTripEndDate, string strLocationStart, string strLocationEnd)
	  {
		try
		{
		    using (this.objDataContext = GetDataContext())
		    {
			  List<SearchTrackerResult> lstSearchTracker = this.objDataContext.SearchTracker(inRow, inPage, strSort, strTripStartDate, strTripEndDate, strLocationStart, strLocationEnd, strSearch).ToList();
			  return lstSearchTracker;
		    }
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Tracker, _mySession.UserId);
		    return null;
		}
	  }
    }
}