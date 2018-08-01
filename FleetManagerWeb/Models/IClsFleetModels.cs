namespace FleetManagerWeb.Models
{
    using FleetManagerWeb.Models;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IClsFleetModels
    {
        DeleteFleetModelsResult DeleteFleetModels(string strFleetModelsList, long lgDeletedBy);

        List<SelectListItem> GetAllFleetModelsForDropDown();

        List<GetFleetModelsAllResult> GetFleetModelsAll();

        ClsFleetModels GetFleetModelsByFleetModelsId(long lgFleetModelsId);

        bool IsFleetModelsExists(long lgFleetModelsId, string strFleetModelsName);

        long SaveFleetModels(ClsFleetModels objSave);

        List<SearchFleetModelsResult> SearchFleetModels(int inRow, int inPage, string strSearch, string strSort);
    }
}