using System.Collections.Generic;

namespace FleetManager.Data.Models
{
    public interface IClsTracker
    {
	  List<GetTrackerAllResult> GetTrackerAll();

	  ClsTracker GetTrackerByTrackerId(long lgTrackerId);

	  int SaveTracker(IClsTracker objSave);

	  List<SearchTrackerResult> SearchTracker(int inRow, int inPage, string strSearch, string strSort, string strTripStartDate, string strTripEndDate, string strLocationStart, string strLocationEnd);
    }
}