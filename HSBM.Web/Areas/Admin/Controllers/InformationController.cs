using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class InformationController : Controller
    {
        //
        // GET: /Information/
        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult AccessDeniedOperation()
        {
            return View();
        }
	}
}