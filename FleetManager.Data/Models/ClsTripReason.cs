namespace FleetManager.Data.Models
{
    using FleetManager.Core.Common;
    using FleetManager.Data.Models;
    using FleetManagerWeb.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;

    public partial class ClsTripReason : DataContextEntity<TripReasonDataContext>, IClsTripReason
    {
	  public ClsTripReason(TripReasonDataContext context = null) : base(context)
	  {
	  }

	  public bool hdniFrame { get; set; }

        public long lgId { get; set; }

        public List<ClsTripReason> listTripReason { get; set; }

        public string strTripReasonName { get; set; }

        public DeleteTripReasonResult DeleteTripReason(string strTripReasonIdList, long lgDeletedBy)
        {
            DeleteTripReasonResult result = new DeleteTripReasonResult();
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext =GetDataContext())
                    {
                        result = this.objDataContext.DeleteTripReason(strTripReasonIdList, lgDeletedBy, PageMaster.TripReason).ToList().FirstOrDefault();
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
            }

            return result;
        }

        public List<SelectListItem> GetAllTripReasonForDropDown()
        {
            List<SelectListItem> lstTripReason = new List<SelectListItem>();
            try
            {
                using (this.objDataContext =GetDataContext())
                {
                    lstTripReason.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetTripReasonAllResult> lstTripReasonResult = this.objDataContext.GetTripReasonAll().ToList();
                    if (lstTripReasonResult != null && lstTripReasonResult.Count > 0)
                    {
                        foreach (var item in lstTripReasonResult)
                        {
                            lstTripReason.Add(new SelectListItem { Text = item.TripReasonName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
            }

            return lstTripReason;
        }

        public List<GetTripReasonAllResult> GetTripReasonAll()
        {
            try
            {
                using (this.objDataContext =GetDataContext())
                {
                    List<GetTripReasonAllResult> lstTripReasonAll = this.objDataContext.GetTripReasonAll().ToList();
                    return lstTripReasonAll;
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
                return null;
            }
        }

        public ClsTripReason GetTripReasonByTripReasonId(long lgTripReasonId)
        {
            ClsTripReason objClsTripReason = new ClsTripReason();
            try
            {
                using (this.objDataContext =GetDataContext())
                {
                    GetTripReasonByIdResult item = this.objDataContext.GetTripReasonById(lgTripReasonId).ToList().FirstOrDefault();
                    if (item != null)
                    {
                        objClsTripReason.lgId = item.Id;
                        objClsTripReason.strTripReasonName = item.TripReasonName;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
            }

            return objClsTripReason;
        }

        public bool IsTripReasonExists(long lgTripReasonId, string strTripReasonName)
        {
            try
            {
                using (this.objDataContext =GetDataContext())
                {
                    if (this.objDataContext.TripReason.Where(x => x.Id != lgTripReasonId && x.TripReasonName == strTripReasonName && x.IsDeleted == false).Count() > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
                return false;
            }
        }

        public long SaveTripReason(ClsTripReason objSave)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext =GetDataContext())
                    {
                        var result = this.objDataContext.InsertOrUpdateTripReason(objSave.lgId, objSave.strTripReasonName, _mySession.UserId, PageMaster.TripReason).FirstOrDefault();
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
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
                return 0;
            }
        }

        public List<SearchTripReasonResult> SearchTripReason(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext =GetDataContext())
                {
                    List<SearchTripReasonResult> lstSearchTripReason = this.objDataContext.SearchTripReason(inRow, inPage, strSearch, strSort).ToList();
                    return lstSearchTripReason;
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, _mySession.UserId);
                return null;
            }
        }
    }
}