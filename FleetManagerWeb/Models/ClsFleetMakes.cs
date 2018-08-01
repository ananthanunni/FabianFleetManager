namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public partial class ClsFleetMakes : DataContextEntity<FleetMakesDataContext>, IClsFleetMakes
    {
	  public ClsFleetMakes(FleetMakesDataContext context = null) : base(context)
	  {
	  }

	  public bool hdniFrame { get; set; }

        public long lgId { get; set; }

        public List<ClsFleetMakes> listFleetMakes { get; set; }

        public string strFleetMakesName { get; set; }

        public DeleteFleetMakesResult DeleteFleetMakes(string strFleetMakesIdList, long lgDeletedBy)
        {
            DeleteFleetMakesResult result = new DeleteFleetMakesResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new FleetMakesDataContext(Functions.StrConnection))
                    {
                        result = this.objDataContext.DeleteFleetMakes(strFleetMakesIdList, lgDeletedBy, PageMaster.FleetMakes).ToList().FirstOrDefault();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, mySession.Current.UserId);
            }

            return result;
        }

        public List<SelectListItem> GetAllFleetMakesForDropDown()
        {
            List<SelectListItem> lstFleetMakes = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new FleetMakesDataContext(Functions.StrConnection))
                {
                    lstFleetMakes.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetFleetMakesAllResult> lstFleetMakesResult = this.objDataContext.GetFleetMakesAll().ToList();
                    if (lstFleetMakesResult != null && lstFleetMakesResult.Count > 0)
                    {
                        foreach (var item in lstFleetMakesResult)
                        {
                            lstFleetMakes.Add(new SelectListItem { Text = item.FleetMakesName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, mySession.Current.UserId);
            }

            return lstFleetMakes;
        }

        public List<GetFleetMakesAllResult> GetFleetMakesAll()
        {
            try
            {
                using (this.objDataContext = new FleetMakesDataContext(Functions.StrConnection))
                {
                    List<GetFleetMakesAllResult> lstFleetMakesAll = this.objDataContext.GetFleetMakesAll().ToList();
                    return lstFleetMakesAll;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, mySession.Current.UserId);
                return null;
            }
        }

        public ClsFleetMakes GetFleetMakesByFleetMakesId(long lgFleetMakesId)
        {
            ClsFleetMakes objClsFleetMakes = new ClsFleetMakes();
            try
            {
                using (this.objDataContext = new FleetMakesDataContext(Functions.StrConnection))
                {
                    GetFleetMakesByIdResult item = this.objDataContext.GetFleetMakesById(lgFleetMakesId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objClsFleetMakes.lgId = item.Id;
                        objClsFleetMakes.strFleetMakesName = item.FleetMakesName;
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, mySession.Current.UserId);
            }

            return objClsFleetMakes;
        }

        public bool IsFleetMakesExists(long lgFleetMakesId, string strFleetMakesName)
        {
            try
            {
                using (this.objDataContext = new FleetMakesDataContext(Functions.StrConnection))
                {
                    if (this.objDataContext.FleetMakes.Where(x => x.Id != lgFleetMakesId && x.Make == strFleetMakesName && x.IsDeleted == false).Count() > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, mySession.Current.UserId);
                return false;
            }
        }

        public long SaveFleetMakes(ClsFleetMakes objSave)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new FleetMakesDataContext(Functions.StrConnection))
                    {
                        var result = this.objDataContext.InsertOrUpdateFleetMakes(objSave.lgId, objSave.strFleetMakesName, mySession.Current.UserId, PageMaster.FleetMakes).FirstOrDefault();
                        if (result != null)
                        {
                            objSave.lgId = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return objSave.lgId;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, mySession.Current.UserId);
                return 0;
            }
        }

        public List<SearchFleetMakesResult> SearchFleetMakes(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new FleetMakesDataContext(Functions.StrConnection))
                {
                    List<SearchFleetMakesResult> lstSearchFleetMakes = this.objDataContext.SearchFleetMakes(inRow, inPage, strSearch, strSort).ToList();
                    return lstSearchFleetMakes;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetMakes, mySession.Current.UserId);
                return null;
            }
        }
    }
}