using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace SkillForge.App_Start
{
     public class BundleConfig
     {
          public static void RegisterBundles(BundleCollection bundles)
          {
               //CSS
               // Add your bundle configuration here
              bundles.Add(new StyleBundle("~/bundles/demo/css").Include(
                  "~/Content/assets/css/demo.css"));

               bundles.Add(new StyleBundle("~/bundles/vendor/core/css").Include(
                  "~/Content/assets/vendor/core.css"));

               bundles.Add(new StyleBundle("~/bundles/pages/page-auth/css").Include(
                  "~/Content/assets/pages/page-auth.css"));

               bundles.Add(new StyleBundle("~/bundles/pages/page-icons/css").Include(
                  "~/Content/assets/pages/page-icons.css"));

               bundles.Add(new StyleBundle("~/bundles/pages/page-misc/css").Include(
                    "~/Content/assets/pages/page-misc.css"));

               bundles.Add(new StyleBundle("~/bundles/vendor/fonts/iconify-icons/css").Include(
                    "~/Content/assets/vendor/fonts/iconify-icons.css"));

               //JS
               bundles.Add(new ScriptBundle("~/bundles/vendor/core/js").Include(
                    "~/Content/assets/vendor/core.js"));

               bundles.Add(new ScriptBundle("~/bundles/vendor/js/menu/js").Include(
                    "~/Content/assets/vendor/js/menu.js"));

               bundles.Add(new ScriptBundle("~/bundles").Include(
                    "~/Content/assets/js/main.js"));

               bundles.Add(new ScriptBundle("~/bundles/vendor/jquery/js").Include(
                     "~/Content/assets/vendor/jquery/jquery.js"));

               bundles.Add(new ScriptBundle("~/bundles/vendor/jquery/js/jquery.min.js").Include(
                     "~/Content/assets/vendor/jquery/jquery.min.js"));

               bundles.Add(new ScriptBundle("~/bundles/vendor/jquery/js/jquery.slim.min.js").Include(
                     "~/Content/assets/libs/jquery/jquery.slim.min.js"));

               bundles.Add(new ScriptBundle("~/bundles/js/dashboard-analytics/js").Include
                    ("~/Content/assets/js/dashboard-analytics.js"));

               bundles.Add(new ScriptBundle("~/bundles/js/helpers/js").Include
                    ("~/Content/js/helpers.js");

               bundles.Add(new ScriptBundle("~/bundles"))
          }
     }
}