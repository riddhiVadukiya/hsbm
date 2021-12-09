using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.FarmStays;
using HSBM.EntityModel.OrdersMaster;
using HSBM.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        OrdersService _OrdersService = new OrdersService();
        //
        // GET: /Admin/Orders/
        public ActionResult Index()
        {
            ViewBag.farmstaysid = "";
            if (Request.QueryString["farmstaysid"] != null)
            {
                ViewBag.farmstaysid = Convert.ToString(Request.QueryString["farmstaysid"]);
            }

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

        public JsonResult GetAllOrdersBySearchRequest(OrdersMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (p_GridParams.DefaultOrderBy.Contains("strCreatedDate"))
                {
                    p_GridParams.DefaultOrderBy = p_GridParams.DefaultOrderBy.Replace("strCreatedDate", "CreatedDate");
                }

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Id";

                GridDataResponse _GridDataResponse = _OrdersService.GetAllOrdersBySearchRequest(p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public ActionResult ViewOrderDetail(long id)
        {
            OrdersMaster ordersMaster = _OrdersService.GetOrderDetailByKey(id);
            return View(ordersMaster);
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
        //    List<FarmStays> _List = new List<FarmStays>();

        //    FarmStaysService _farmStaysService = new FarmStaysService();
        //    _List = _farmStaysService.GetFarmStaysForDropDown();
        //    return Json(_List, JsonRequestBehavior.AllowGet);
        //}

    }
}