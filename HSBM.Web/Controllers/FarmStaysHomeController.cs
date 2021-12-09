using HSBM.EntityModel.Blogs;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.Front;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Controllers
{
    public class FarmStaysHomeController : BaseController
    {
        //
        // GET: /FarmStaysHome/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllBanner()
        {
            try
            {
                FrontFarmStaysHomeService _FrontFarmStaysHomeService = new FrontFarmStaysHomeService();

                GridDataResponse _GridDataResponse = _FrontFarmStaysHomeService.GetAllBannerBySearchRequest(WebRequestResponseServiceContext);
                if (_GridDataResponse.data != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
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

        public JsonResult GetAllPopularBlog()
        {
            List<FrontBlogResponse> _List = new List<FrontBlogResponse>();
            BlogService _BlogService = new BlogService();
            try
            {
                _List = _BlogService.GetAllPopularBlog(WebRequestResponseServiceContext);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return JsonSuccessResponse(_List, string.Empty);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public JsonResult GetAllPopularFarmStay()
        {
            List<FarmStaysPopularFarmstayResponse> _List = new List<FarmStaysPopularFarmstayResponse>();
            FrontFarmStaysHomeService _FrontFarmStaysHomeService = new FrontFarmStaysHomeService();
            BlogService _BlogService = new BlogService();
            try
            {
                _List = _FrontFarmStaysHomeService.GetAllPopularFarmStay(WebRequestResponseServiceContext);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return JsonSuccessResponse(_List, string.Empty);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public JsonResult GetAllFarmStaysDiscount()
        {
            List<FarmStaysDiscountResponse> _List = new List<FarmStaysDiscountResponse>();
            FrontFarmStaysHomeService _FrontFarmStaysHomeService = new FrontFarmStaysHomeService();
            BlogService _BlogService = new BlogService();
            try
            {
                _List = _FrontFarmStaysHomeService.GetAllFarmStaysDiscount(WebRequestResponseServiceContext);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return JsonSuccessResponse(_List, string.Empty);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }       
	}
}