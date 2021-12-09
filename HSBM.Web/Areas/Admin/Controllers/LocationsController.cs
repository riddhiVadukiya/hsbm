using MVCGrid.Models;
using MVCGrid.Web;
using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.CityMaster;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.CountryMaster;
using HSBM.EntityModel.RegionMaster;
using HSBM.Service.Contracts;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    //[CustomAuthorize(Module.Location)]
    public class LocationsController : BaseController
    {
        //public readonly ILocationsService _LocationsService;
        LocationsService _LocationsService = new LocationsService();
        CountryService _CountryService = new CountryService();
        CityService _CityService = new CityService();
        RegionService _RegionService = new RegionService();

        //public LocationsController(ILocationsService LocationsService)
        public LocationsController()
        {
            //_LocationsService = LocationsService;
        }

        public ActionResult Index()
        {
            _LocationsService.GetAllLocations(new CountryMasterRequest());

            return View();
        }

        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanView)]
        public ActionResult Country()
        {


            //MVCGridDefinitionTable.Add("CountryGridview", new MVCGridBuilder<CountryMaster>()
            //    .WithAuthorizationType(AuthorizationType.AllowAnonymous)
            //    .WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Dsc)
            //    .WithPaging(paging: true, itemsPerPage: 5, allowChangePageSize: true, maxItemsPerPage: 100)
            //    .WithAdditionalQueryOptionNames("search")
            //    .AddColumns(cols =>
            //    {
            //        cols.Add("Id").WithValueExpression((p, c) => c.UrlHelper.Action("UpdateCountry", "Locations", new { id = p.Id }))
            //            .WithValueTemplate("<a href='{Value}'>{Model.Id}</a>", false)
            //            .WithPlainTextValueExpression(p => p.Id.ToString());
            //        cols.Add("CountryName").WithHeaderText("CountryName")
            //            .WithVisibility(true, true)
            //            .WithValueExpression(p => p.CountryName);
            //        cols.Add("Code").WithHeaderText("Code")
            //            .WithVisibility(true, true)
            //            .WithValueExpression(p => p.Code);
            //        cols.Add("IsActive")
            //            .WithSortColumnData("IsActive")
            //            .WithVisibility(visible: true, allowChangeVisibility: true)
            //            .WithHeaderText("Status")
            //            .WithValueExpression(p => p.IsActive ? "Active" : "Inactive")
            //            .WithCellCssClassExpression(p => p.IsActive ? "success" : "danger");
            //        cols.Add("ViewLink").WithSorting(false)
            //            .WithHeaderText("")
            //            .WithHtmlEncoding(false)
            //            .WithValueExpression((p, c) => c.UrlHelper.Action("UpdateCountry", "Locations", new { id = p.Id }))
            //            .WithValueTemplate("<a href='{Value}'>Edit</a>");
            //    })
            //    //.WithAdditionalSetting(MVCGrid.Rendering.BootstrapRenderingEngine.SettingNameTableClass, "notreal") // Example of changing table css class
            //    .WithRetrieveDataMethod((context) =>
            //    {
            //        var options = context.QueryOptions;
            //        int totalRecords = 20;

            //        //var repo = DependencyResolver.Current.GetService<IPersonRepository>();
            //        //string globalSearch = options.GetAdditionalQueryOptionString("search");
            //        //string sortColumn = options.GetSortColumnData<string>();
            //        //var items = repo.GetData(out totalRecords, globalSearch, options.GetLimitOffset(), options.GetLimitRowcount(), sortColumn, options.SortDirection == SortDirection.Dsc);
            //        return new QueryResult<CountryMaster>()
            //        {
            //            Items = _LocationsService.GetAllCountry(),
            //            TotalRecords = totalRecords
            //        };
            //    }));


            GridParams p_GridParams = new GridParams();

            // var CountryList = _LocationsService.GetAllCountry();

            return View();
        }

        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanAdd)]
        public ActionResult AddCountry()
        {
            return PartialView("AddUpdateCountry", new CountryMaster());
        }

        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanUpdate)]
        public ActionResult UpdateCountry(long Id)
        {
            return PartialView("AddUpdateCountry", _CountryService.GetCountryByCountryId(Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult AddUpdateCountry(CountryMaster countryMaster)
        {
            if (countryMaster.Id > 0)
            {
                //if (!SessionProxy.CheckModuleAccess(Module.Location, ModuleAccess.CanUpdate))
                    return RedirectToAction("AccessDeniedOperation", "Information");
            }
            else
            {
                //if (!SessionProxy.CheckModuleAccess(Module.Location, ModuleAccess.CanAdd))
                    return RedirectToAction("AccessDeniedOperation", "Information");
            }
            if (_CountryService.AddorUpdateCountry(countryMaster))
            {
                return RedirectToAction("Country");
            }
            else
            {
                return PartialView("AddUpdateCountry", countryMaster);
            }
        }

        public ActionResult AddUpdateCloneCountry(CountryMaster countryMaster)
        {
            if (_CountryService.AddorUpdateCountry(countryMaster))
            {
                return RedirectToAction("Country");
            }
            else
            {
                return RedirectToAction("Country");
            }
        }

        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanDelete)]
        public ActionResult DeleteCountry(long Id)
        {
            _CountryService.DeleteCountry(Id);
            return RedirectToAction("Country");
        }


     //   [CustomAuthorizeAction(Module.Location, ModuleAccess.CanView)]
        public ActionResult Region()
        {
            return View();
            //return View(_LocationsService.GetAllRegion());
        }

       // [CustomAuthorizeAction(Module.Location, ModuleAccess.CanAdd)]
        public ActionResult AddRegion()
        {
            GetCountryDropDown();
            return PartialView("AddUpdateRegion", new RegionMaster());
        }

       // [CustomAuthorizeAction(Module.Location, ModuleAccess.CanUpdate)]
        public ActionResult UpdateRegion(long Id)
        {
            GetCountryDropDown();
            return PartialView("AddUpdateRegion", _RegionService.GetRegionById(Id));
        }

        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanDelete)]
        public ActionResult DeleteRegion(long Id)
        {
            _RegionService.DeleteRegionById(Id);
            return RedirectToAction("Region");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult AddUpdateRegion(RegionMaster regionMaster)
        {
            if (ModelState.IsValid && regionMaster.CountryMasterId != 0)
            {
                if (_RegionService.AddUpdateRegion(regionMaster))
                {
                    return RedirectToAction("Region");
                }
                else
                {
                    GetCountryDropDown();
                    return PartialView("AddUpdateRegion", regionMaster);
                }
            }
            else
            {
                GetCountryDropDown();
                return PartialView("AddUpdateRegion", regionMaster);
            }
        }

        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanView)]
        public ActionResult City()
        {
            return View(_CityService.GetAllCity());
        }

        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanAdd)]
        public ActionResult AddCity()
        {
            GetCountryDropDown();
            GetRegionDropDown(0);
            return PartialView("AddUpdateCity", new CityMaster());
        }

        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanUpdate)]
        public ActionResult UpdateCity(long Id)
        {
            var _city = _CityService.GetCityById(Id);
            GetCountryDropDown();
            GetRegionDropDown(_city.CountryMasterId);
            return PartialView("AddUpdateCity", _city);
        }

        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanDelete)]
        public ActionResult DeleteCity(long Id)
        {
            _CityService.DeleteCity(Id);
            return RedirectToAction("City");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult AddUpdateCity(CityMaster cityMaster, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && cityMaster.CountryMasterId != 0 && cityMaster.RegionMasterId != 0)
            {
                if (file != null && file.ContentLength > 0)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var fileNameAuto = Guid.NewGuid() + "-" + Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images/destination/"), fileNameAuto);
                        file.SaveAs(path);

                        cityMaster.ImageUrl = fileNameAuto;
                        cityMaster.ImageOrignalName = fileName;
                    }
                }

                if (_CityService.AddUpdateCity(cityMaster))
                {
                    return RedirectToAction("City");
                }
                else
                {
                    GetCountryDropDown();
                    GetRegionDropDown(cityMaster.CountryMasterId);
                    return PartialView("AddUpdateCity", cityMaster);
                }
            }
            else
            {
                GetCountryDropDown();
                GetRegionDropDown(cityMaster.CountryMasterId);
                return PartialView("AddUpdateCity", cityMaster);
            }
        }

        public JsonResult GetRegionList(long Id)
        {
            List<SelectListItem> _List = _RegionService.GetAllRegionById(Id);
            _List.Find(x => x.Text == "Select").Value = string.Empty;
            return Json(_List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCountryList()
        {
            return Json(_CountryService.GetAllCountry(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityList(long Id)
        {
            List<SelectListItem> _List = _CityService.CityDropDown(Id);
            _List.Find(x => x.Text == "Select").Value = string.Empty;
            return Json(_List, JsonRequestBehavior.AllowGet);
        }


        private void GetCountryDropDown()
        {
            List<SelectListItem> _List = _CountryService.GetAllCountry();
            _List.Find(x => x.Text == "Select").Value = string.Empty;
            ViewBag.CountryDropDown = _List;
        }
        private void GetRegionDropDown(long CountryId)
        {
            List<SelectListItem> _List = _RegionService.GetAllRegionById(CountryId);
            _List.Find(x => x.Text == "Select").Value = string.Empty;
            ViewBag.RegionDropDown = _List;
        }
        private void GetCityDropDown(long RegionId)
        {
            ViewBag.CityDropDown = _CityService.CityDropDown(RegionId);
        }

        public JsonResult GetAllCountriesBySearchRequest(CountryMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "CountryName";

                GridDataResponse _GridDataResponse = _CountryService.GetAllCountriesBySearchRequest(p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);

                //if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && _GridDataResponse.data != null && _GridDataResponse.recordsTotal > 0)
                //{
                //    _GridDataResponse.draw = p_GridParams.draw;
                //    return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
                //}
                //else
                //    return Json(null, JsonRequestBehavior.AllowGet);

            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public JsonResult GetAllRegionBySearchRequest(RegionMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "RegionName";

                GridDataResponse _GridDataResponse = _RegionService.GetRegionBySearchRequest(p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);

            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public JsonResult GetAllCityBySearchRequest(CityMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "CityName";

                GridDataResponse _GridDataResponse = _CityService.GetCityBySearchRequest(p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanUpdate)]
        public JsonResult ActiveAndInactiveSwitchForCountryUpdate(CountryMaster countryMaster)
        {
            _CountryService.ActiveInactiveCountryById(countryMaster);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanUpdate)]
        public JsonResult ActiveAndInactiveSwitchForRegionMasterUpdate(RegionMaster regionMaster)
        {
            _RegionService.ActiveInactiveRegionById(regionMaster);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanUpdate)]
        public JsonResult ActiveAndInactiveSwitchForCityMasterUpdate(CityMaster cityMaster)
        {
            _CityService.ActiveInactiveCityById(cityMaster);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[CustomAuthorizeAction(Module.Location, ModuleAccess.CanUpdate)]
        public JsonResult TopDestinationSwitchForCityMasterUpdate(CityMaster cityMaster)
        {
            _CityService.TopDestinationSwitchCityById(cityMaster);
            return Json("", JsonRequestBehavior.AllowGet);
        }

    }
}