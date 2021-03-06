using System.Web;
using System.Web.Optimization;

namespace HSBM.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.11.1.min.js",

                        "~/Scripts/jquery-ui-1.11.4.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/toastr.js",
                        "~/Scripts/jquery-confirm.js",

                        "~/Areas/Admin/Content/JS/jquery.flot.js",
                        "~/Areas/Admin/Content/JS/jquery.flot.pie.js",
                        "~/Areas/Admin/Content/JS/jquery.flot.time.js",
                        "~/Areas/Admin/Content/JS/jquery.flot.stack.js",
                        "~/Areas/Admin/Content/JS/jquery.flot.resize.js",
                        "~/Areas/Admin/Content/JS/jquery.flot.orderBars.js",
                        "~/Areas/Admin/Content/JS/jquery.flot.spline.min.js",

                        "~/Areas/Admin/Content/JS/jquery.vmap.js",
                        "~/Areas/Admin/Content/JS/jquery.vmap.world.js",
                        "~/Areas/Admin/Content/JS/jquery.vmap.sampledata.js",


                        "~/Areas/Admin/Content/JS/bootstrap.min.js",
                        "~/Areas/Admin/Content/JS/fastclick.js",
                        "~/Areas/Admin/Content/JS/nprogress.js",
                        "~/Areas/Admin/Content/JS/Chart.min.js",
                        "~/Areas/Admin/Content/JS/gauge.min.js",
                        "~/Areas/Admin/Content/JS/bootstrap-progressbar.min.js",
                        "~/Areas/Admin/Content/JS/icheck.min.js",
                        "~/Areas/Admin/Content/JS/skycons.js",

                        "~/Areas/Admin/Content/JS/curvedLines.js",
                        "~/Areas/Admin/Content/JS/date.js",


                        "~/Areas/Admin/Content/JS/moment.min.js",
                        "~/Scripts/bootstrap-datetimepicker.js",

                        "~/Scripts/angular.js",
                        "~/Scripts/angular-filter.min.js",
                        "~/Scripts/angular-sanitize.js",
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/jquery.mCustomScrollbar.concat.min.js",
                        "~/Scripts/woco.accordion.js",
                        "~/Scripts/jquery.materialripple.js",
                        "~/Scripts/functions.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/ng-file-upload.js",
                        "~/Scripts/angular-toastr.tpls.js",
                        "~/Scripts/tinymce/tinymce.min.js",
                        "~/Scripts/angular-ui-tinymce/tinymce.js",
                        "~/Scripts/jquery.signalR-2.2.1.min.js",
                        "~/Scripts/locationpicker.jquery.min.js",
                        "~/Areas/Admin/Content/JS/daterangepicker.js",
                        "~/Areas/Admin/Content/JS/angular-input-stars.js",
                        "~/Areas/Admin/Content/JS/fileModel-directive.js",

                        "~/Content/fullcalendar/fullcalendar.min.js"

                        ));

            bundles.Add(new ScriptBundle("~/bundles/frontjquery").Include(
                        "~/Scripts/jquery-1.11.1.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery-ui-1.11.4.js",
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-filter.min.js",
                        "~/Scripts/angular-sanitize.js",
                        "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/jquery.mCustomScrollbar.concat.min.js",
                        "~/Scripts/moment.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/bootstrap-datetimepicker.js",
                        "~/Scripts/woco.accordion.js",
                        "~/Scripts/jquery.materialripple.js",
                        "~/Scripts/functions.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/ng-file-upload.js",
                        "~/Scripts/angular-toastr.tpls.js",
                        "~/Scripts/tinymce/tinymce.min.js",
                        "~/Scripts/angular-ui-tinymce/tinymce.js",
                        "~/Scripts/jquery.signalR-2.2.1.min.js",
                        "~/Scripts/locationpicker.jquery.min.js",


                        "~/Content/js/lib/jquery-form.min",
                        "~/Content/js/lib/jquery.magnific-popup.min.js",
                        "~/Content/js/lib/jquery.owl.carousel.js",
                        "~/Content/js/lib/ap.fotorama.js",
                        "~/Content/js/lib/jquery.parallax-1.1.3.js",

                        "~/Content/js/lib/md-map-extand.js",
                        "~/Content/js/lib/perfect-scrollbar.min.js",
                        "~/Content/js/scripts.js",
                        "~/Content/js/lib/SmoothScroll.js",

                        "~/Content/js/lib/jquery.sticky-kit.min.js",
                        "~/Content/js/rzslider.js"
                        ));

            #region Admin

            bundles.Add(new ScriptBundle("~/bundles/adminScripts").Include(
                        "~/Areas/Admin/Scripts/App/AppHubmain.js",
                        "~/Areas/Admin/Scripts/App/AppMain.js",
                        "~/Scripts/Common.js",
                        "~/Scripts/angular-modal-service.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Account/RegisterController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Account/SubuserController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Customer/CustomerController.js",
                        "~/Areas/Admin/Scripts/App/Factories/AccountFactory.js",
                        "~/Areas/Admin/Scripts/App/Controllers/RoleManagement/RoleMasterController.js",
                        "~/Areas/Admin/Scripts/App/Factories/RoleManagementFactory.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Locations/CountryController.js",
                        "~/Areas/Admin/Scripts/App/Factories/CountryFactory.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Locations/RegionController.js",
                        "~/Areas/Admin/Scripts/App/Factories/RegionFactory.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Locations/CityController.js",
                        "~/Areas/Admin/Scripts/App/Factories/CityFactory.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Category/CategoryAController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Category/CategoryBController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Category/CategoryCController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/BannerMaster/BannerMaster.js",
                        "~/Areas/Admin/Scripts/App/Controllers/CMSPageMaster/CMSPageMasterController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/SiteSetting/SiteSettingController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Currency/CurrencyController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/EmailTemplates/EmailTemplatesController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Subscription/SubscriptionController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/AmenityMaster/AmenityMasterController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Blogs/BlogsController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/FAQ/FAQGroupController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/FAQ/FAQsMasterController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Feedback/FeedbackMasterController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/ManageBookings/ManageBookingsController.js",
                        "~/Areas/Admin/Scripts/App/Factories/ManageBookingsFactory.js",
                        "~/Areas/Admin/Scripts/App/Controllers/ManageBookings/HotelBookingDetailController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/SupplierMarkup/SupplierMarkupController.js",
                        "~/Areas/Admin/Scripts/App/Factories/SupplierMarkupFactory.js",
                        "~/Areas/Admin/Scripts/App/Controllers/AccountSummary/AccountSummaryController.js",
                        "~/Areas/Admin/Scripts/App/Factories/AccountSummaryFactory.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Blogs/AddUpdateBlogsController.js",
                        "~/Areas/Admin/Content/JS/ng-tags-input.min.js",
                        "~/Scripts/isteven-multi-select.js",
                        "~/Areas/Admin/Scripts/App/Controllers/BlogCategory/BlogCategoryController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Blogs/BlogCommentController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/FarmStays/FarmStaysController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/FarmStays/AddUpdateFarmStaysController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/CategoryMaster/CategoryMasterController.js",
                        "~/Areas/Admin/Scripts/App/Controllers/Home/HomeController.js"
                        
                        //"~/Areas/Admin/Scripts/App/Controllers/Orders/OrdersController.js"

            ));

            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                    "~/Areas/Admin/Content/CSS/bootstrap.min.css",
                    "~/Areas/Admin/Content/CSS/font-awesome/css/font-awesome.min.css",
                    "~/Areas/Admin/Content/CSS/nprogress.css",
                    "~/Areas/Admin/Content/CSS/green.css",
                    "~/Areas/Admin/Content/CSS/bootstrap-progressbar-3.3.4.min.css",
                    "~/Areas/Admin/Content/CSS/jqvmap.min.css",
                    "~/Areas/Admin/Content/CSS/daterangepicker.css",
                    "~/Areas/Admin/Content/CSS/custom.css",
                    "~/Content/DataTables/css/jquery.dataTables.min.css",
                    "~/Content/jquery.materialripple.css",
                    "~/Content/angular-toastr.css",
                    "~/Content/toastr.css",
                    "~/Content/jquery-confirm.css",
                    "~/Content/style.css",
                    "~/Areas/Admin/Content/CSS/fonts/glyphicons.less",
                    "~/Areas/Admin/Content/CSS/angular-input-stars.css",

                    // login                    
                    "~/Areas/Admin/Content/CSS/animate.min.css",
                    "~/Content/fullcalendar/fullcalendar.min.css",
                    "~/Content/themes/base/datepicker.css"
                    ));

            #endregion

            bundles.Add(new ScriptBundle("~/bundles/frontEndScripts").Include(
                "~/Content/js/ui.js",
                 "~/Scripts/FrontApp/FrontAppMain.js",
                 "~/Scripts/Common.js",
                 "~/Scripts/FrontApp/Controllers/Hotels/HotelsController.js",
                   "~/Scripts/FrontApp/Controllers/Hotels/HotelBookingController.js",
                 "~/Scripts/FrontApp/Controllers/Common/CommonController.js",
                 "~/Scripts/FrontApp/Controllers/Customer/CustomerController.js",
                 "~/Scripts/FrontApp/Controllers/Customer/CustomerWishlistController.js",
                 "~/Scripts/FrontApp/Factories/CustomerFactory.js",
                 "~/Scripts/FrontApp/Factories/HotelsFactory.js",
                 "~/Scripts/FrontApp/Factories/CommonFactory.js",
                 "~/Scripts/FrontApp/Controllers/Blogs/BlogsController.js",
                 "~/Scripts/FrontApp/Factories/BlogsFactory.js",
                 "~/Scripts/FrontApp/select.js",
                 "~/Areas/Admin/Content/JS/daterangepicker.js",
                 "~/Scripts/FrontApp/Controllers/FarmStays/FarmStaysHomeController.js",
                 "~/Scripts/FrontApp/Factories/FarmStaysHomeFactory.js"  ,                 
                 "~/Scripts/FrontApp/Controllers/FarmStays/FarmStaysDetailController.js",
                 "~/Scripts/FrontApp/Controllers/FarmStays/FarmStaysBookingController.js",
                 "~/Scripts/FrontApp/Controllers/Orders/OrdersController.js"               
                 ));

            bundles.Add(new ScriptBundle("~/bundles/footer-js").Include(
                        "~/Scripts/nprogress.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/font-awesome.min.css",


                      "~/Content/css/lib/bootstrap.min.css",

                      "~/Content/bootstrap-theme.min.css",
                      "~/Content/DataTables/css/jquery.dataTables.min.css",
                      "~/Content/jquery.mCustomScrollbar.css",
                      "~/Content/woco-accordion.css",
                      "~/Content/jquery.materialripple.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/angular-toastr.css",


                      "~/Content/css/lib/jquery-ui.css",

                      "~/Content/css/demo.css",
                      "~/Content/css/style.css",
                      "~/Content/css/custom.css",

                      "~/Content/css/lib/awe-booking-font.css",
                      "~/Content/css/lib/font-awesome.css",
                      "~/Content/css/lib/magnific-popup.css",
                      "~/Content/css/lib/owl.carousel.css",
                      "~/Content/css/lib/fotorama.css",
                      "~/Content/css/rzslider.css",
                      "~/Content/css/select.css",

                      "~/Content/fullcalendar/fullcalendar.min.css",
                      "~/Areas/Admin/Content/CSS/daterangepicker.css"

            ));




            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));



            #region New Front

            bundles.Add(new StyleBundle("~/Content/NewTheme/css").Include(                      
                      "~/Content/NewTheme/css/bootstrap.min.css",
                      //"~/Content/NewTheme/css/bootstrap-datepicker3.css",
                      "~/Content/NewTheme/css/bootstrap-datetimepicker.css",
                      "~/Content/NewTheme/css/style.css",
                      "~/Content/NewTheme/css/responsive.css",
                      "~/Content/NewTheme/css/font-awesome.min.css",
                      "~/Content/NewTheme/css/glyphicons.less",
                      "~/Content/NewTheme/css/star-rating.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/NewTheme/js").Include(                
                        "~/Scripts/jquery-1.11.1.min.js",
                        //"~/Scripts/NewTheme/jquery-3.3.1.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery-ui-1.11.4.js",
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-cookies.min.js",
                        "~/Scripts/angular-filter.min.js",
                        "~/Scripts/angular-sanitize.js",
                        //"~/Scripts/NewTheme/jquery-3.3.1.slim.min.js",
                        "~/Scripts/NewTheme/bootstrap.min.js",
                       // "~/Scripts/NewTheme/bootstrap-datepicker.min.js",
                        //"~/Scripts/DataTables/jquery.dataTables.min.js",
                        //"~/Scripts/jquery.mCustomScrollbar.concat.min.js",
                        "~/Scripts/moment.min.js",
                        //"~/Scripts/bootstrap.min.js",
                       // "~/Scripts/bootstrap-datetimepicker.js",
                        //"~/Scripts/woco.accordion.js",
                        //"~/Scripts/jquery.materialripple.js",
                        //"~/Scripts/functions.js",
                        //"~/Scripts/respond.js",
                        "~/Scripts/ng-file-upload.js",
                        "~/Scripts/angular-toastr.tpls.js",
                        //"~/Scripts/tinymce/tinymce.min.js",
                        "~/Scripts/angular-ui-tinymce/tinymce.js", 
                        //"~/Scripts/jquery.signalR-2.2.1.min.js",
                        //"~/Scripts/locationpicker.jquery.min.js",


                        //"~/Content/js/lib/jquery-form.min",
                        //"~/Content/js/lib/jquery.magnific-popup.min.js",
                        //"~/Content/js/lib/jquery.owl.carousel.js",
                        //"~/Content/js/lib/ap.fotorama.js",
                        //"~/Content/js/lib/jquery.parallax-1.1.3.js",

                        //"~/Content/js/lib/md-map-extand.js",
                        //"~/Content/js/lib/perfect-scrollbar.min.js",
                        //"~/Content/js/scripts.js",
                        //"~/Content/js/lib/SmoothScroll.js",

                        //"~/Content/js/lib/jquery.sticky-kit.min.js",
                        //"~/Content/js/rzslider.js",          
                        //"~/Scripts/NewTheme/jquery-3.3.1.slim.min.js",
                        //"~/Scripts/NewTheme/bootstrap.min.js",
                        //"~/Scripts/NewTheme/bootstrap-datepicker.min.js",
                        //"~/Scripts/FrontApp/Controllers/Blogs/BlogsController.js",
                        //"~/Scripts/FrontApp/Factories/BlogsFactory.js",
                         "~/Scripts/DataTables/jquery.dataTables.min.js",
                        "~/Scripts/jquery.mCustomScrollbar.concat.min.js",
                        "~/Scripts/NewTheme/bootstrap-datetimepicker.js",
                        "~/Scripts/NewTheme/star-rating.js"
                        )         
                );

            #endregion


            BundleTable.EnableOptimizations = false;

        }
    }
}
