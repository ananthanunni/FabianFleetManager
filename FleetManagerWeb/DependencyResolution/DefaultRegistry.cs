// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FleetManagerWeb.DependencyResolution {
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Transactions;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry {
        #region Constructors and Destructors

        public DefaultRegistry() {
            Scan(
                scan =>
                {
                    ////scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });
		////NIRAV
		//For<FleetManager.Core.Configuration.IConfiguration>().Use<FleetManager.Core.Configuration.Configuration>();

		RegisterCore();
		RegisterDataContexts();
		RegisterModels();
		RegisterServices();		
        }

	  #endregion

	  private void RegisterModels()
	  {
		For<FleetManager.Data.Models.IClsRole>().Use<FleetManager.Data.Models.ClsRole>();
		For<FleetManager.Data.Models.IClsUser>().Use<FleetManager.Data.Models.ClsUser>();
		For<FleetManager.Data.Models.IClsFleetMakes>().Use<FleetManager.Data.Models.ClsFleetMakes>();
		For<FleetManager.Data.Models.IClsFleetModels>().Use<FleetManager.Data.Models.ClsFleetModels>();
		For<FleetManager.Data.Models.IClsFleetColors>().Use<FleetManager.Data.Models.ClsFleetColors>();
		For<FleetManager.Data.Models.IClsTripReason>().Use<FleetManager.Data.Models.ClsTripReason>();
		For<FleetManager.Data.Models.IClsTracker>().Use<FleetManager.Data.Models.ClsTracker>();
		For<FleetManager.Data.Models.IClsCarFleet>().Use<FleetManager.Data.Models.ClsCarFleet>();
	  }

	  private void RegisterCore()
	  {
		ForSingletonOf<FleetManager.Core.Configuration.IConfiguration>().Use<FleetManager.Core.Configuration.Configuration>();
		For<FleetManager.Core.Logging.ILogger>().Use<FleetManager.Core.Logging.Logger>();
		For<FleetManager.Core.Common.IMySession>().Use<Common.MySession>();
	  }

	  private void RegisterDataContexts()
	  {
		For<FleetManager.Data.Models.CommonDataContext>().Use(ctx =>
		    new FleetManager.Data.Models.CommonDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		).Transient();

		For<FleetManager.Data.Models.CarFleetDataContext>().Use(ctx =>
		    new FleetManager.Data.Models.CarFleetDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		).Transient();

		For<FleetManager.Data.Models.FleetColorsDataContext>().Use(ctx =>
		    new FleetManager.Data.Models.FleetColorsDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		).Transient();

		For<FleetManager.Data.Models.FleetMakesDataContext>().Use(ctx =>
		    new FleetManager.Data.Models.FleetMakesDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		).Transient();

		For<FleetManager.Data.Models.FleetModelsDataContext>().Use(ctx =>
		    new FleetManager.Data.Models.FleetModelsDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		).Transient();

		For<FleetManager.Data.Models.RoleDataContext>().Use(ctx =>
		    new FleetManager.Data.Models.RoleDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		).Transient();

		For<FleetManager.Data.Models.TrackerDataContext>().Use(ctx =>
		    new FleetManager.Data.Models.TrackerDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		).Transient();

		For<FleetManager.Data.Models.TripReasonDataContext>().Use(ctx =>
		    new FleetManager.Data.Models.TripReasonDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		).Transient();

		For<FleetManager.Data.Models.UserDataContext>().Use(ctx =>
		    new FleetManager.Data.Models.UserDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		).Transient();

		For<FleetManager.Data.Models.CompanyDataContext>().Use(ctx =>
		new FleetManager.Data.Models.CompanyDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		).Transient();
	  }

	  private void RegisterServices()
	  {
		For<FleetManager.Service.Interaction.IAlertTextProvider>().Use<FleetManager.Service.Interaction.AlertTextProvider>();
		For<FleetManager.Service.Converter.IDateFormatConverter>().Use<FleetManager.Service.Converter.DateFormatConverter>();
		For<FleetManager.Service.Auth.IPermissionChecker>().Use<FleetManager.Service.Auth.PermissionChecker>();
		For<FleetManager.Service.Cookie.ICookieHandler>().Use<FleetManager.Service.Cookie.CookieHandler>();
		For<FleetManager.Service.Auth.IAuthentication>().Use<FleetManager.Service.Auth.Authentication>();
		ForSingletonOf<FleetManager.Service.Configuration.IAppConfiguration>().Use<FleetManager.Service.Configuration.AppConfiguration>();

		For<FleetManager.Service.Company.ICompanyService>().Use<FleetManager.Service.Company.CompanyService>();
		For<FleetManager.Service.Fleet.IFleetService>().Use<FleetManager.Service.Fleet.IFleetService>();
		For<FleetManager.Service.User.IUserService>().Use<FleetManager.Service.User.UserService>();
		For<FleetManager.Service.Tracking.ITrackerService>().Use<FleetManager.Service.Tracking.TrackerService>();
	  }
    }
}