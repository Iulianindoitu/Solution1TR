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
               // Core CSS Bundles
               bundles.Add(new StyleBundle("~/bundles/core/css").Include(
                    "~/Content/assets/vendor/css/core.css",
                    "~/Content/assets/vendor/fonts/iconify-icons.css",
                    "~/Content/assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.css",
                    "~/Content/assets/vendor/libs/apex-charts/apex-charts.css"
               ));

               // Demo CSS Bundle
               bundles.Add(new StyleBundle("~/bundles/demo/css").Include(
                    "~/Content/assets/css/demo.css"
               ));

               // Login CSS Bundle
               bundles.Add(new StyleBundle("~/bundles/login/css").Include(
                    "~/Content/assets/vendor/css/pages/page-auth.css"
               ));

               // Core JavaScript Bundles
               bundles.Add(new ScriptBundle("~/bundles/core/js").Include(
                    "~/Content/assets/vendor/js/helpers.js",
                    "~/Content/assets/js/config.js",
                    "~/Content/assets/vendor/libs/jquery/jquery.js",
                    "~/Content/assets/vendor/libs/popper/popper.js",
                    "~/Content/assets/vendor/js/bootstrap.js",
                    "~/Content/assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.js",
                    "~/Content/assets/vendor/js/menu.js",
                    "~/Content/assets/js/main.js",
                    "~/Content/assets/vendor/libs/apex-charts/apexcharts.js",
                    "~/Content/assets/js/dashboards-analytics.js"
               ));

               // Login JavaScript Bundle
               bundles.Add(new ScriptBundle("~/bundles/login/js").Include(
                    "~/Content/assets/js/pages-auth.js"
               ));

               // Favicon
               bundles.Add(new StyleBundle("~/bundles/favicon").Include(
                    "~/Content/assets/img/favicon/head.ico"
               ));
          }
     }
}