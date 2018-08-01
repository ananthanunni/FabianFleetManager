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
		For<Models.IClsRole>().Use<Models.ClsRole>();
		For<Models.IClsUser>().Use<Models.ClsUser>();
		For<Models.IClsFleetMakes>().Use<Models.ClsFleetMakes>();
		For<Models.IClsFleetModels>().Use<Models.ClsFleetModels>();
		For<Models.IClsFleetColors>().Use<Models.ClsFleetColors>();
		For<Models.IClsTripReason>().Use<Models.ClsTripReason>();
		For<Models.IClsTracker>().Use<Models.ClsTracker>();
		For<Models.IClsCarFleet>().Use<Models.ClsCarFleet>();
	  }

	  private void RegisterCore()
	  {
		ForSingletonOf<FleetManager.Core.Configuration.IConfiguration>().Use<FleetManager.Core.Configuration.Configuration>();
		For<FleetManager.Core.Logging.ILogger>().Use<FleetManager.Core.Logging.Logger>();
	  }

	  private void RegisterDataContexts()
	  {
		For<Models.CommonDataContext>().Use(ctx =>
		    new Models.CommonDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		);

		For<Models.CarFleetDataContext>().Use(ctx =>
		    new Models.CarFleetDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		);

		For<Models.FleetColorsDataContext>().Use(ctx =>
		    new Models.FleetColorsDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		);

		For<Models.FleetMakesDataContext>().Use(ctx =>
		    new Models.FleetMakesDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		);

		For<Models.FleetModelsDataContext>().Use(ctx =>
		    new Models.FleetModelsDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		);

		For<Models.RoleDataContext>().Use(ctx =>
		    new Models.RoleDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		);

		For<Models.TrackerDataContext>().Use(ctx =>
		    new Models.TrackerDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		);

		For<Models.TripReasonDataContext>().Use(ctx =>
		    new Models.TripReasonDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		);

		For<Models.UserDataContext>().Use(ctx =>
		    new Models.UserDataContext(ctx.GetInstance<FleetManager.Core.Configuration.IConfiguration>().ConnectionString)
		);
	  }

	  private void RegisterServices()
	  {
		For<FleetManager.Service.Interaction.IAlertTextProvider>().Use<FleetManager.Service.Interaction.AlertTextProvider>();
		For<FleetManager.Service.Converter.IDateFormatConverter>().Use<FleetManager.Service.Converter.DateFormatConverter>();
	  }
    }
}