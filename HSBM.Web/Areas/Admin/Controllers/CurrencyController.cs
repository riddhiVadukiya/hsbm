using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.CurrencyMaster;
using HSBM.Service.Contracts;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class CurrencyController : BaseController
    {
        CurrencyService _CurrencyService = new CurrencyService();

        [CustomAuthorizeAction(Module.MultiCurrency, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllCurrencyBySearchRequest(CurrencyMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Id";

                GridDataResponse _GridDataResponse = _CurrencyService.GetAllCurrencyBySearchRequest(p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        //[CustomAuthorizeAction(Module.Currency, ModuleAccess.CanAdd)]
        public ActionResult AddCurrency()
        {
            ViewBag.Message = "";
            return PartialView("AddUpdateCurrency", new CurrencyMaster());
        }

        //[CustomAuthorizeAction(Module.Currency, ModuleAccess.CanUpdate)]
        public ActionResult UpdateCurrency(long Id)
        {
            ViewBag.Message = "";
            return PartialView("AddUpdateCurrency", _CurrencyService.GetCurrencyById(Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUpdateCurrency(CurrencyMaster currencyMaster)
        {
            if (currencyMaster.Id > 0)
            {
                var _Currency = _CurrencyService.GetCurrencyById(currencyMaster.Id);
                currencyMaster.CreatedBy = _Currency.CreatedBy;
                currencyMaster.CreatedDate = _Currency.CreatedDate;
                currencyMaster.UpdatedBy = SessionProxy.UserDetails.Id;
                currencyMaster.UpdateDate = DateTime.Now;
            }
            else
            {
                currencyMaster.CreatedBy = SessionProxy.UserDetails.Id;
                currencyMaster.CreatedDate = DateTime.Now;
            }

            int Affected = _CurrencyService.AddorUpdateCurrency(currencyMaster);
            if (Affected == 1)
            {
                return RedirectToAction("Index");
            }
            else if (Affected == 2)
            {
                ViewBag.Message = "CurrencyName Already Exist";
                return PartialView("AddUpdateCurrency", currencyMaster);
            }
            else
            {
                ViewBag.Message = "Error in Add/Edit Currency";
                return PartialView("AddUpdateCurrency", currencyMaster);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[CustomAuthorizeAction(Module.Currency, ModuleAccess.CanUpdate)]
        public JsonResult ActiveAndInactiveSwitchUpdate(CurrencyMaster currencyMaster)
        {
            _CurrencyService.ActiveAndInactiveSwitchUpdateForCur(currencyMaster);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[CustomAuthorizeAction(Module.Currency, ModuleAccess.CanUpdate)]
        public JsonResult UpdateLatestCurrencyRate(CurrencyMaster currencyMaster)
        {
            CurrencyService _CurrencyService = new CurrencyService();
            var currencies = _CurrencyService.GetAllCurrencyForDropDown();
            var _baseCurrency = currencies.Where(t => t.Selected).Select(t => t.Text).FirstOrDefault();
            string url = string.Format("http://apilayer.net/api/live?access_key={0}&currencies={1}&source={2}", ConfigurationManager.AppSettings["CurrencyRateAPIKey"], string.Join(",", (currencies.Where(t => t.Selected == false).Select(t => t.Text).ToList())), _baseCurrency);

            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            Dictionary<string, decimal> latestRates = new Dictionary<string, decimal>();
            latestRates.Add(_baseCurrency, 1);


            var responseObj = JsonConvert.DeserializeObject<CurrencyLayer>(responseString);

            foreach (var item in responseObj.quotes)
            {
                latestRates.Add(item.Key.Replace(_baseCurrency, ""), Convert.ToDecimal(item.Value));
            }

            _CurrencyService.UpdateCurrencyRates(latestRates, SessionProxy.UserDetails.Id);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        //[CustomAuthorizeAction(Module.Currency, ModuleAccess.CanDelete)]
        public ActionResult DeleteCurrency(long Id)
        {
            _CurrencyService.DeleteCurrencyById(Id);
            return RedirectToAction("Index");
        }
    }
}