namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsFleetColors
    {
        DeleteFleetColorsResult DeleteFleetColors(string strFleetColorsList, long lgDeletedBy);

        List<SelectListItem> GetAllFleetColorsForDropDown();

        List<GetFleetColorsAllResult> GetFleetColorsAll();

        ClsFleetColors GetFleetColorsByFleetColorsId(long lgFleetColorsId);

        bool IsFleetColorsExists(long lgFleetColorsId, string strFleetColorsName);

        long SaveFleetColors(ClsFleetColors objSave);

        List<SearchFleetColorsResult> SearchFleetColors(int inRow, int inPage, string strSearch, string strSort);
    }
}