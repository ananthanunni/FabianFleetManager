using System.Collections.Generic;
using System.Web.Mvc;

namespace FleetManager.Data.Models
{
    public interface IClsTripReason
    {
	  DeleteTripReasonResult DeleteTripReason(string strTripReasonList, long lgDeletedBy);

	  List<SelectListItem> GetAllTripReasonForDropDown();

	  List<GetTripReasonAllResult> GetTripReasonAll();

	  IClsTripReason GetTripReasonByTripReasonId(long lgTripReasonId);

	  bool IsTripReasonExists(long lgTripReasonId, string strTripReasonName);

	  long SaveTripReason(IClsTripReason objSave);

	  List<SearchTripReasonResult> SearchTripReason(int inRow, int inPage, string strSearch, string strSort);
    }
}