using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.CurrencyMaster;
using HSBM.EntityModel.EmailTemplates;
using HSBM.EntityModel.RoleMaster;
using HSBM.Service.Contracts;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Controllers
{
    public class CommonFrontController : BaseController
    {
        CountryService _CountryService = new CountryService();
        CityService _CityService = new CityService();
        BannerService _BannerService = new BannerService();
        LocationsService _LocationsService = new LocationsService();
        CurrencyService _CurrencyService;
        EmailTemplateService _EmailTemplateService = new EmailTemplateService();
        /// <summary>
        /// Method to check if user is logged in
        /// </summary>
        /// <returns> True/False </returns>
        public JsonResult CheckUserIsLoggedIn()
        {
            return Json(SessionProxy.CheckUserIsLogin(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get country list
        /// </summary>
        /// <returns> Json of Country list </returns>
        public JsonResult GetCountryList()
        {
            return Json(_CountryService.GetAllCountry(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get all destinations
        /// </summary>
        /// <returns> Json of City list </returns>
        public JsonResult GetAllDestination()
        {
            return Json(_CityService.GetAllCity(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Get all city by Country Id
        /// </summary>
        /// <param name="p_CountryId"> Country Id </param>
        /// <returns> Json of City list </returns>
        public JsonResult GetAllCityByCountryId(long p_CountryId)
        {
            return Json(_CityService.GetAllCityByCountryId(p_CountryId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to Get all banners
        /// </summary>
        /// <returns> Json of all Banners </returns>
        public JsonResult GetAllBanners()
        {
            return Json(_BannerService.GetAllBanners(WebRequestResponseServiceContext), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to get all top destination city
        /// </summary>
        /// <returns> Json of all top destination city list </returns>
        public JsonResult GetAllTopDestinationCity()
        {
            return Json(_LocationsService.GetAllTopDestinationCity(), JsonRequestBehavior.AllowGet);
        }

       
        /// <summary>
        /// Method to get all currency for Dropdown
        /// </summary>
        /// <returns> Json of currency list </returns>
        public JsonResult GetAllCurrencyForDropDown()
        {
            _CurrencyService = new CurrencyService();
            List<SelectListItem> _List = new List<SelectListItem>();
            _List = _CurrencyService.GetAllCurrencyForDropDown();

            if (SessionProxy.BaseCurrency == string.Empty || SessionProxy.BaseCurrency == null)
            {
                SessionProxy.BaseCurrency = _List.FirstOrDefault(x => x.Selected).Text;
            }
            else
            {
                _List.ForEach(t => {
                    t.Selected = t.Text == SessionProxy.BaseCurrency;
                });
            }

            return Json(_List, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to change base currency for current session
        /// </summary>
        /// <param name="id"> Currency Id</param>
        /// <returns> Json of success response </returns>
        public JsonResult ChangeBaseCurrency(int id)
        {
            CurrencyMaster master = new CurrencyMaster();
            _CurrencyService = new CurrencyService();
            master = _CurrencyService.GetCurrencyById(id);
            SessionProxy.BaseCurrency = master.CurrencyCode;
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCMSPages()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                foreach (Common.Enums.CMSPages _EnumValue in Enum.GetValues(typeof(Common.Enums.CMSPages)))
                {
                    list.Add(new SelectListItem() { Text = Helper.GetEnumDescription( _EnumValue), Value = (_EnumValue).ToString() });
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendSubscribeRequest(string email)
        {
            if (email != string.Empty && email != null)
            {
                SubscriptionService service = new SubscriptionService();

                try
                {
                    EntityModel.SubscriptionMaster.SubscriptionMaster subscription = new EntityModel.SubscriptionMaster.SubscriptionMaster();
                    subscription.CreatedDate = DateTime.UtcNow;
                    subscription.Email = email;
                    subscription.FirstName = string.Empty;
                    subscription.LastName = string.Empty;
                    subscription.UpdateDate = null;
                    subscription.IsActive = true;
                    int result = service.AddUpdateSubscription(subscription);
                    if (result == 1)
                    {
                        EmailTemplates _EmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.Subscription);
                        if (_EmailTemplates != null)
                        {
                            bool resultMail = Helper.SendMail(email, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                            return JsonSuccessResponse("You are successfully subscribed.", JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (result == 2)
                    {
                        return JsonSuccessResponse("You are already subscribed.", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return JsonErrorResponse("Your subscription request is not completed. Please try again.", JsonRequestBehavior.AllowGet);
                    }
                }
                catch
                {
                    return JsonErrorResponse("Your subscription request is not completed. Please try again.", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return JsonErrorResponse("", JsonRequestBehavior.AllowGet);
            }
        }

    }
}