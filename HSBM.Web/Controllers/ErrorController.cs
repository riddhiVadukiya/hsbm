using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}