using HSBM.Common.Utils;
using HSBM.EntityModel.RoleMaster;
using HSBM.Service.Contracts;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class CommonFrontController : BaseController
    {
        CountryService _CountryService = new CountryService();
        CityService _CityService = new CityService();

        public JsonResult GetCountryList()
        {
            return Json(_CountryService.GetAllCountry(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllCityByCountryId(long p_CountryId)
        {
            return Json(_CityService.GetAllCityByCountryId(p_CountryId), JsonRequestBehavior.AllowGet);
        }
    }
}