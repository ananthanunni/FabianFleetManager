using FleetManager.Core.Logging;
using FleetManager.Data.Models;
using FleetManager.Model.Common;
using FleetManagerWeb.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManager.Service.Auth
{
    public class PermissionChecker : IPermissionChecker
    {
	  private CommonDataContext _objDataContext;
	  private ILogger _logger;
	  private readonly IMySession _mySession;

	  public PermissionChecker(CommonDataContext objDataContext, ILogger logger,IMySession mySession)
	  {
		_objDataContext = objDataContext;
		_logger = logger;
		_mySession = mySession;
	  }

	  public GetPagePermissionResult CheckPagePermission(long lgPageId)
	  {
		try
		{
			  GetPagePermissionResult objRights = _objDataContext.GetPagePermission(lgPageId, _mySession.UserId, _mySession.RoleId).FirstOrDefault();
			  if (objRights == null)
			  {
				objRights = new GetPagePermissionResult();
			  }

			  return objRights;
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
		    return new GetPagePermissionResult();
		}
	  }

	  public List<GetPagePermissionResult> GerRolePermissionByRoleId(long lgRoleId)
	  {
		try
		{
			  return _objDataContext.GetPagePermission(0, 0, lgRoleId).ToList();
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.Role, _mySession.UserId);
		    return null;
		}
	  }
    }
}
