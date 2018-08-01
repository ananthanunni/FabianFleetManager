namespace FleetManager.Data.Models
{
    using FleetManager.Data.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsCarFleet
    {
        List<GetCarFleetAllResult> GetCarFleetAll();

        ClsCarFleet GetCarFleetByCarFleetId(long lgCarFleetId);

        int SaveCarFleet(ClsCarFleet objSave);

        List<SearchCarFleetResult> SearchCarFleet(int inRow, int inPage, string strSearch, string strSort, string strTripStartDate, string strTripEndDate);
        DeleteCarFleetResult DeleteCarFleet(string strCarFleetId, long lgDeletedBy);
    }
}