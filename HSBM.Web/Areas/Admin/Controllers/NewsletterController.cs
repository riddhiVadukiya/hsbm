using HSBM.EntityModel.Common;
using HSBM.EntityModel.SubscriptionMaster;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class NewsletterController : Controller
    {

        EmailTemplateService _EmailTemplateService = new EmailTemplateService();
        SubscriptionService _SubscriptionService = new SubscriptionService();
        RequestResponseServiceContext requestResponseServiceContext = new RequestResponseServiceContext();

        private void GetEmailTemplatesDropDown()
        {
            ViewBag.EmailTemplatesDropDownDropDown = _EmailTemplateService.EmailTemplatesDropDown(requestResponseServiceContext);
        }

        public ActionResult Index()
        {
            GetEmailTemplatesDropDown();

            return View(new Newsletter() { Users = _SubscriptionService.GetAllSubscription() });
        }

        public JsonResult GetEmailTemplate(long Id)
        {
            return Json(_EmailTemplateService.GetEmailTemplatesById(requestResponseServiceContext, Id), JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult SendNewsLetter(Newsletter newsletter)
        {

            foreach (var item in newsletter.Users.Where(t => t.IsChecked))
            {
                new Thread(t =>
                {
                    HSBM.Common.Utils.Helper.SendMail(item.Email, newsletter.Subject, newsletter.TemplateHtml, string.Empty, string.Empty);
                });

            }

            return RedirectToAction("Index");
        }
    }
}