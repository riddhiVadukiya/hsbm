using HSBM.Common.Utils;
using HSBM.EntityModel.FrontEnd;
using HSBM.Service.Services;
using HSBM.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Controllers
{
    public class FarmStaysController : BaseController
    {
        FrontFarmStaysSearchService _FrontFarmStaysSearchService = new FrontFarmStaysSearchService();

        public ActionResult Index()
        {
            FarmStaysRequestModel req = new FarmStaysRequestModel();
            req.CheckIn = DateTime.Now.AddDays(1).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
            req.CheckOut = DateTime.Now.AddDays(2).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); ;
            req.Guests = 2;
            req.IsSolo = false;
            return View(req);
        }

        //public ActionResult Index(string CheckIn, string CheckOut, int Guests, bool IsSolo = false)
        //{
        //    DateTime dtCheckIn = DateTime.ParseExact(CheckIn, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
        //    DateTime dtCheckOut = DateTime.ParseExact(CheckOut, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);


        //    FarmStaysRequestModel req = new FarmStaysRequestModel();
        //    req.CheckIn = dtCheckIn;
        //    req.CheckOut = dtCheckOut;
        //    req.Guests = Guests;
        //    req.IsSolo = IsSolo;
        //    return View(req);
        //}

        [HttpPost]
        public ActionResult Index(FarmStaysRequestModel req)
        {
            return View(req);
        }


        public JsonResult GetFarmstays(FarmStaysRequestModel reqSerach)
        {
            SearchFarmStaysRequest _SearchFarmStaysRequest = new SearchFarmStaysRequest();
            _SearchFarmStaysRequest.CheckIn = reqSerach.CheckIn;
            _SearchFarmStaysRequest.CheckOut = reqSerach.CheckOut;
            _SearchFarmStaysRequest.Guests = reqSerach.Guests;
            _SearchFarmStaysRequest.IsSolo = reqSerach.IsSolo;
            _SearchFarmStaysRequest.IsPackage = reqSerach.IsPackage;
            _SearchFarmStaysRequest.Child = reqSerach.Child;

            _SearchFarmStaysRequest.CurrencyCode = Helper.GetCurrentCurrency();

            var obj = _FrontFarmStaysSearchService.GetFarmStayslist(WebRequestResponseServiceContext, _SearchFarmStaysRequest);

            var PerNight = (Convert.ToDateTime(reqSerach.CheckOut) - Convert.ToDateTime(reqSerach.CheckIn)).Days;

            return Json(new { FarmStaysObj = obj, PerNight = PerNight }, JsonRequestBehavior.AllowGet);
        }

    }
}