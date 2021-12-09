using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.SubscriptionMaster;
using HSBM.Service.Contracts;
using HSBM.Service.Services;
using HSBM.Web.Areas.Admin.Models;
using HSBM.Web.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class SubscriptionController : BaseController
    {
        SubscriptionService _SubscriptionService = new SubscriptionService();
        public GridView ExcelGridView { get; set; }
        public string fileName { get; set; }

        [CustomAuthorizeAction(Module.Subscribers, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Export(string CreatedFromDate, string CreatedToDate, int ExportTypeExcel)
        {
            SubscriptionMasterRequest p_SubscriptionMasterRequest = new SubscriptionMasterRequest();
            p_SubscriptionMasterRequest.CreatedDateFrom = CreatedFromDate;
            p_SubscriptionMasterRequest.CreatedDateTo = CreatedToDate;
            GridParams p_GridParams = new GridParams();
            p_GridParams.take = int.MaxValue;

            if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                p_GridParams.DefaultOrderBy = "Id";

            GridView gv = new GridView();
            GridDataResponse _GridDataResponse = new GridDataResponse();
            _GridDataResponse = _SubscriptionService.GetAllSubscriptionBySearchRequest(p_GridParams, p_SubscriptionMasterRequest);
            var data = (from s in ((List<SubscriptionMasterResponse>)_GridDataResponse.data) select new { SubscribedDate = s.CreatedDate.ToString("dd-MM-yyyy"), Email = s.Email, Status = Convert.ToString(s.IsActive) });
            gv.DataSource = data;
            gv.DataBind();


            if (Convert.ToBoolean(ExportTypeExcel))
            {
                return new ExportToExcel((GridView)gv, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_Subscription");
            }
            else
            {

                List<object> _data = new List<object>();
                _data.AddRange(data);

                List<string> _header = new List<string>();
                _header.Add("SubscribedDate");
                _header.Add("Email");
                _header.Add("Status");

                // string _CSVString = covertToCsv<object>(_data, _header);
                return new ExportToCSV(_data, _header, DateTime.Now.ToString("yyyyMMdd_hhmmss") + "_Subscription");
            }
        }



        public JsonResult GetAllSubscriptionBySearchRequest(SubscriptionMasterRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (p_GridParams.DefaultOrderBy.Contains("strCreatedDate"))
                {
                    p_GridParams.DefaultOrderBy = p_GridParams.DefaultOrderBy.Replace("strCreatedDate","CreatedDate");
                }

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Id";

                GridDataResponse _GridDataResponse = _SubscriptionService.GetAllSubscriptionBySearchRequest(p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        [CustomAuthorizeAction(Module.Subscribers, ModuleAccess.CanAdd)]
        public ActionResult AddSubscription()
        {
            ViewBag.Message = "";
            return View(new SubscriptionMaster());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAction(Module.Subscribers, ModuleAccess.CanAdd)]
        public ActionResult AddSubscription(SubscriptionMaster subscriptionMaster)
        {
            subscriptionMaster.IsActive = true;
            subscriptionMaster.CreatedDate = DateTime.Now;

            int Affected = _SubscriptionService.AddUpdateSubscription(subscriptionMaster);

            if (Affected == 1)
            {
                return RedirectToAction("Information", new { msg = "Thanks for Subscription" });
            }
            else if (Affected == 2)
            {
                ViewBag.Message = "Email Alredy Exist";
                return PartialView("AddUpdateSubscription", subscriptionMaster);
            }
            else
            {
                ViewBag.Message = "Error in add Subscription";
                return PartialView("AddUpdateSubscription", subscriptionMaster);
            }
        }

        [CustomAuthorizeAction(Module.Subscribers, ModuleAccess.CanDelete)]
        public ActionResult DeleteSubscription(long Id)
        {
            _SubscriptionService.DeleteSubscriptionById(Id);
            return RedirectToAction("Index");
        }

        public ActionResult Unsubscribe(long Id)
        {
            _SubscriptionService.UnsubscribeUser(Id);
            return RedirectToAction("Information", new { msg = "Unsubscribe successful" });
        }

        public ActionResult UnsubscribeUserByAdmin(long Id)
        {
            _SubscriptionService.UnsubscribeUser(Id);
            return RedirectToAction("Index");
        }

        public ActionResult Information(string msg)
        {
            ViewBag.msg = msg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAction(Module.Subscribers, ModuleAccess.CanUpdate)]
        public JsonResult ActiveAndInactiveSwitchUpdate(SubscriptionMaster subscriptionMaster)
        {
            _SubscriptionService.ActiveAndInactiveSwitchUpdateForSC(subscriptionMaster);
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}