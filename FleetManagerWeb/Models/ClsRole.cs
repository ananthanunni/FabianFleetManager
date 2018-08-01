﻿namespace FleetManagerWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Web.Mvc;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public partial class ClsRole : IClsRole
    {
        private RoleDataContext objDataContext = null;

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
                using (this.objDataContext = new RoleDataContext(Functions.StrConnection))
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
                return null;
            }
        }

        public List<SelectListItem> GetAllRoleForDropDown()
        {
            List<SelectListItem> lstRole = new List<SelectListItem>();
            try
            {
                using (this.objDataContext = new RoleDataContext(Functions.StrConnection))
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
            }

            return lstRole;
        }

        public ClsRole GetRoleByRoleId(long lgRoleId)
        {
            ClsRole objClsRole = new ClsRole();
            try
            {
                using (this.objDataContext = new RoleDataContext(Functions.StrConnection))
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
            }

            return objClsRole;
        }

        public bool IsRoleExists(long lgRoleId, string strRoleName)
        {
            try
            {
                using (this.objDataContext = new RoleDataContext(Functions.StrConnection))
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
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
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
                    using (this.objDataContext = new RoleDataContext(Functions.StrConnection))
                    {
                        var result = this.objDataContext.InsertOrUpdateRole(objSave.lgId, objSave.strRoleName, objSave.strDescription, objSave.blAllowKilometerLimit, mySession.Current.UserId, PageMaster.Role, objSave.blAllowDispatchBackDateEntry).FirstOrDefault();
                        if (result != null)
                        {
                            roleId = result.InsertedId;
                            foreach (var item in objSave.strRights.Split(','))
                            {
                                string[] strRight = item.Split('|');
                                this.objDataContext.InsertOrUpdateRolePermissions(strRight[0].longSafe(), roleId, strRight[1].longSafe(), strRight[2].boolSafe(), strRight[3].boolSafe(), strRight[4].boolSafe(), strRight[5].boolSafe(), strRight[6].boolSafe(), mySession.Current.UserId, PageMaster.Role);
                            }
                        }
                    }

                    scope.Complete();
                }

                return roleId;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
                return 0;
            }
        }

        public List<SearchRoleResult> SearchRole(int inRow, int inPage, string strSearch, string strSort)
        {
            try
            {
                using (this.objDataContext = new RoleDataContext(Functions.StrConnection))
                {
                    return this.objDataContext.SearchRole(inRow, inPage, strSearch, strSort).ToList();
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, mySession.Current.UserId);
                return null;
            }
        }
    }
}