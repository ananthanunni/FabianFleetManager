namespace FleetManager.Data.Models
{
    using FleetManager.Core.Common;
    using FleetManager.Core.Extensions;
    using FleetManagerWeb.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;

    public partial class ClsRole : DataContextEntity<RoleDataContext>,IClsRole
    {
	  public ClsRole()
	  {

	  }
	  public ClsRole(RoleDataContext context) : base(context)
	  {
	  }

	  public bool blAllowDispatchBackDateEntry { get; set; }

        public bool blAllowKilometerLimit { get; set; }

        public bool hdniFrame { get; set; }

        public long lgId { get; set; }

        public List<ClsRole> listRole { get; set; }

        public string strDescription { get; set; }

        public string strRights { get; set; }

        public string strRoleName { get; set; }

        public DeleteRoleResult DeleteRole(string strRoleIdList, long lgDeletedBy)
        {
            try
            {
                using (this.objDataContext =GetDataContext())
                {
                    DeleteRoleResult result = this.objDataContext.DeleteRole(strRoleIdList, lgDeletedBy, PageMaster.Role).FirstOrDefault();
                    if (result == null)
                    {
                        result = new DeleteRoleResult();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, _mySession.UserId);
                return null;
            }
        }

        public List<SelectListItem> GetAllRoleForDropDown()
        {
            List<SelectListItem> lstRole = new List<SelectListItem>();
            try
            {
                using (this.objDataContext =GetDataContext())
                {
                    lstRole.Add(new SelectListItem { Text = "--Select--", Value = string.Empty });
                    List<GetRoleAllResult> lstRoleResult = this.objDataContext.GetRoleAll().ToList();
                    if (lstRoleResult.Count > 0)
                    {
                        foreach (var item in lstRoleResult)
                        {
                            lstRole.Add(new SelectListItem { Text = item.RoleName, Value = item.Id.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {		    
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, _mySession.UserId);
            }

            return lstRole;
        }

        public ClsRole GetRoleByRoleId(long lgRoleId)
        {
            ClsRole objClsRole = new ClsRole();
            try
            {
                using (this.objDataContext =GetDataContext())
                {
                    GetRoleByIdResult item = this.objDataContext.GetRoleById(lgRoleId).FirstOrDefault();
                    if (item != null)
                    {
                        objClsRole.lgId = item.Id;
                        objClsRole.strRoleName = item.RoleName;
                        objClsRole.strDescription = item.DESCRIPTION;
                        objClsRole.blAllowKilometerLimit = Convert.ToBoolean(item.AllowKilometerLimit);
                        objClsRole.blAllowDispatchBackDateEntry = Convert.ToBoolean(item.AllowDispatchBackDateEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, _mySession.UserId);
            }

            return objClsRole;
        }

        public bool IsRoleExists(long lgRoleId, string strRoleName)
        {
            try
            {
                using (this.objDataContext =GetDataContext())
                {
                    if (this.objDataContext.Roles.Where(x => x.Id != lgRoleId && x.RoleName == strRoleName && x.IsDeleted == false).Count() > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, _mySession.UserId);
                return false;
            }
        }

        public long SaveRole(ClsRole objSave)
        {
            try
            {
                long roleId = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext =GetDataContext())
                    {
                        var result = this.objDataContext.InsertOrUpdateRole(objSave.lgId, objSave.strRoleName, objSave.strDescription, objSave.blAllowKilometerLimit, _mySession.UserId, PageMaster.Role, objSave.blAllowDispatchBackDateEntry).FirstOrDefault();
                        if (result != null)
                        {
                            roleId = result.InsertedId;
                            foreach (var item in objSave.strRights.Split(','))
                            {
                                string[] strRight = item.Split('|');
                                this.objDataContext.InsertOrUpdateRolePermissions(strRight[0].LongSafe(), roleId, strRight[1].LongSafe(), strRight[2].BoolSafe(), strRight[3].BoolSafe(), strRight[4].BoolSafe(), strRight[5].BoolSafe(), strRight[6].BoolSafe(), _mySession.UserId, PageMaster.Role);
                            }
                        }
                    }

                    scope.Complete();
                }

                return roleId;
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, _mySession.UserId);
                return 0;
            }
        }

        public List<SearchRoleResult> SearchRole(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext =GetDataContext())
                {
                    return this.objDataContext.SearchRole(inRow, inPage, strSearch, strSort).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, _mySession.UserId);
                return null;
            }
        }
    }
}