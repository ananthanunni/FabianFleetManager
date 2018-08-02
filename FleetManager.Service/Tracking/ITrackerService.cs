using System.Collections.Generic;
using System.Web.Mvc;
using FleetManager.Data.Models;

namespace FleetManager.Service.Tracking
{
    public interface ITrackerService
    {
	  List<SearchTripReasonResult> SearchTripReason(int inNoOfRows, int v1, string strSearchValue, string v2);
	  List<SearchTrackerResult> SearchTracker(int rows, int page, string search, string v, string tripstartdate, string tripenddate, string locationstart, string locationend);
	  IClsTracker GetTrackerByTrackerId(long lgTrackerId);
	  int SaveTracker(IClsTracker objTracker);
	  DeleteTripReasonResult DeleteTripReason(string strTripReasonId, int userId);
	  List<SelectListItem> GetAllTripReasonForDropDown();
	  IClsTripReason GetTripReasonByTripReasonId(long lgTripReasonId);
	  bool IsTripReasonExists(long lgId, string strTripReasonName);
	  long SaveTripReason(IClsTripReason objTripReason);
    }
}