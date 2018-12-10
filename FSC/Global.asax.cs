using AutoMapper;
using FSC.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Reflection;
using FSC.DataLayer.Repository;
using FSC.DataLayer.Repository.Interface;

namespace FSC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());
            ConfigureContainer();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            builder.RegisterType<ChecklistRepository>().As<IChecklistRepository>().InstancePerRequest();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerRequest();

            //// OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //Set the MVC DependencyResolver
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }

    }
}
