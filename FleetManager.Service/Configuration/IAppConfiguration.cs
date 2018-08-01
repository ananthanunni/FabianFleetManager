using FleetManager.Core.Configuration;

namespace FleetManager.Service.Configuration
{
    public interface IAppConfiguration
    {
	  IConfiguration BaseConfiguration { get; }

	  string CreateRootDirectory(string pageName, string dirPath);
	  string GetRootDirectory(string pageName);
	  string GetSettings(string keyName);
    }
}