using HSBM.EntityModel.CurrencyMaster;
using HSBM.Service.Services;
using HSBM.Web.Controllers;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.SiteSettings;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using HSBM.EntityModel.CMSPageMaster;

namespace HSBM.Web.Models
{
    public class LayoutModels
    {
        public static JsonResult GetAllCurrency()
        {
            CurrencyService _CurrencyService = new CurrencyService();
            List<SelectListItem> _List = new List<SelectListItem>();
            _List = _CurrencyService.GetAllCurrencyForDropDown();

            if (SessionProxy.BaseCurrency == string.Empty || SessionProxy.BaseCurrency == null)
            {
                SessionProxy.BaseCurrency = _List.FirstOrDefault(x => x.Selected).Text;
            }
            else
            {
                _List.ForEach(t =>
                {
                    t.Selected = t.Text == SessionProxy.BaseCurrency;
                });
            }

            return Json(_List, JsonRequestBehavior.AllowGet);
            //return CommonFrontController.GetAllCurrencyForDropDown();
        }
        //public static List<SiteSettings>  GetAllSiteSettingBySearchRequest()
        //{
        //    RequestResponseServiceContext requestResponseServiceContext = new RequestResponseServiceContext();
        //    List<SiteSettings> _ListSiteSettings = new List<SiteSettings>();
        //    SiteSettingService _SiteSettingService = new SiteSettingService();            
        //    return _ListSiteSettings = _SiteSettingService.GetSiteSettingRecord(requestResponseServiceContext);
        //}
        public static SiteSettings GetSiteSettingById(long id)
        {
            RequestResponseServiceContext requestResponseServiceContext = new RequestResponseServiceContext();
            SiteSettings _SiteSettings = new SiteSettings();
            SiteSettingService _SiteSettingService = new SiteSettingService();
            return _SiteSettings = _SiteSettingService.GetSiteSettingRecordForFront(requestResponseServiceContext, id);
        }
        public static CMSPageMaster GetCMSPageByIdForFront(long id)
        {
            RequestResponseServiceContext requestResponseServiceContext = new RequestResponseServiceContext();
            CMSPageMaster _CMSPageMaster = new CMSPageMaster();
            CMSPageService _CMSPageService = new CMSPageService();
            return _CMSPageMaster = _CMSPageService.GetCMSPageByIdForFront(requestResponseServiceContext, id);
        }
        public static CMSPageMaster GetCMSPageById(long id)
        {
            RequestResponseServiceContext requestResponseServiceContext = new RequestResponseServiceContext();
            CMSPageMaster _CMSPageMaster = new CMSPageMaster();
            CMSPageService _CMSPageService = new CMSPageService();
            return _CMSPageMaster = _CMSPageService.GetCMSPageById(requestResponseServiceContext, id);
        }        
        protected static JsonResult Json(object p_Data, JsonRequestBehavior p_JsonRequestBehavior)
        {
            return new JsonDotNetResult
            {
                Data = p_Data,                
                JsonRequestBehavior = p_JsonRequestBehavior,
            };

        }
    }
}