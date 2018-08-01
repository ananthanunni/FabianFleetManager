namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsTripReason
    {
        DeleteTripReasonResult DeleteTripReason(string strTripReasonList, long lgDeletedBy);

        List<SelectListItem> GetAllTripReasonForDropDown();

        List<GetTripReasonAllResult> GetTripReasonAll();

        ClsTripReason GetTripReasonByTripReasonId(long lgTripReasonId);

        bool IsTripReasonExists(long lgTripReasonId, string strTripReasonName);

        long SaveTripReason(ClsTripReason objSave);

        List<SearchTripReasonResult> SearchTripReason(int inRow, int inPage, string strSearch, string strSort);
    }
}