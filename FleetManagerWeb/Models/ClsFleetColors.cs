namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public partial class ClsFleetColors : IClsFleetColors
    {
        private FleetColorsDataContext objDataContext = null;

        public bool hdniFrame { get; set; }

        public long lgId { get; set; }

        public List<ClsFleetColors> listFleetColors { get; set; }

        public string strFleetColorsName { get; set; }

        public DeleteFleetColorsResult DeleteFleetColors(string strFleetColorsIdList, long lgDeletedBy)
        {
            DeleteFleetColorsResult result = new DeleteFleetColorsResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new FleetColorsDataContext(Functions.StrConnection))
                    {
                        result = this.objDataContext.DeleteFleetColors(strFleetColorsIdList, lgDeletedBy, PageMaster.FleetColors).ToList().FirstOrDefault();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
            }

            return result;
        }

        public List<SelectListItem> GetAllFleetColorsForDropDown()
        {
            List<SelectListItem> lstFleetColors = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new FleetColorsDataContext(Functions.StrConnection))
                {
                    lstFleetColors.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetFleetColorsAllResult> lstFleetColorsResult = this.objDataContext.GetFleetColorsAll().ToList();
                    if (lstFleetColorsResult != null && lstFleetColorsResult.Count > 0)
                    {
                        foreach (var item in lstFleetColorsResult)
                        {
                            lstFleetColors.Add(new SelectListItem { Text = item.FleetColorsName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
            }

            return lstFleetColors;
        }

        public List<GetFleetColorsAllResult> GetFleetColorsAll()
        {
            try
            {
                using (this.objDataContext = new FleetColorsDataContext(Functions.StrConnection))
                {
                    List<GetFleetColorsAllResult> lstFleetColorsAll = this.objDataContext.GetFleetColorsAll().ToList();
                    return lstFleetColorsAll;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return null;
            }
        }

        public ClsFleetColors GetFleetColorsByFleetColorsId(long lgFleetColorsId)
        {
            ClsFleetColors objClsFleetColors = new ClsFleetColors();
            try
            {
                using (this.objDataContext = new FleetColorsDataContext(Functions.StrConnection))
                {
                    GetFleetColorsByIdResult item = this.objDataContext.GetFleetColorsById(lgFleetColorsId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objClsFleetColors.lgId = item.Id;
                        objClsFleetColors.strFleetColorsName = item.FleetColorsName;
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
            }

            return objClsFleetColors;
        }

        public bool IsFleetColorsExists(long lgFleetColorsId, string strFleetColorsName)
        {
            try
            {
                using (this.objDataContext = new FleetColorsDataContext(Functions.StrConnection))
                {
                    if (this.objDataContext.FleetColors.Where(x => x.Id != lgFleetColorsId && x.FleetColorsName == strFleetColorsName && x.IsDeleted == false).Count() > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return false;
            }
        }

        public long SaveFleetColors(ClsFleetColors objSave)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new FleetColorsDataContext(Functions.StrConnection))
                    {
                        var result = this.objDataContext.InsertOrUpdateFleetColors(objSave.lgId, objSave.strFleetColorsName, mySession.Current.UserId, PageMaster.FleetColors).FirstOrDefault();
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return 0;
            }
        }

        public List<SearchFleetColorsResult> SearchFleetColors(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new FleetColorsDataContext(Functions.StrConnection))
                {
                    List<SearchFleetColorsResult> lstSearchFleetColors = this.objDataContext.SearchFleetColors(inRow, inPage, strSearch, strSort).ToList();
                    return lstSearchFleetColors;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.FleetColors, mySession.Current.UserId);
                return null;
            }
        }
    }
}