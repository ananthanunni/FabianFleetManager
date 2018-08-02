using System.Collections.Generic;
using System.Web.Mvc;
using FleetManager.Data.Models;

namespace FleetManager.Service.Tracking
{
    public class TrackerService:ITrackerService
    {
	  private readonly IClsTracker _tracker;
	  private readonly IClsTripReason _tripReason;

	  public TrackerService(IClsTracker tracker,IClsTripReason tripReason)
	  {
		_tracker = tracker;
		_tripReason = tripReason;
	  }

	  public DeleteTripReasonResult DeleteTripReason(string strTripReasonId, int userId)
	  {
		return _tripReason.DeleteTripReason(strTripReasonId, userId);
	  }

	  public List<SelectListItem> GetAllTripReasonForDropDown()
	  {
		return _tripReason.GetAllTripReasonForDropDown();
	  }

	  public IClsTracker GetTrackerByTrackerId(long lgTrackerId)
	  {
		return _tracker.GetTrackerByTrackerId(lgTrackerId);
	  }

	  public IClsTripReason GetTripReasonByTripReasonId(long lgTripReasonId)
	  {
		return _tripReason.GetTripReasonByTripReasonId(lgTripReasonId);
	  }

	  public bool IsTripReasonExists(long lgId, string strTripReasonName)
	  {
		return _tripReason.IsTripReasonExists(lgId, strTripReasonName);
	  }

	  public int SaveTracker(IClsTracker objTracker)
	  {
		return _tracker.SaveTracker(objTracker);
	  }

	  public long SaveTripReason(IClsTripReason objTripReason)
	  {
		return _tripReason.SaveTripReason(objTripReason);
	  }

	  public List<SearchTrackerResult> SearchTracker(int inNoOfRows, int v1, string strSearchValue, string v2, string empty1, string empty2, string empty3, string empty4)
	  {
		return _tracker.SearchTracker(inNoOfRows, v1, strSearchValue, v2, empty1, empty2, empty3, empty4);
	  }

	  public List<SearchTripReasonResult> SearchTripReason(int inNoOfRows, int v1, string strSearchValue, string v2)
	  {
		return _tripReason.SearchTripReason(inNoOfRows, v1, strSearchValue, v2);
	  }
    }
}
