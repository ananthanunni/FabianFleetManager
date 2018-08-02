using System.Collections.Generic;
using System.Web.Mvc;
using FleetManager.Data.Models;

namespace FleetManager.Service.Fleet
{
    public interface IFleetService
    {
	  List<SearchCarFleetResult> SearchCarFleet(int rows, int page, string search, string v, string tripstartdate, string tripenddate);
	  DeleteCarFleetResult DeleteCarFleet(string carFleetId, int userId);
	  IClsCarFleet GetCarFleetByCarFleetId(long carFleetId);
	  int SaveCarFleet(IClsCarFleet objCarFleet);
	  List<SelectListItem> GetAllFleetColorsForDropDown();
	  List<SelectListItem> GetAllFleetMakesForDropDown();
	  List<SelectListItem> GetAllFleetModelsForDropDown();
	  List<SearchFleetMakesResult> SearchFleetMakes(int inNoOfRows, int v1, string strSearchValue, string v2);
	  List<SearchFleetModelsResult> SearchFleetModels(int inNoOfRows, int v1, string strSearchValue, string v2);
	  List<SearchFleetColorsResult> SearchFleetColors(int inNoOfRows, int v1, string strSearchValue, string v2);
	  DeleteFleetColorsResult DeleteFleetColors(string strFleetColorsId, int userId);
	  IClsFleetColors GetFleetColorsByFleetColorsId(long lgFleetColorsId);
	  bool IsFleetColorsExists(long lgId, string strFleetColorsName);
	  long SaveFleetColors(IClsFleetColors objFleetColors);
	  DeleteFleetMakesResult DeleteFleetMakes(string strFleetMakesId, int userId);
	  IClsFleetMakes GetFleetMakesByFleetMakesId(long lgFleetMakesId);
	  bool IsFleetMakesExists(long lgId, string strFleetMakesName);
	  long SaveFleetMakes(IClsFleetMakes objFleetMakes);
	  DeleteFleetModelsResult DeleteFleetModels(string strFleetModelsId, int userId);
	  IClsFleetModels GetFleetModelsByFleetModelsId(long lgFleetModelsId);
	  bool IsFleetModelsExists(long lgId, string strFleetModelsName);
	  long SaveFleetModels(IClsFleetModels objFleetModels);
    }
}