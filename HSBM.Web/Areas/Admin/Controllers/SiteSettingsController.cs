using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.CurrencyMaster;
using HSBM.EntityModel.SiteSettings;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class SiteSettingsController : BaseController
    {
        SiteSettingService _SiteSettingService = new SiteSettingService();

        [CustomAuthorizeAction(Module.SiteSettings, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }

            return View();
        }

        [CustomAuthorizeAction(Module.SiteSettings, ModuleAccess.CanView)]
        public JsonResult GetAllSiteSettingBySearchRequest(SiteSettingsRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);
                GridDataResponse _GridDataResponse = _SiteSettingService.GetAllSiteSettingBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

                if (_GridDataResponse.data != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception, JsonRequestBehavior.AllowGet);
            }
        }

        //[CustomAuthorizeAction(Module.SiteSettings, ModuleAccess.CanAdd)]
        //public ActionResult AddSiteSetting()
        //{
        //    ViewBag.Message = "";
        //    SiteSettings siteSettings = new SiteSettings();
        //    siteSettings.CurrencyList = new List<CurrencyMasterResponse>();

        //    var currencies = new CurrencyService().GetAllCurrencyForDropDown();
        //    currencies.ForEach(t =>
        //    {
        //        siteSettings.CurrencyList.Add(new CurrencyMasterResponse()
        //        {
        //            Id = Convert.ToInt32(t.Value),
        //            Name = t.Text
        //        });
        //    });

        //    return PartialView("AddUpdateSiteSettings", siteSettings);
        //}

        [CustomAuthorizeAction(Module.SiteSettings, ModuleAccess.CanUpdate)]
        public ActionResult UpdateSiteSetting(long Id)
        {
            ViewBag.Message = "";
            SiteSettings siteSettings = new SiteSettings();

            try
            {
                siteSettings = _SiteSettingService.GetSiteSettingById(WebRequestResponseServiceContext, Id);

                if (siteSettings != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    if (siteSettings.SiteSettingId == (int)Common.Enums.SiteSettingEnum.DefaultCurrency)
                    {
                        var currencies = new CurrencyService().GetAllCurrencyForDropDown();
                        if (currencies.Any())
                        {
                            currencies.ForEach(t => { t.Value = t.Text; });
                            siteSettings.CurrencyList = currencies.AsEnumerable();
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }

                    return PartialView("AddUpdateSiteSettings", siteSettings);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUpdateSiteSetting(SiteSettings siteSettings)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (siteSettings.Id > 0)
                    {
                        siteSettings.UpdatedBy = SessionProxy.UserDetails.Id;
                        siteSettings.UpdateDate = DateTime.Now;
                    }
                    else
                    {
                        siteSettings.CreatedBy = SessionProxy.UserDetails.Id;
                        siteSettings.CreatedDate = DateTime.Now;
                    }

                    int Affected = _SiteSettingService.AddorUpdateSiteSetting(WebRequestResponseServiceContext, siteSettings);

                    if (Affected == 1)
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Setting has been updated succefully", Type = "success", ErrorTitle = "Success" };
                        return RedirectToAction("Index");
                    }

                    ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                    return PartialView("AddUpdateSiteSettings", siteSettings);

                }
                catch
                {

                    ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                }
            }

            return PartialView("AddUpdateSiteSettings", siteSettings);

        }

        //[CustomAuthorizeAction(Module.SiteSettings, ModuleAccess.CanDelete)]
        //public ActionResult DeleteSiteSetting(long Id)
        //{
        //    _SiteSettingService.DeleteSiteSettingById(WebRequestResponseServiceContext, Id);
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAction(Module.SiteSettings, ModuleAccess.CanUpdate)]
        public JsonResult ActiveAndInactiveSwitchUpdate(SiteSettings siteSettings)
        {
            _SiteSettingService.ActiveAndInactiveSwitchUpdateForSS(WebRequestResponseServiceContext, siteSettings);
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}
