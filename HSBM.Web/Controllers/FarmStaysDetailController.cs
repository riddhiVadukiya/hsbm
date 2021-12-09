using HSBM.Common.Utils;
using HSBM.EntityModel.Front;
using HSBM.EntityModel.FrontEnd;
using HSBM.Service.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Controllers
{
    public class FarmStaysDetailController : BaseController
    {
        FrontFarmStaysDetailService _FrontFarmStaysDetailService = new FrontFarmStaysDetailService();
        // GET: FarmStaysDetail
        public ActionResult Index(int FarmStayId,string CheckIn,string CheckOut,int Guests,bool IsSolo=false,int Child=0)
        {
            FarmStaysDetail _FarmStaysDetail = new FarmStaysDetail();
            ViewBag.FarmStayId = FarmStayId;
            //ViewBag.CheckIn = CheckIn;
            //ViewBag.CheckOut = CheckOut;
            //ViewBag.Guests = Guests;
            //ViewBag.IsSolo = IsSolo;
                if (Convert.ToDateTime(CheckIn) <= DateTime.Now)
                {
                    CheckIn = (DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"));
                    CheckOut = (DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"));
                }


            SearchFarmStaysRequest _SearchFarmStays = new SearchFarmStaysRequest();
            _SearchFarmStays.FarmStayId = FarmStayId;
            _SearchFarmStays.CheckOut = CheckOut;
            _SearchFarmStays.CheckIn = CheckIn;
            _SearchFarmStays.Guests = Guests;
            _SearchFarmStays.IsSolo = IsSolo;

            _FarmStaysDetail = _FrontFarmStaysDetailService.GetFarmStayDetailByFarmStayId(_SearchFarmStays);

            _FarmStaysDetail.FarmStayId = FarmStayId;
            _FarmStaysDetail.CheckOut = CheckOut;
            _FarmStaysDetail.CheckIn = CheckIn;
            _FarmStaysDetail.Guests = Guests;
            _FarmStaysDetail.IsSolo = IsSolo;

            return View(_FarmStaysDetail);
        }
        public ActionResult GetAvailableRoom(SearchFarmStaysRequest SearchFarmStaysRequest)
        {
            RoomDetails _RoomDetails = new RoomDetails();

            SearchFarmStaysRequest.CurrencyCode = Helper.GetCurrentCurrency();

            _RoomDetails = _FrontFarmStaysDetailService.GetAvailableRoom(SearchFarmStaysRequest);
            return Json(_RoomDetails, JsonRequestBehavior.AllowGet);
        }
    }
}