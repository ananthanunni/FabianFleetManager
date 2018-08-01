using FleetManager.Core.Configuration;
using FleetManager.Core.Logging;
using FleetManager.Data.Models;
using FleetManagerWeb.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManager.Service.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
	  private readonly ILogger _logger;
	  private readonly CommonDataContext _commonDataContext;

	  public AppConfiguration(ILogger logger, IConfiguration configuration,
		CommonDataContext commonDataContext)
	  {
		_logger = logger;
		BaseConfiguration = configuration;
		_commonDataContext = commonDataContext;
	  }

	  public IConfiguration BaseConfiguration { get; }

	  public string CreateRootDirectory(string pageName, string dirPath)
	  {
		try
		{
		    var objSetting = _commonDataContext.AAAAConfigSettings.Where(x => x.KeyName == "DocRootFolderPath").FirstOrDefault();
		    if (objSetting != null)
		    {
			  dirPath = objSetting.KeyValue + dirPath;
			  if (!Directory.Exists(dirPath))
			  {
				Directory.CreateDirectory(dirPath);
			  }
		    }

		    return dirPath;
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
		    return null;
		}
	  }

	  public string GetRootDirectory(string pageName)
	  {
		try
		{
		    string strDocPath = _commonDataContext.AAAAConfigSettings.Where(a => a.KeyName == "DocRootFolderPath").FirstOrDefault().KeyValue.ToString();
		    string strKeyValue = _commonDataContext.AAAAConfigSettings.Where(a => a.KeyName == pageName).FirstOrDefault().KeyValue.ToString();
		    return strDocPath + strKeyValue;
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
		    return null;
		}
	  }

	  public string GetSettings(string keyName)
	  {
		try
		{
		    var objSetting = _commonDataContext.AAAAConfigSettings.Where(x => x.KeyName == keyName).FirstOrDefault();
		    if (objSetting != null)
		    {
			  return objSetting.KeyValue;
		    }

		    return string.Empty;
		}
		catch (Exception ex)
		{
		    _logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
		    return string.Empty;
		}
	  }
    }
}
