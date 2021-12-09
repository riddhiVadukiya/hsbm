using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.EmailTemplates;
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
    public class EmailTemplatesController : BaseController
    {
        EmailTemplateService _EmailTemplateService = new EmailTemplateService();

        [CustomAuthorizeAction(Module.EmailTemplates, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }
            return View();
        }

        [CustomAuthorizeAction(Module.EmailTemplates, ModuleAccess.CanAdd)]
        public ActionResult AddEmailTemplates()
        {
            ViewBag.Message = "";
            return PartialView("AddUpdateEmailTemplates", new EmailTemplates());
        }

        [CustomAuthorizeAction(Module.EmailTemplates, ModuleAccess.CanUpdate)]
        public ActionResult UpdateEmailTemplates(long Id)
        {
            EmailTemplates template = new EmailTemplates();
            try
            {
                template = _EmailTemplateService.GetEmailTemplatesById(WebRequestResponseServiceContext, Id);
                if (template != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return PartialView("AddUpdateEmailTemplates", template);
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

        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUpdateEmailTemplates(EmailTemplates emailTemplates)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    emailTemplates.IsActive = true;
                    if (emailTemplates.Id > 0)
                    {
                        emailTemplates.UpdatedBy = SessionProxy.UserDetails.Id;
                        emailTemplates.UpdateDate = DateTime.Now;
                    }
                    else
                    {
                        emailTemplates.CreatedBy = SessionProxy.UserDetails.Id;
                        emailTemplates.CreatedDate = DateTime.Now;
                    }

                    int Affected = _EmailTemplateService.AddOrUpdateEmailTemplates(WebRequestResponseServiceContext, emailTemplates);

                    if (Affected == 1 && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                    {
                        if (emailTemplates.Id > 0)
                        {
                            TempData["ToastrMSG"] = new ToastrMSG() { Message = "Email Template has been updated successfully.", Type = "success", ErrorTitle = "Success" };
                        }
                        else
                        {
                            TempData["ToastrMSG"] = new ToastrMSG() { Message = "Email Template has been added successfully.", Type = "success", ErrorTitle = "Success" };
                        }
                        return RedirectToAction("Index");
                    }
                    else if (Affected == 2 && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Email Template  Already Exist.", Type = "success", ErrorTitle = "Success" };
                      //  ViewBag.Message = "EmailTemplate Name Already Exist";
                        return PartialView("AddUpdateEmailTemplates", emailTemplates);
                    }
                    else
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Error in Add/Update Email Template.", Type = "error", ErrorTitle = "Error" };
                       // ViewBag.Message = "Error in Add/Update EmailTemplate";
                        return PartialView("AddUpdateEmailTemplates", emailTemplates);
                    }
                }
                catch
                {
                    TempData["ToastrMSG"] = new ToastrMSG() { Message = "Error in Add/Update Email Template.", Type = "error", ErrorTitle = "Error" };
                    return PartialView("AddUpdateEmailTemplates", emailTemplates);
                }
            }
            else
            {                
                return PartialView("AddUpdateEmailTemplates", emailTemplates);
            }
        }

        [CustomAuthorizeAction(Module.EmailTemplates, ModuleAccess.CanDelete)]
        public ActionResult DeleteEmailTemplates(long Id)
        {
            _EmailTemplateService.DeleteEmailTemplatesById(WebRequestResponseServiceContext, Id);
            return RedirectToAction("Index");
        }

        [CustomAuthorizeAction(Module.EmailTemplates, ModuleAccess.CanView)]
        public JsonResult GetAllEmailTemplatesBySearchRequest(EmailTemplatesRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Id";

                GridDataResponse _GridDataResponse = _EmailTemplateService.GetAllEmailTempalatesBySearchRequest(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ActiveAndInactiveSwitchUpdate(EmailTemplates emailTemplates)
        {
            _EmailTemplateService.ActiveAndInactiveSwitchUpdateForET(WebRequestResponseServiceContext, emailTemplates);
            return Json("", JsonRequestBehavior.AllowGet);
        }

    }
}