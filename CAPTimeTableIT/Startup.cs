using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using CAPTimeTableIT.App_Start;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(CAPTimeTableIT.Startup))]
namespace CAPTimeTableIT
{
    public partial class Startup
    {
        public static IContainer container { get; set; }
        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();
            //register DI here
            var builder = AutofacConfig.Configuration(app);

            container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            httpConfig.DependencyResolver = resolver;
            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            ConfigureAuth(app);
        }
    }
}
