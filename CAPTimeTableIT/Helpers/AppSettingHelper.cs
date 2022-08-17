using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace CAPTimeTableIT.Helpers
{
    public sealed class AppSettingHelper
    {
        private static readonly AppSettingHelper instance = new AppSettingHelper();
        public string Email { get; set; }
        public string Password { get; set; }
        public string HomePageUrl { get; set; }
        public string WebFolderUrl { get; set; }
        public string UniversityWebsiteUrl { get; set; }
        static AppSettingHelper()
        {
        }
        private AppSettingHelper()
        {
            Email = ConfigurationManager.AppSettings["EmailId"];
            Password = ConfigurationManager.AppSettings["EmailPassword"];
            HomePageUrl = ConfigurationManager.AppSettings["HomePageUrl"];
            UniversityWebsiteUrl = ConfigurationManager.AppSettings["UniversityWebsite"];
            WebFolderUrl = HostingEnvironment.MapPath("~/");
        }
        public static AppSettingHelper Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
