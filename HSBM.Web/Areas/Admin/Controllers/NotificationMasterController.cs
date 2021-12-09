using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.Notification;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class NotificationMasterController : BaseController
    {
        NotificationService _NotificationService = new NotificationService();

        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetAllNotification()
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Title";

                GridDataResponse _GridDataResponse = _NotificationService.GetAllNotification(p_GridParams);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);

            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public ActionResult AddNotification()
        {
            return PartialView("AddUpdateNotification", new Notification());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUpdateNotification(Notification _Notification)
        {
            _Notification.UserType = Convert.ToInt32(UserTypes.Customer);
            _Notification.CreatedBy = SessionProxy.UserDetails.Id;
            _Notification.CreatedDate = DateTime.Now;

            int Affected = _NotificationService.AddNotification(_Notification);

            if (Affected == 1)
            {
                return RedirectToAction("List");
            }
            else
            {
                return PartialView("AddUpdateNotification", new Notification());
            }
        }

        public ActionResult ViewNotification(long Id)
        {
            Notification _Notification = new Notification();

            _Notification = _NotificationService.GetNotificationById(Id);

            return PartialView("ViewNotification", _Notification);
        }

        public ActionResult DeleteNotification(long Id)
        {
            _NotificationService.DeleteNotificationById(Id);

            return RedirectToAction("List");
        }
    }
}