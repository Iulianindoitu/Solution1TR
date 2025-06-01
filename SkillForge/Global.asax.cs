using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using SkillForge.App_Start;
using System.Data.Entity;
using SkillForge.DAL;

namespace SkillForge
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Initialize database
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            using (var context = new ApplicationDbContext())
            {
                    context.Database.Initialize(force: false);
            }
        }
    }
}