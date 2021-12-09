using HSBM.Common.Utils;
using HSBM.EntityModel.SystemUsers;
using HSBM.Repository.Contracts;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HSBM.Common.Enums;
using HSBM.EntityModel.Common;
using HSBM.Service.Services;
using HSBM.EntityModel.ForgotPassword;
using HSBM.EntityModel.CityMaster;
namespace HSBM.Web.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {

        #region Init
        CountryService _CountryService = new CountryService();
        CityService _CityService = new CityService();
        RegionService _RegionService = new RegionService();
        RoleManagementService _RoleManagementService = new RoleManagementService();
        SystemUserService _SystemUserService = new SystemUserService();
        #endregion

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (SessionProxy.UserDetails != null)
            {
                SessionProxy.UserDetails = null;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        #region Forgot Password

        public ActionResult ForgotPassword()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel _ForgotPasswordModel)
        {
            try
            {
                string _ChangePasswordUrl = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + @"Admin/Account/ResetPassword";
                _ForgotPasswordModel.UserType = Convert.ToInt32(UserTypes.Admin);
                _ForgotPasswordModel = _SystemUserService.forgotPassword(WebRequestResponseServiceContext, _ForgotPasswordModel);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && _ForgotPasswordModel != null)
                {
                    string p_CallBackUrl = _ChangePasswordUrl + "?p_PasswordKey=" + Helper.Encrypt(_ForgotPasswordModel.Id.ToString());

                    EntityModel.EmailTemplates.EmailTemplates template = new EntityModel.EmailTemplates.EmailTemplates();
                    EmailTemplateService emailTemplateService = new EmailTemplateService();
                    template = emailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.ForgotPassword);

                    if (template != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                    {
                        template.TemplatesHtml = template.TemplatesHtml.Replace("#UserName#", _ForgotPasswordModel.FirstName);
                        template.TemplatesHtml = template.TemplatesHtml.Replace("#href#", p_CallBackUrl);
                    }
                    else
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "There is some error in sending reset password link to given email address", Type = "error", ErrorTitle = "Error" };
                        return RedirectToAction("ForgotPassword", _ForgotPasswordModel);
                    }

