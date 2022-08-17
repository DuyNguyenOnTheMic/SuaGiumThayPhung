using Autofac;
using Autofac.Integration.Mvc;
using CAPTimeTableIT.Helpers;
using CAPTimeTableIT.Infrastructure;
using CAPTimeTableIT.Infrastructure.Entities;
using CAPTimeTableIT.Infrastructure.Repositories;
using CAPTimeTableIT.Infrastructure.Services.Abstract;
using CAPTimeTableIT.Infrastructure.Services.Implement;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CAPTimeTableIT.App_Start
{
    public class AutofacConfig
    {
        public static ContainerBuilder Configuration(IAppBuilder app)
        {
            var email = AppSettingHelper.Instance.Email;
            var password = AppSettingHelper.Instance.Password;
            var webFolder = AppSettingHelper.Instance.WebFolderUrl;
            var builder = new ContainerBuilder();            
            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            builder.Register(c => new ApplicationDbContext())
                .As<DbContext>().InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerRequest();

            //declare services
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<SubjectService>().As<ISubjectService>().InstancePerRequest();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerRequest();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>().InstancePerRequest();
            builder.RegisterType<SemesterService>().As<ISemesterService>().InstancePerRequest();
            builder.RegisterType<CapWeekService>().As<ICapWeekService>().InstancePerRequest();
            builder.RegisterType<CapDayService>().As<ICapDayService>().InstancePerRequest();
            builder.RegisterType<ClassService>().As<IClassService>().InstancePerRequest();
            builder.RegisterType<UserProfileService>().As<IUserProfileService>().InstancePerRequest();
            builder.RegisterType<StatisticService>().As<IStatisticService>().InstancePerRequest();
            builder.RegisterType<ExcelService>().As<IExcelService>().InstancePerRequest();
            builder.RegisterType<CapstoneEmailService>().As<ICapstoneEmailService>()
                 .WithParameter("email", email)
                 .WithParameter("password", password)
                 .WithParameter("webFolderUrl", webFolder)
                .InstancePerRequest();
            //Register user store
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();
            return builder;
        }
    }
}