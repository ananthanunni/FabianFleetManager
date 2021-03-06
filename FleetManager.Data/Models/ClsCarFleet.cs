﻿namespace FleetManager.Data.Models
{
    using FleetManager.Core.Common;
    using FleetManager.Core.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;

    public partial class ClsCarFleet : DataContextEntity<CarFleetDataContext>, IClsCarFleet
    {
	  public ClsCarFleet()
	  {

	  }

	  public ClsCarFleet(CarFleetDataContext context):base(context)
	  {

	  }
        
        public bool hdniFrame { get; set; }
        public int inId { get; set; }
        public int inOwner_Id { get; set; }
        public string strCode { get; set; }
        public string strReg { get; set; }
        public string strDesc { get; set; }
        public int inColor_Id { get; set; }
        public string strFuel_Type { get; set; }
        public string strLast_Trip { get; set; }
        public int inLast_Km { get; set; }
        public string strLast_Location { get; set; }
        public string strModel { get; set; }
        public string strMake { get; set; }
        public int inFleetModels_Id { get; set; }
        public int inFleetMakes_Id { get; set; }
        public List<ClsCarFleet> listCarFleet { get; set; }
        public List<SelectListItem> lstFleetColors { get; set; }
        public List<SelectListItem> lstFleetMakes { get; set; }
        public List<SelectListItem> lstFleetModels { get; set; }
        public List<GetCarFleetAllResult> GetCarFleetAll()
        {
            try
            {
                using (this.objDataContext = GetDataContext())
                {
                    List<GetCarFleetAllResult> lstCarFleetAll = this.objDataContext.GetCarFleetAll().ToList();
                    return lstCarFleetAll;
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, _mySession.UserId);
                return null;
            }
        }
        public ClsCarFleet GetCarFleetByCarFleetId(long lgCarFleetId)
        {
            ClsCarFleet objClsCarFleet = new ClsCarFleet();
            try
            {
                using (this.objDataContext = GetDataContext())
                {
                    GetCarFleetByIdResult item = this.objDataContext.GetCarFleetById(lgCarFleetId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objClsCarFleet.inId = item.ID;
                        objClsCarFleet.inOwner_Id = item.Owner_Id;
                        objClsCarFleet.strLast_Trip = item.Last_Trip.Replace(' ', '/');
                        objClsCarFleet.strCode = item.Code;
                        objClsCarFleet.strReg = item.Reg;
                        objClsCarFleet.strDesc = item.Description;
                        objClsCarFleet.inColor_Id = item.Color_Id;
                        objClsCarFleet.strFuel_Type = item.Fuel_Type;
                        objClsCarFleet.inLast_Km = item.Last_Km;
                        objClsCarFleet.strLast_Location = item.Last_Location;
                        objClsCarFleet.strMake = item.Make;
                        objClsCarFleet.inFleetModels_Id = System.Convert.ToInt32(item.FleetModels_Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, _mySession.UserId);
            }

            return objClsCarFleet;
        }
        public int SaveCarFleet(ClsCarFleet objSave)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = GetDataContext())
                    {
                        var result = this.objDataContext.InsertOrUpdateCarFleet(objSave.inId,objSave.inOwner_Id, objSave.strCode, objSave.strReg, objSave.strDesc, objSave.inColor_Id, 
                            objSave.strFuel_Type, objSave.strLast_Trip, objSave.inLast_Km, objSave.strLast_Location,objSave.inFleetModels_Id, objSave.inFleetMakes_Id, _mySession.UserId, PageMaster.CarFleet.ToString().IntSafe(), null, null).FirstOrDefault();

                        if (result != null)
                        {
                            objSave.inId = result.InsertedId;
                        }
                    }

			  objDataContext.SubmitChanges();
                    scope.Complete();
                }

                return objSave.inId;
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, _mySession.UserId);
                return 0;
            }
        }
        public List<SearchCarFleetResult> SearchCarFleet(int inRow, int inPage, string strSearch, string strSort, string strTripStartDate, string strTripEndDate)
        {
            try
            {
                using (this.objDataContext = GetDataContext())
                {
                    List<SearchCarFleetResult> lstSearchCarFleet = this.objDataContext.SearchCarFleet(inRow, inPage, strSort, strTripStartDate, strTripEndDate, strSearch).ToList();
                    return lstSearchCarFleet;
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.CarFleet, _mySession.UserId);
                return null;
            }
        }
        public DeleteCarFleetResult DeleteCarFleet(string strCarFleetId, long lgDeletedBy)
        {
            try
            {
                using (this.objDataContext = GetDataContext())
                {
                    DeleteCarFleetResult result = this.objDataContext.DeleteCarFleet(strCarFleetId, lgDeletedBy, PageMaster.User).FirstOrDefault();
                    if (result == null)
                    {
                        result = new DeleteCarFleetResult();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.User, _mySession.UserId);
                return null;
            }
        }
    }
}