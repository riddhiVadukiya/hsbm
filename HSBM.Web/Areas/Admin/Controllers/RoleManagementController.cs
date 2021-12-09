using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.Service.ServiceContext;
using HSBM.EntityModel.RoleMaster;
using HSBM.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using HSBM.Web.Helpers;
using HSBM.Service.Services;
using HSBM.EntityModel.RoleMasterDetails;
using HSBM.Common.Enums;

namespace HSBM.Web.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class RoleManagementController : BaseController
    {
        RoleManagementService _RoleManagementService = new RoleManagementService();

        [CustomAuthorizeAction(Module.Roles, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }

            return View();
        }

        [CustomAuthorizeAction(Module.Roles, ModuleAccess.CanAdd)]
        public ActionResult Add()
        {
            var RoleObject = _RoleManagementService.GetRoleMasterForAdd(true);
            return PartialView("Edit", RoleObject);
        }
        
        [CustomAuthorizeAction(Module.Roles, ModuleAccess.CanUpdate)]
        public ActionResult Edit(long Id)
        {
            var RoleObject = _RoleManagementService.GetRoleMasterById(Id);
            return View(RoleObject);
        }
        
        [CustomAuthorizeAction(Module.Roles, ModuleAccess.CanView)]
        public ActionResult View(long Id)
        {
            var RoleObject = _RoleManagementService.GetRoleMasterById(Id);
            return View(RoleObject);
        }

        [CustomAuthorizeAction(Module.Roles, ModuleAccess.CanDelete)]
        public ActionResult Delete(long Id)
        {
            bool result = _RoleManagementService.UsersExistInRole(WebRequestResponseServiceContext, Id);
            if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && result == true)
            {
                ViewBag.Message = "Role can not be deleted because Users exist in the role";
                return View("Index");
            }

            if (_RoleManagementService.DeleteRoleMasterById(Id))
            {
                ViewBag.Message = "Success";
            }
            else
            {
                ViewBag.Message = "Error";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult AddUpdateRole(RoleMaster roleMaster)
        {
            try
            {

                roleMaster.IsAdmin = true;

                bool previlegeselected = false;
                foreach (RoleMasterDetails roledetail in roleMaster.RoleMasterDetails)
                {
                    if (roledetail.CanAdd || roledetail.CanUpdate || roledetail.CanDelete || roledetail.CanView)
                    {
                        previlegeselected = true;
                        break;
                    }
                }
                if (!previlegeselected)
                {
                    var RoleObject = _RoleManagementService.GetRoleMasterForAdd(true);
                    ViewBag.ToastrMSG = new ToastrMSG() { Message = "Please select privilege(s).", Type = "error", ErrorTitle = "Error" };
                    return PartialView("Edit", RoleObject);
                }

                int Affected = _RoleManagementService.AddUpdateRoleMaster(WebRequestResponseServiceContext, roleMaster);

                if (Affected > 0 && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    #region Broadcast NewRole
                    //IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AppHub>();
                    //context.Clients.All.BroadcastNewRole(Helper.Encrypt(JsonConvert.SerializeObject(roleMaster)));
                    #endregion

                    #region Apply Role
                    var _currentUserData = SessionProxy.UserDetails;
                    if (roleMaster != null && roleMaster.Id == _currentUserData.RoleMasterID)
                    {
                        _currentUserData.RoleMasterDetails = roleMaster.RoleMasterDetails.Where(t => t.CanView).ToList();// || t.CanUpdate || t.CanDelete || t.CanAdd).ToList();
                    }
                    SessionProxy.UserDetails = _currentUserData;

                    #endregion

                    if (roleMaster.Id > 0)
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Role has been updated succefully", Type = "success", ErrorTitle = "Success" };
                    }
                    else
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Role has been added succefully", Type = "success", ErrorTitle = "Success" };
                    }
                    return RedirectToAction("Index");
                }
                else if (Affected > 0 && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.IS_EXIST)
                {
                    ModelState.Clear();
                    ViewBag.ToastrMSG = new ToastrMSG() { Message = "Role already exist.", Type = "error", ErrorTitle = "Error" };
                    var RoleObject = _RoleManagementService.GetRoleMasterForAdd(true);
                    return PartialView("Edit", RoleObject);
                }
                else
                {
                    ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                    var RoleObject = _RoleManagementService.GetRoleMasterForAdd(true);                    
                    return PartialView("Edit", RoleObject);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
                var RoleObject = _RoleManagementService.GetRoleMasterForAdd(true);
                return PartialView("Edit", RoleObject);
            }
        }

        public JsonResult GetAllRolesForGrid(RoleMasterRequest p_SearchRequest)
        {
            try
            {
                p_SearchRequest.IsAdmin = true;
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);
                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Id";

                // p_GridParams = null;

                GridDataResponse _GridDataResponse = _RoleManagementService.GetAllRoleMasterForGrid(WebRequestResponseServiceContext, p_GridParams, p_SearchRequest);

                //if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && _GridDataResponse.data != null) // && _GridDataResponse.recordsTotal > 0)
                //{
                    //_GridDataResponse.draw = p_GridParams.draw;
                    //return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);

                    return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
                //}
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAction(Module.Roles, ModuleAccess.CanUpdate)]
        public JsonResult ActiveAndInactiveSwitchUpdate(RoleMaster roleMaster)
        {

            try
            {
                _RoleManagementService.ActiveAndInactiveSwitchUpdateForRole(WebRequestResponseServiceContext,roleMaster);
                
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage, JsonRequestBehavior.AllowGet);
            }            
        }
    }
}