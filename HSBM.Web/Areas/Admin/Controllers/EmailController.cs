using Utils = HSBM.Common.Utils;
using HSBM.EntityModel.SiteSettings;
using HSBM.Service.Contracts;
using HSBM.Web.Helpers;
using HSBM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HSBM.Common.Enums;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class EmailController : BaseController
    {
        EmailViewModel emailViewModel = new EmailViewModel();
                
        public ActionResult Index()
        {
            return View(emailViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]        
        public ActionResult SendMails(EmailViewModel emailViewModel)
        {
            try
            {
                string[] toEmail;
                string[] ccEmail;
                string[] bccEmail;
                List<string> listOfAttachments = new List<string>();
                toEmail = !string.IsNullOrEmpty(emailViewModel.ToEmail) ? emailViewModel.ToEmail.Split(',') : null;
                ccEmail = !string.IsNullOrEmpty(emailViewModel.CC) ? emailViewModel.CC.Split(',') : null;
                bccEmail = !string.IsNullOrEmpty(emailViewModel.BCC) ? emailViewModel.BCC.Split(',') : null;
                foreach (string toemail in toEmail)
                {
                    Utils.Helper.SendMail(toemail, emailViewModel.Subject, emailViewModel.Body, emailViewModel.BCC, emailViewModel.CC);
                }
            }
            catch { }
            return Redirect("/Admin/Home/Index");
        }
    }
}