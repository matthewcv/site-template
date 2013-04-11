﻿using System.Web;
using System.Web.Optimization;

namespace mywebsite
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/account").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/toastr.js",
                        "~/Scripts/KnockoutValidation.js",
                        "~/Scripts/Account/models.js",
                        "~/Scripts/Account/app.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/candy").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/sammy-{version}.js",
                        "~/Scripts/toastr.js",
                        "~/Scripts/KnockoutValidation.js",
                        "~/Scripts/Candy/models.js",
                        "~/Scripts/Candy/app.js"
                ));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/toastr.css",
                "~/Content/site.css"
                ));

        }
    }
}