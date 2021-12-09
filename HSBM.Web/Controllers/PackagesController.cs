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
    public class PackagesController : BaseController
    {
        FrontFarmStaysSearchService _FrontFarmStaysSearchService = new FrontFarmStaysSearchService();

        public ActionResult Index()
        {
            FarmStaysRequestModel req = new FarmStaysRequestModel();
            req.CheckIn = DateTime.Now.AddDays(1).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
            req.CheckOut = DateTime.Now.AddDays(2).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); ;
            req.Guests = 2;
            req.IsSolo = false;
            req.IsPackage = true;
            return View(req);
        }

    }
}