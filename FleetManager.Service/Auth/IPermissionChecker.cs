using FleetManager.Data.Models;
using System.Collections.Generic;

namespace FleetManager.Service.Auth
{
    public interface IPermissionChecker
    {
	  GetPagePermissionResult CheckPagePermission(long lgPageId);
	  List<GetPagePermissionResult> GerRolePermissionByRoleId(long lgRoleId);
    }
}