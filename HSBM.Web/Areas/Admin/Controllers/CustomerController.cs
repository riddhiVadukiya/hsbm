using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.SiteSettings;
using HSBM.EntityModel.SystemUsers;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Areas.Admin.Models;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class CustomerController : BaseController
    {
        CountryService _CountryService = new CountryService();
        CityService _CityService = new CityService();
        RegionService _RegionService = new RegionService();
        SystemUserService _SystemUserService = new SystemUserService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewCustomerDetail(long id)
        {

            SystemUsers systemUser = _SystemUserService.GetSystemUserByKey(WebRequestResponseServiceContext, id);
            systemUser.CountryName = systemUser.CountryMasterID.HasValue ? _CountryService.GetCountryByCountryId(systemUser.CountryMasterID.Value).CountryName : string.Empty;
            systemUser.RegionName = systemUser.RegionMasterID.HasValue ? _RegionService.GetRegionById(systemUser.RegionMasterID.Value).RegionName : string.Empty;
            systemUser.CityName = systemUser.CityMasterID.HasValue ? _CityService.GetCityById(systemUser.CityMasterID.Value).CityName : string.Empty;
            systemUser.FullAddress += ", " + systemUser.CityName + ", " + systemUser.RegionName + ", " + systemUser.CountryName;
            return View(systemUser);
        }
        public ActionResult Export(string CustomerName, int ExportTypeExcel)
        {
            SystemUsersRequest p_SystemUsersRequest = new SystemUsersRequest();
            p_SystemUsersRequest.UserName = CustomerName;            
            GridParams p_GridParams = new GridParams();
            p_GridParams.take = int.MaxValue;

            if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                p_GridParams.DefaultOrderBy = "Id";

            GridView gv = new GridView();
            p_SystemUsersRequest.UserType = (int)HSBM.Common.Enums.UserTypes.Customer;
            GridDataResponse _GridDataResponse = _SystemUserService.GetAllSubUsersBySearchRequest(p_GridParams, p_SystemUsersRequest);
            var data = (from s in ((List<SystemUsersResponse>)_GridDataResponse.data) select new { CustomerName = s.FirstName + " " + s.LastName, Email = s.Email, Gender = s.Gender, Mobile = s.Mobile, Address = s.Address });
            gv.DataSource = data;
            gv.DataBind();

            if (Convert.ToBoolean(ExportTypeExcel))
            {
                return new ExportToExcel((GridView)gv, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_Customers");
            }
            else
            {
                List<object> _data = new List<object>();
                _data.AddRange(data);

                List<string> _header = new List<string>();
                _header.Add("CustomerName");
                _header.Add("Email");
                _header.Add("Gender");
                _header.Add("Mobile");
                _header.Add("Address");

                return new ExportToCSV(_data, _header, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_Customers");
            }
        }

        public JsonResult GetAllCustomerBySearchRequest(SystemUsersRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Id";

                p_SearchRequest.UserType = (int)HSBM.Common.Enums.UserTypes.Customer;
                GridDataResponse _GridDataResponse = _SystemUserService.GetAllSubUsersBySearchRequest(p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);

            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ActiveAndInactiveSwitchUpdate(SystemUsers systemUsers)
        {
            _SystemUserService.ActiveAndInactiveSwitchUpdate(systemUsers);
            return Json("", JsonRequestBehavior.AllowGet);
        }


        private void GetGenderDropDown()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            lst.Add(new SelectListItem() { Text = "Female", Value = "Female" });
            ViewBag.GenderDropDown = lst;
        }

        private void GetCountryDropDown()
        {
            ViewBag.CountryDropDown = _CountryService.GetAllCountry();
        }
        private void GetRegionDropDown(long CountryId)
        {
            ViewBag.RegionDropDown = _RegionService.GetAllRegionById(CountryId);
        }
        private void GetCityDropDown(long RegionId)
        {
            ViewBag.CityDropDown = _CityService.CityDropDown(RegionId);
        }

        public JsonResult GetRegionList(long Id)
        {
            return Json(_RegionService.GetAllRegionById(Id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityList(long Id)
        {
            return Json(_CityService.CityDropDown(Id), JsonRequestBehavior.AllowGet);
        }
    }
}