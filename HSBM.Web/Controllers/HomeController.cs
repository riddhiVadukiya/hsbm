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
    public class HomeController : BaseController
    {        

        /// <summary>
        /// Hoem page
        /// </summary>
        /// <returns> View </returns>
        public ActionResult Index()
        {
            HSBM.Common.Utils.Helper.GetCurrentCurrency();
            try
            {
                FrontFarmStaysHomeService _FrontFarmStaysHomeService = new FrontFarmStaysHomeService();
                List<FarmStaysHomeResponse> _ListFarmStaysPopularFarmstayResponse = new List<FarmStaysHomeResponse>();
                GridDataResponse _GridDataResponse = _FrontFarmStaysHomeService.GetAllBannerBySearchRequest(WebRequestResponseServiceContext);
                if (_GridDataResponse.data != null)
                {
                    _ListFarmStaysPopularFarmstayResponse = (List<FarmStaysHomeResponse>)_GridDataResponse.data;
                    return View(_ListFarmStaysPopularFarmstayResponse);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception _Exception)
            {
                //return JsonErrorResponse(_Exception);
            }
            return View();
        }

        public JsonResult GetAllBanner()
        {
            try
            {
                FrontFarmStaysHomeService _FrontFarmStaysHomeService = new FrontFarmStaysHomeService();

                GridDataResponse _GridDataResponse = _FrontFarmStaysHomeService.GetAllBannerBySearchRequest(WebRequestResponseServiceContext);
                if (_GridDataResponse.data != null)
                {
                    return Json(_GridDataResponse.data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }
	}
}