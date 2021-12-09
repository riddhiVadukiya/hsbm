using HSBM.EntityModel.Common;
using HSBM.Service.ServiceContext;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The RequestResponseServiceContext.
        /// </summary>
        private readonly RequestResponseServiceContext _requestResponseServiceContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        public BaseController()
        {
            _requestResponseServiceContext = new RequestResponseServiceContext();
        }

        /// <summary>
        /// Gets the WebRequestResponseServiceContext.
        /// </summary>
        public RequestResponseServiceContext WebRequestResponseServiceContext { get { return _requestResponseServiceContext; } }

        
        protected override JsonResult Json(object p_Data, string p_ContentType, System.Text.Encoding p_ContentEncoding, JsonRequestBehavior p_JsonRequestBehavior)
        {
            return new JsonDotNetResult
            {
                Data = p_Data,
                ContentType = p_ContentType,
                ContentEncoding = p_ContentEncoding,
                JsonRequestBehavior = p_JsonRequestBehavior,
            };

        }

        protected JsonResult JsonDateTimeResponse(object p_Data, JsonRequestBehavior p_JsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            return new JsonDotNetResult
            {
                Data = p_Data,
                JsonRequestBehavior = p_JsonRequestBehavior,
                DateFormat = "MM/dd/yyyy hh:mm:ss tt"
            };

        }

        protected JsonResult JsonErrorResponse(Exception p_Exception, JsonRequestBehavior p_JsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            _ILogger.Error(p_Exception.Message, p_Exception);

            var response = new JsonResponse()
            {
                Message = p_Exception.Message,
                Errors = null
            };
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 400;
            return Json(response, p_JsonRequestBehavior);
        }

        protected JsonResult JsonHttpResponse(string p_ErrorMessage, HttpStatusCode p_StatusCode, JsonRequestBehavior p_JsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            var response = new JsonResponse()
            {
                Message = p_ErrorMessage,
                Errors = null
            };
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)p_StatusCode;
            return Json(response, p_JsonRequestBehavior);
        }

        protected JsonResult JsonErrorResponse(string p_ErrorMessage, JsonRequestBehavior p_JsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            var response = new JsonResponse()
            {
                Message = p_ErrorMessage,
                Errors = null
            };
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 400;
            return Json(response, p_JsonRequestBehavior);
        }

        protected JsonResult JsonSuccessResponse(object p_CustomData, string p_SuccessMessage, JsonRequestBehavior p_JsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            var response = new JsonResponse()
            {
                Data = p_CustomData,
                Message = p_SuccessMessage,
                Errors = null
            };

            Response.StatusCode = 200;
            return Json(response, p_JsonRequestBehavior);
        }

        protected JsonResult JsonSuccessResponse(string p_SuccessMessage, JsonRequestBehavior p_JsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            var response = new JsonResponse()
            {
                Data = null,
                Message = p_SuccessMessage,
                Errors = null
            };
            Response.StatusCode = 200;
            return Json(response, p_JsonRequestBehavior);
        }

     
    }
}