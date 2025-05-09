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
               // CSS Bundles
               bundles.Add(new StyleBundle("~/bundles/login/css").Include(
                    "~/Content/assets/vendor/fonts/iconify-icons.css",
                    "~/Content/assets/vendor/css/core.css",
                    "~/Content/assets/css/demo.css",
                    "~/Content/assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.css",
                    "~/Content/assets/vendor/css/pages/page-auth.css"
               ));

               // JavaScript Bundles
               bundles.Add(new ScriptBundle("~/bundles/login/js").Include(
                    "~/Content/assets/vendor/js/helpers.js",
                    "~/Content/assets/js/config.js",
                    "~/Content/assets/vendor/libs/jquery/jquery.js",
                    "~/Content/assets/vendor/libs/popper/popper.js",
                    "~/Content/assets/vendor/js/bootstrap.js",
                    "~/Content/assets/vendor/libs/perfect-scrollbar/perfect-scrollbar.js",
                    "~/Content/assets/vendor/js/menu.js",
                    "~/Content/assets/js/main.js"
               ));

               // Favicon
               bundles.Add(new StyleBundle("~/bundles/favicon").Include(
                    "~/Content/assets/img/favicon/head.ico"
               ));
          }
     }
}