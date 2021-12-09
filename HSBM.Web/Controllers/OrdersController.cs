using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.Front;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Controllers
{
    public class OrdersController : BaseController
    {
        FrontOrdersService _OrdersService = new FrontOrdersService();
        //
        // GET: /Orders/
        [CustomAuthorize]
        public ActionResult Index()
        {
            List<SelectListItem> _List = new List<SelectListItem>();
            foreach (BookingStatus item in Enum.GetValues(typeof(BookingStatus)))
            {
                _List.Add(new SelectListItem() { Text = Helper.GetEnumDescription(item), Value = Convert.ToString((int)item) });
            }
            ViewBag.OrderStatus = _List;

            List<SelectListItem> _ListFrontFarmStays = new List<SelectListItem>();

            FrontFarmStaysSearchService _farmStaysService = new FrontFarmStaysSearchService();
            _ListFrontFarmStays = _farmStaysService.GetFarmStaysForDropDown();
            ViewBag.FrontFarmStays = _ListFrontFarmStays;
            return View();
        }
        [CustomAuthorize]
        public JsonResult GetAllOrdersBySearchRequest(FrontOrdersMasterRequest p_SearchRequest)
        {
            try
            {
                p_SearchRequest.CustomerId = SessionProxy.CustomerDetails.Id;
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (p_GridParams.DefaultOrderBy.Contains("strCreatedDate"))
                {
                    p_GridParams.DefaultOrderBy = p_GridParams.DefaultOrderBy.Replace("strCreatedDate", "CreatedDate");
                }

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "OrderDate desc";

                p_SearchRequest.CurrencyCode = Helper.GetCurrentCurrency();
                GridDataResponse _GridDataResponse = _OrdersService.GetAllOrdersBySearchRequest(p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }
        [CustomAuthorize]
        public ActionResult ViewOrderDetail(long id)
        {
            string Id = Helper.Encrypt(id.ToString());
            string CurrencyCode = Helper.GetCurrentCurrency();
            FrontOrdersMaster ordersMaster = _OrdersService.GetOrderDetailByKey(id, CurrencyCode);
            return View(ordersMaster);
        }
        public ActionResult ViewGuestOrderDetail(string OId)
        {
            if(!string.IsNullOrEmpty( OId)){
            long id = Convert.ToInt32( Helper.Decrypt(OId));
            string CurrencyCode = Helper.GetCurrentCurrency();
            FrontOrdersMaster ordersMaster = _OrdersService.GetOrderDetailByKey(id, CurrencyCode);
            if (ordersMaster != null)
                ordersMaster.IsGuestUser = true;
            return View("ViewOrderDetail",ordersMaster);
            }
            return RedirectToAction("Index", "Home");
        }
        //#region Order Status

        //public JsonResult GetOrderStatusForDropDown()
        //{

        //    List<SelectListItem> _List = new List<SelectListItem>();
        //    foreach (OrderStatus item in Enum.GetValues(typeof(OrderStatus)))
        //    {
        //        _List.Add(new SelectListItem() { Text = Helper.GetEnumDescription(item), Value = Convert.ToString((int)item) });
        //    }
        //    return Json(_List, JsonRequestBehavior.AllowGet);
        //}

        //#endregion


        //public JsonResult GetFarmStaysForDropDown()
        //{
        //    //List<FrontFarmStays> _List = new List<FrontFarmStays>();

        //    //FrontFarmStaysSearchService _farmStaysService = new FrontFarmStaysSearchService();
        //    //_List = _farmStaysService.GetFarmStaysForDropDown();
        //    //return Json(_List, JsonRequestBehavior.AllowGet);
        //    return Json(null, JsonRequestBehavior.AllowGet);
        //}
	}
}