                    bool _Success = Helper.SendMail(_ForgotPasswordModel.Email, template.Subject, template.TemplatesHtml, string.Empty, string.Empty);
                    if (_Success)
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Successfully sent reset password link to given email address , please verify it", Type = "success", ErrorTitle = "Success" };
                        return RedirectToAction("ForgotPassword", new ForgotPasswordModel());
                    }
                    else
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "There is some error in sending reset password link to given email address", Type = "error", ErrorTitle = "Error" };
                        return RedirectToAction("ForgotPassword", _ForgotPasswordModel);
                    }
                }
                else
                {
                    TempData["ToastrMSG"] = new ToastrMSG() { Message = "Email Address With no one user found, please try again later with valid email Address!", Type = "error", ErrorTitle = "Error" };
                    return RedirectToAction("ForgotPassword", _ForgotPasswordModel);
                }
            }
            catch (Exception _exception)
            {
                throw _exception;
            }

            return RedirectToAction("ForgotPassword", _ForgotPasswordModel);
        }

        #endregion

        # region Reset Password Get Method
        /// <summary>
        /// Reset Password Get Method
        /// </summary>
        /// <param name="p_PasswordKey">Password Key</param>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult ResetPassword(string p_PasswordKey)
        {
            try
            {
                if (string.IsNullOrEmpty(p_PasswordKey))
                {
                    return RedirectToAction("ForgotPassword", "Account");
                }
                int AdminId = Convert.ToInt32(Helper.Decrypt(p_PasswordKey));
                if (AdminId > 0)
                {
                    ResetPasswordModel _ResetPasswordModel = new ResetPasswordModel();
                    _ResetPasswordModel.Id = AdminId;
                    return View(_ResetPasswordModel);
                }
            }
            catch (Exception exception)
            {
                throw (exception);
            }
            return null;
        }
        # endregion

        # region Reset Password post Method
        /// <summary>
        /// Reset Password post Method
        /// </summary>
        /// <param name="commonRequestDTO">Common Request DTO</param>
        /// <returns>Response</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel _ResetPasswordModel)
        {
            try
            {
                bool result = false;
                SystemUsers _systemUsers = new SystemUsers();
                _systemUsers = _SystemUserService.GetSystemUserByKey(WebRequestResponseServiceContext, _ResetPasswordModel.Id);
                if (_systemUsers != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    _ResetPasswordModel.ConfirmPassword = Helper.Encrypt(_ResetPasswordModel.ConfirmPassword);
                    result = _SystemUserService.ResetPasswordById(WebRequestResponseServiceContext, _ResetPasswordModel);
                    if (result)
                    {
                        if (!string.IsNullOrEmpty(_systemUsers.UserName) && !string.IsNullOrEmpty(_ResetPasswordModel.ConfirmPassword))
                        {
                            SessionProxy.UserDetails = _SystemUserService.SystemUserLogin(WebRequestResponseServiceContext, new LoginUser() { UserName = _systemUsers.UserName, Password = _ResetPasswordModel.ConfirmPassword, UserType = Convert.ToInt32(UserTypes.Admin) });

                            var xxx = SessionProxy.UserDetails;

                            if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                return RedirectToAction("Login");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Password Not Updated Successfully";
                        return PartialView("ResetPassword", _ResetPasswordModel);
                    }
                }
                else
                {
                    ViewBag.Message = "User Not Found!";
                    return PartialView("ResetPassword", _ResetPasswordModel);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        # endregion

        [CustomAuthorize]
        public ActionResult ChangePassword()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }
            return View();
        }

        [CustomAuthorize]
        [HttpPost]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            SystemUsers user = new SystemUsers();
            user = _SystemUserService.GetSystemUserByKey(WebRequestResponseServiceContext, SessionProxy.UserDetails.Id);
            bool result = false;

            if (user != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
            {
                if (user.Password == Helper.Encrypt(OldPassword) && NewPassword == ConfirmPassword && OldPassword != NewPassword)
                {
                    result = _SystemUserService.ChangeSystemUserPassword(SessionProxy.UserDetails.Id, Helper.Encrypt(NewPassword));
                    if (result)
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Password has been successfully changed ", Type = "success", ErrorTitle = "Success" };
                        SessionProxy.UserDetails.Password = Helper.Encrypt(NewPassword);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Password has not been successfully changed", Type = "error", ErrorTitle = "error" };
                        return Redirect(Request.UrlReferrer.PathAndQuery);
                    }
                }
                else
                {
                    TempData["ToastrMSG"] = new ToastrMSG() { Message = "Old Password has not been matched ", Type = "error", ErrorTitle = "Error" };
                    //return RedirectToAction("ChangePassword", "Home");
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                    
                }                
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult LogOff()
        {
            SessionProxy.UserDetails = null;

            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login(SystemUsers p_SystemUser)
        {

            if (!string.IsNullOrEmpty(p_SystemUser.UserName) && !string.IsNullOrEmpty(p_SystemUser.Password))
            {
                p_SystemUser.Password = Helper.Encrypt(p_SystemUser.Password);

                p_SystemUser.UserType = _SystemUserService.GetUserType(WebRequestResponseServiceContext, p_SystemUser);
                if (!(p_SystemUser.UserType == (int)UserTypes.Admin || p_SystemUser.UserType == (int)UserTypes.SubAdmin))
                {
                    ViewBag.UserPassword = "Incorrect Email Address/Password.";
                    return View();
                }

                SessionProxy.UserDetails = _SystemUserService.SystemUserLogin(WebRequestResponseServiceContext, new LoginUser() { UserName = p_SystemUser.UserName, Password = p_SystemUser.Password, UserType = (int)p_SystemUser.UserType });

                var xxx = SessionProxy.UserDetails;

                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.CURRENT_PASSWORD_INCORRECT)
                {
                    ViewBag.UserPassword = "Incorrect Email Address/Password.";
                }
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.NOT_FOUND)
                {
                    ViewBag.UserPassword = "Incorrect Email Address/Password.";
                }
                
            }
            return View();
        }

        public JsonResult Signup(SystemUsers p_SystemUser)
        {
            try
            {
                if (p_SystemUser.Id == 0)
                {
                    p_SystemUser.CreatedDate = CommonDateTimeFunction.GetCurrentCstDateTime();
                    p_SystemUser.IsActive = true;
                }
                else
                {
                    p_SystemUser.UpdatedDate = CommonDateTimeFunction.GetCurrentCstDateTime();
                }

                p_SystemUser.UserType = (int)HSBM.Common.Enums.UserTypes.User;
                p_SystemUser.RoleMasterID = (int)HSBM.Common.Enums.DefaultRole.NonSubscriber;
                p_SystemUser.Password = Helper.Encrypt(p_SystemUser.Password);

                _SystemUserService.AddUpdateSystemUser(WebRequestResponseServiceContext, p_SystemUser);

                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return JsonSuccessResponse(WebRequestResponseServiceContext.Response.StatusMessage);
                }

                return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorizeAction(Module.SystemUsers, ModuleAccess.CanUpdate)]
        public JsonResult ActiveAndInactiveSwitchUpdate(SystemUsers systemUsers)
        {
            _SystemUserService.ActiveAndInactiveSwitchUpdate(systemUsers);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        private void GetCountryDropDown()
        {
            ViewBag.CountryDropDown = _CountryService.GetAllCountry();
        }
        private void GetRegionDropDown(long CountryId)
        {
            ViewBag.RegionDropDown = _RegionService.GetAllRegionById(CountryId);
        }
        private void GetCityDropDown(long RegionId)
        {
            if (RegionId != 0)
            {
                ViewBag.CityDropDown = _CityService.CityDropDown(RegionId);
            }
            else
            {
                List<SelectListItem> _List = new List<SelectListItem>();
                _List.Add(new SelectListItem() { Text = "Select", Value = "" });
                ViewBag.CityDropDown = _List;
            }
        }
        private void GetRoleDropDown()
        {
            ViewBag.RoleDropDown = _RoleManagementService.RoleDropDown();
        }
        private void GetGenderDropDown()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            lst.Add(new SelectListItem() { Text = "Female", Value = "Female" });
            ViewBag.GenderDropDown = lst;
        }

        [CustomAuthorize]
        public ActionResult Profile()
        {
            SystemUserService _SystemUserService = new SystemUserService();
            SystemUsers _SystemUsers = _SystemUserService.GetSystemUserByKey(WebRequestResponseServiceContext, SessionProxy.UserDetails.Id);
            //GetCountryDropDown();
            //GetGenderDropDown();

            //if (_SystemUsers.CountryMasterID.Value > 0)
            //{
            //    GetRegionDropDown(_SystemUsers.CountryMasterID.Value);
            //}
            //else
            //{
            //    ViewBag.RegionDropDown = new List<SelectListItem>();
            //}

            //if (_SystemUsers.RegionMasterID != null && _SystemUsers.RegionMasterID.Value > 0)
            //{
            //    GetCityDropDown(_SystemUsers.RegionMasterID.Value);
            //}
            //else
            //{
            //    ViewBag.CityDropDown = new List<SelectListItem>();
            //}

            return View(_SystemUsers);
        }

        [CustomAuthorize]
        public ActionResult UpdateProfile(SystemUsers p_SystemUsers)
        {
            if (p_SystemUsers != null)
            {
                SystemUserService _SystemUserService = new SystemUserService();

                _SystemUserService.AddUpdateSystemUser(WebRequestResponseServiceContext, p_SystemUsers);

                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    SessionProxy.UserDetails.FirstName = p_SystemUsers.FirstName;
                    SessionProxy.UserDetails.LastName = p_SystemUsers.LastName;
                    TempData["ToastrMSG"] = new ToastrMSG() { Message = "Your profile has been updated successfully", Type = "success", ErrorTitle = "Success" };
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.ToastrMSG = new ToastrMSG() { Message = "Somthing went wrong!", Type = "error", ErrorTitle = "Error" };
            return PartialView("Profile", p_SystemUsers);
        }

        #region Sub User
        [CustomAuthorize]
        [CustomAuthorizeAction(Module.SystemUsers, ModuleAccess.CanView)]
        public ActionResult Subuser()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }
            return View();
        }

        [CustomAuthorizeAction(Module.SystemUsers, ModuleAccess.CanView)]
        public JsonResult GetAllSubuserBySearchRequest(SystemUsersRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Id";

                p_SearchRequest.ParentId = SessionProxy.UserDetails.Id;
                GridDataResponse _GridDataResponse = _SystemUserService.GetAllSubUsersBySearchRequest(p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);

            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        [CustomAuthorize]
        [CustomAuthorizeAction(Module.SystemUsers, ModuleAccess.CanAdd)]
        public ActionResult AddSubuser()
        {
            GetCountryDropDown();
            GetRegionDropDown(0);
            GetCityDropDown(0);
            GetRoleDropDown();
            GetGenderDropDown();

            return PartialView("AddUpdateSubUser", new SystemUsers());
        }

        [CustomAuthorize]
        [CustomAuthorizeAction(Module.SystemUsers, ModuleAccess.CanUpdate)]
        public ActionResult UpdateSubuser(long Id)
        {
            var _subuser = _SystemUserService.GetSubUsersByIdAndParentId(Id, SessionProxy.UserDetails.Id);
            _subuser.Password = Helper.Decrypt(_subuser.Password);

            GetCountryDropDown();
            GetRegionDropDown(_subuser.CountryMasterID.HasValue ? _subuser.CountryMasterID.Value : 0);
            GetCityDropDown(_subuser.RegionMasterID.HasValue ? _subuser.RegionMasterID.Value : 0);
            GetRoleDropDown();
            GetGenderDropDown();
            return PartialView("AddUpdateSubUser", _subuser);
        }

        [CustomAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUpdateSubUser(SystemUsers systemUsers)
        {
            long result = 0;
            systemUsers.IsVerify = true;
            SystemUsers temp = new SystemUsers();

            if (systemUsers.Id > 0)
            {
                temp = _SystemUserService.GetSubUsersByIdAndParentId(systemUsers.Id, SessionProxy.UserDetails.Id);

                temp.RoleMasterID = systemUsers.RoleMasterID;
                //temp.UserName = systemUsers.UserName;
                temp.Password = systemUsers.Password;
                temp.FirstName = systemUsers.FirstName;
                temp.LastName = systemUsers.LastName;
                temp.MiddleName = systemUsers.MiddleName;
                temp.Email = systemUsers.Email;
                temp.Telephone = systemUsers.Telephone;
                temp.Mobile = systemUsers.Mobile;
                temp.Gender = systemUsers.Gender;
                temp.Address = systemUsers.Address;
                temp.Address2 = systemUsers.Address2;
                temp.CountryMasterID = systemUsers.CountryMasterID;
                temp.RegionMasterID = systemUsers.RegionMasterID;
                temp.CityMasterID = systemUsers.CityMasterID;
                temp.IsActive = systemUsers.IsActive;
                systemUsers.UpdatedBy = SessionProxy.UserDetails.Id;
                systemUsers.UpdatedDate = DateTime.Now;
                systemUsers = temp;

            }
            else
            {
                systemUsers.ParentId = SessionProxy.UserDetails.Id;
                if (SessionProxy.UserDetails.UserType == (int)HSBM.Common.Enums.UserTypes.Admin)
                {
                    systemUsers.UserType = (int)HSBM.Common.Enums.UserTypes.SubAdmin;
                }

                systemUsers.CreatedBy = SessionProxy.UserDetails.Id;
                systemUsers.CreatedDate = DateTime.Now;
            }
            if (systemUsers.Password != null)
            {
                systemUsers.Password = Helper.Encrypt(systemUsers.Password);
            }

            _SystemUserService.DuplicateUsernameOrEmail(WebRequestResponseServiceContext, systemUsers);
            if (!(WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.IS_EXIST))
            {
                if (_SystemUserService.AddUpdateSystemUser(WebRequestResponseServiceContext, systemUsers))
                {
                    if (systemUsers.Id > 0)
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "SystemUser has been updated succefully", Type = "success", ErrorTitle = "Success" };

                    else
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "SystemUser has been added succefully", Type = "success", ErrorTitle = "Success" };
                    return RedirectToAction("Subuser");
                }
                else
                {
                    TempData["ToastrMSG"] = new ToastrMSG() { Message = WebRequestResponseServiceContext.Response.StatusMessage, Type = "error", ErrorTitle = "Error" };
                    //ViewBag.Message = WebRequestResponseServiceContext.Response.StatusMessage;
                }
            }
            else
            {
                //TempData["ToastrMSG"] = new ToastrMSG() { Message = "Username / Email already in use", Type = "error", ErrorTitle = "Error" };
                ViewBag.Message = "Username / Email already in use";
            }

            GetCountryDropDown();
            GetRegionDropDown(systemUsers.CountryMasterID.HasValue ? systemUsers.CountryMasterID.Value : 0);
            GetCityDropDown(systemUsers.RegionMasterID.HasValue ? systemUsers.RegionMasterID.Value : 0);
            GetRoleDropDown();
            GetGenderDropDown();
            //GetCurrencyDropDown();
            return PartialView("AddUpdateSubUser", systemUsers);
        }

        [CustomAuthorize]
        [CustomAuthorizeAction(Module.SystemUsers, ModuleAccess.CanDelete)]
        public ActionResult DeleteSubuser(long Id)
        {
            _SystemUserService.DeleteSystemUserByUserId(WebRequestResponseServiceContext, Id);
            return RedirectToAction("Subuser");
        }
        #endregion
    }
}