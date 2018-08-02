using FleetManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FleetManager.Service.Fleet
{
    public class FleetService:IFleetService
    {
	  private readonly IClsCarFleet _carFleet;
	  private readonly IClsFleetColors _fleetColors;
	  private readonly IClsFleetMakes _fleetMakes;
	  private readonly IClsFleetModels _models;

	  public FleetService(IClsCarFleet carFleet, IClsFleetColors fleetColors, IClsFleetMakes fleetMakes, IClsFleetModels models)
	  {
		_carFleet = carFleet;
		_fleetColors = fleetColors;
		_fleetMakes = fleetMakes;
		_models = models;
	  }

	  public DeleteCarFleetResult DeleteCarFleet(string carFleetId, int userId)
	  {
		return _carFleet.DeleteCarFleet(carFleetId, userId);
	  }

	  public IClsCarFleet GetCarFleetByCarFleetId(long carFleetId)
	  {
		return _carFleet.GetCarFleetByCarFleetId(carFleetId);
	  }

	  public int SaveCarFleet(IClsCarFleet carFleet)
	  {
		return _carFleet.SaveCarFleet(carFleet as ClsCarFleet);
	  }

	  public List<SearchCarFleetResult> SearchCarFleet(int rows, int page, string search, string v, string tripstartdate, string tripenddate)
	  {
		return _carFleet.SearchCarFleet(rows, page, search, v, tripstartdate, tripenddate);
	  }

	  public List<SelectListItem> GetAllFleetColorsForDropDown()
	  {
		return _fleetColors.GetAllFleetColorsForDropDown();
	  }

	  public List<SelectListItem> GetAllFleetMakesForDropDown()
	  {
		return _fleetMakes.GetAllFleetMakesForDropDown();
	  }

	  public List<SelectListItem> GetAllFleetModelsForDropDown()
	  {
		return _models.GetAllFleetModelsForDropDown();
	  }

	  public List<SearchFleetMakesResult> SearchFleetMakes(int inNoOfRows, int v1, string strSearchValue, string v2)
	  {
		return _fleetMakes.SearchFleetMakes(inNoOfRows, v1, strSearchValue, v2);
	  }

	  public List<SearchFleetModelsResult> SearchFleetModels(int inNoOfRows, int v1, string strSearchValue, string v2)
	  {
		return _models.SearchFleetModels(inNoOfRows, v1, strSearchValue, v2);
	  }

	  public List<SearchFleetColorsResult> SearchFleetColors(int inNoOfRows, int v1, string strSearchValue, string v2)
	  {
		return _fleetColors.SearchFleetColors(inNoOfRows, v1, strSearchValue, v2);
	  }

	  public DeleteFleetColorsResult DeleteFleetColors(string strFleetColorsId, int userId)
	  {
		return _fleetColors.DeleteFleetColors(strFleetColorsId, userId);
	  }

	  public IClsFleetColors GetFleetColorsByFleetColorsId(long lgFleetColorsId)
	  {
		return _fleetColors.GetFleetColorsByFleetColorsId(lgFleetColorsId);
	  }

	  public bool IsFleetColorsExists(long lgId, string strFleetColorsName)
	  {
		return _fleetColors.IsFleetColorsExists(lgId, strFleetColorsName);
	  }

	  public long SaveFleetColors(IClsFleetColors objFleetColors)
	  {
		return _fleetColors.SaveFleetColors(objFleetColors as ClsFleetColors);
	  }

	  public DeleteFleetMakesResult DeleteFleetMakes(string strFleetMakesId, int userId)
	  {
		return _fleetMakes.DeleteFleetMakes(strFleetMakesId, userId);
	  }

	  public IClsFleetMakes GetFleetMakesByFleetMakesId(long lgFleetMakesId)
	  {
		return _fleetMakes.GetFleetMakesByFleetMakesId(lgFleetMakesId);
	  }

	  public bool IsFleetMakesExists(long lgId, string strFleetMakesName)
	  {
		return _fleetMakes.IsFleetMakesExists(lgId, strFleetMakesName);
	  }

	  public long SaveFleetMakes(IClsFleetMakes objFleetMakes)
	  {
		return _fleetMakes.SaveFleetMakes(objFleetMakes as ClsFleetMakes);
	  }

	  public DeleteFleetModelsResult DeleteFleetModels(string strFleetModelsId, int userId)
	  {
		return _models.DeleteFleetModels(strFleetModelsId, userId);
	  }

	  public IClsFleetModels GetFleetModelsByFleetModelsId(long lgFleetModelsId)
	  {
		return _models.GetFleetModelsByFleetModelsId(lgFleetModelsId);
	  }

	  public bool IsFleetModelsExists(long lgId, string strFleetModelsName)
	  {
		return _models.IsFleetModelsExists(lgId, strFleetModelsName);
	  }

	  public long SaveFleetModels(IClsFleetModels objFleetModels)
	  {
		return _models.SaveFleetModels(objFleetModels as ClsFleetModels);
	  }
    }
}
