namespace FleetManagerWeb
{
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    using FleetManagerWeb.Models;

    using Unity.Mvc4;

    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IClsRole, ClsRole>();
            container.RegisterType<IClsUser, ClsUser>();
            container.RegisterType<IClsFleetMakes, ClsFleetMakes>();
            container.RegisterType<IClsFleetModels, ClsFleetModels>();
            container.RegisterType<IClsFleetColors, ClsFleetColors>();
            container.RegisterType<IClsTripReason, ClsTripReason>();
            container.RegisterType<IClsTracker, ClsTracker>();
            container.RegisterType<IClsCarFleet, ClsCarFleet>();
            RegisterTypes(container);
            return container;
        }
    }
}