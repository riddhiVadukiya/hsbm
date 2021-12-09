using HSBM.EntityModel.CMSPageMaster;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Controllers
{
    public class PageController : BaseController
    {
        [Route("Page/{Page}")]
        public ActionResult Index(string Page)
        {
            CMSPageMaster cmsPage = new CMSPageMaster();

            try
            {
                CMSPageService service = new CMSPageService();
                Common.Enums.CMSPages CMSPage = (Common.Enums.CMSPages)Enum.Parse(typeof(Common.Enums.CMSPages), Page);
                cmsPage = service.GetCMSPageByIdForFront(WebRequestResponseServiceContext, (int)CMSPage);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && cmsPage != null)
                {
                    return View(cmsPage);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
