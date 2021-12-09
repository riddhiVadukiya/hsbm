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
using HSBM.EntityModel.EmailTemplates;
namespace HSBM.Web.Controllers
{
    public class AccountController : BaseController
    {

        #region Init
        CountryService _CountryService = new CountryService();
        CityService _CityService = new CityService();
        RegionService _RegionService = new RegionService();
        RoleManagementService _RoleManagementService = new RoleManagementService();
        SystemUserService _SystemUserService = new SystemUserService();
        EmailTemplateService _EmailTemplateService;
        #endregion

        /// <summary>
        /// Customer signup
        /// </summary>
        /// <returns> View </returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Customer login form
        /// </summary>
        /// <returns> View </returns>
        public ActionResult Login()
        {
            return View();
        }

        #region Forgot Password

        /// <summary>
        /// Customer Forgot Password Form
        /// </summary>
        /// <returns> View </returns>
        public ActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Customer forgot password Post method
        /// </summary>
        /// <param name="_ForgotPasswordModel"> Object of ForgotPasswordModel </param>
        /// <returns> View </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel _ForgotPasswordModel)
        {
            try
            {
                string _ChangePasswordUrl = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + @"/Account/ResetPassword";
                _ForgotPasswordModel.UserType = Convert.ToInt32(UserTypes.Admin);
                _ForgotPasswordModel = _SystemUserService.forgotPassword(WebRequestResponseServiceContext, _ForgotPasswordModel);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && _ForgotPasswordModel != null)
                {
                    string p_CallBackUrl = _ChangePasswordUrl + "?p_PasswordKey=" + Helper.Encrypt(_ForgotPasswordModel.Id.ToString());

                    //p_CallBackUrl = "<a href=\"" + p_CallBackUrl + "\" target='_blank'>Reset Password</a>";

                    //string _Link = string.Empty;

                    //string _Description = "<p>hi " + _ForgotPasswordModel.FirstName + ",</p><p>To reset your password, please go to following page:</p></br>" + p_CallBackUrl + "</br><p>Then log in with your e-mail:" + _ForgotPasswordModel.Email + "</p>" + _Link + "";
                    //bool _Success = Helper.SendMail(_ForgotPasswordModel.Email, "Forgot Password", _Description, string.Empty, string.Empty);
                     EmailTemplates _EmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.ForgotPassword);
                     if (_EmailTemplates != null)
                     {
                         _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", _ForgotPasswordModel.FirstName);
                         _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#href#", p_CallBackUrl);
                         _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#EmailAddress#", _ForgotPasswordModel.Email);

                         bool _Success = Helper.SendMail(_ForgotPasswordModel.Email, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                         if (_Success)
                         {
                             ViewBag.Message = "Successfully sent reset password link to given email address , please verify it";
                             return PartialView("ForgotPassword", _ForgotPasswordModel);
                         }
                     }
                     ViewBag.Message = "There is some error in sending reset password link to given email address";
                     return PartialView("ForgotPassword", _ForgotPasswordModel);
                         
                }
                else
                {
                    ViewBag.Message = "Email Address With no one user found, please try again later with valid email Address!";
                    return PartialView("ForgotPassword", _ForgotPasswordModel);
                }
            }
            catch (Exception _exception)
            {
                throw _exception;
            }
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
                            SessionProxy.CustomerDetails = _SystemUserService.SystemUserLogin(WebRequestResponseServiceContext, new LoginUser() { UserName = _systemUsers.UserName, Password = _ResetPasswordModel.ConfirmPassword, UserType = Convert.ToInt32(UserTypes.Admin) });

                            var xxx = SessionProxy.CustomerDetails;

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

        /// <summary>
        /// Customer change password form
        /// </summary>
        /// <returns> View </returns>
        [CustomAuthorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Customer change password post method
        /// </summary>
        /// <param name="OldPassword">Old Password</param>
        /// <param name="NewPassword">New Password</param>
        /// <param name="ConfirmPassword">Confirm Password</param>
        /// <returns> View </returns>
        [CustomAuthorize]
        [HttpPost]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            SystemUsers user = new SystemUsers();
            user = _SystemUserService.GetSystemUserByKey(WebRequestResponseServiceContext, SessionProxy.CustomerDetails.Id);
            bool result = false;

            if (user != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
            {
                if (user.Password == Helper.Encrypt(OldPassword) && NewPassword == ConfirmPassword && OldPassword != NewPassword)
                {
                    result = _SystemUserService.ChangeSystemUserPassword(SessionProxy.CustomerDetails.Id, Helper.Encrypt(NewPassword));
                    if (result)
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Password has been successfully changed ", Type = "success", ErrorTitle = "Success" };
                        SessionProxy.CustomerDetails.Password = Helper.Encrypt(NewPassword);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ToastrMSG"] = new ToastrMSG() { Message = "Password has not been successfully changed", Type = "success", ErrorTitle = "Success" };
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["ToastrMSG"] = new ToastrMSG() { Message = "Old Password has not been matched ", Type = "error", ErrorTitle = "Error" };
                    return RedirectToAction("Index", "Home");

                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
            //if (OldPassword != NewPassword && NewPassword == ConfirmPassword && _SystemUserService.ChangeSystemUserPassword(SessionProxy.CustomerDetails.Id, Helper.Encrypt(OldPassword), Helper.Encrypt(NewPassword)))
            //{
            //    return RedirectToAction("Index", "Home");
            //}


            //return View();
        }

        /// <summary>
        /// Customer log off
        /// </summary>
        /// <returns> View </returns>
        [HttpPost]
        public ActionResult LogOff()
        {
            SessionProxy.CustomerDetails = null;

            return RedirectToAction("Login");
        }

        /// <summary>
        /// Customer login post method
        /// </summary>
        /// <param name="p_SystemUser"> Object of SystemUsers </param>
        /// <returns> View </returns>
        [HttpPost]
        public ActionResult Login(SystemUsers p_SystemUser)
        {

            if (!string.IsNullOrEmpty(p_SystemUser.UserName) && !string.IsNullOrEmpty(p_SystemUser.Password))
            {
                p_SystemUser.UserType = (int)UserTypes.Admin;
                p_SystemUser.Password = Helper.Encrypt(p_SystemUser.Password);
                SessionProxy.CustomerDetails = _SystemUserService.SystemUserLogin(WebRequestResponseServiceContext, new LoginUser() { UserName = p_SystemUser.UserName, Password = p_SystemUser.Password });

                var xxx = SessionProxy.CustomerDetails;

                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        /// <summary>
        /// Customer signup post method
        /// </summary>
        /// <param name="p_SystemUser"> Object of SystemUsers </param>
        /// <returns> Json of Success/Failure message </returns>
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

        [CustomAuthorize]
        public ActionResult Subuser()
        {
            return View();
        }

        public JsonResult GetAllSubuserBySearchRequest(SystemUsersRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Id";

                p_SearchRequest.ParentId = SessionProxy.CustomerDetails.Id;
                GridDataResponse _GridDataResponse = _SystemUserService.GetAllSubUsersBySearchRequest(p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);

            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        [CustomAuthorize]
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
        public ActionResult UpdateSubuser(long Id)
        {
            var _subuser = _SystemUserService.GetSubUsersByIdAndParentId(Id, SessionProxy.CustomerDetails.Id);
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

            SystemUsers temp = new SystemUsers();

            if (systemUsers.Id > 0)
            {
                temp = _SystemUserService.GetSubUsersByIdAndParentId(systemUsers.Id, SessionProxy.CustomerDetails.Id);

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
                systemUsers.UpdatedBy = SessionProxy.CustomerDetails.Id;
                systemUsers.UpdatedDate = DateTime.Now;
                systemUsers = temp;

            }
            else
            {
                systemUsers.ParentId = SessionProxy.CustomerDetails.Id;
                if (SessionProxy.CustomerDetails.UserType == (int)HSBM.Common.Enums.UserTypes.Admin)
                {
                    systemUsers.UserType = (int)HSBM.Common.Enums.UserTypes.SubAdmin;
                }

                systemUsers.CreatedBy = SessionProxy.CustomerDetails.Id;
                systemUsers.CreatedDate = DateTime.Now;
            }
            if (systemUsers.Password != null)
            {
                systemUsers.Password = Helper.Encrypt(systemUsers.Password);
            }

            if (_SystemUserService.AddUpdateSystemUser(WebRequestResponseServiceContext, systemUsers))
            {
                return RedirectToAction("Subuser");
            }

            GetCountryDropDown();
            GetRegionDropDown(systemUsers.CountryMasterID.HasValue ? systemUsers.CountryMasterID.Value : 0);
            GetCityDropDown(systemUsers.RegionMasterID.HasValue ? systemUsers.RegionMasterID.Value : 0);
            GetRoleDropDown();
            GetGenderDropDown();
            return PartialView("AddUpdateSubUser", systemUsers);
        }

        [CustomAuthorize]
        public ActionResult DeleteSubuser(long Id)
        {
            _SystemUserService.DeleteSystemUserByUserId(WebRequestResponseServiceContext, Id);
            return RedirectToAction("Subuser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ActiveAndInactiveSwitchUpdate(SystemUsers systemUsers)
        {
            _SystemUserService.ActiveAndInactiveSwitchUpdate(systemUsers);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Country DropDown Method
        /// </summary>
        private void GetCountryDropDown()
        {
            ViewBag.CountryDropDown = _CountryService.GetAllCountry();
        }
        /// <summary>
        /// Get Region DropDown Method
        /// </summary>
        /// <param name="CountryId"> CountryId </param>
        private void GetRegionDropDown(long CountryId)
        {
            ViewBag.RegionDropDown = _RegionService.GetAllRegionById(CountryId);
        }
        /// <summary>
        /// Get City DropDown Method
        /// </summary>
        /// <param name="RegionId"> RegionId </param>
        private void GetCityDropDown(long RegionId)
        {
            ViewBag.CityDropDown = _CityService.CityDropDown(RegionId);
        }
        /// <summary>
        /// Get Role DropDown Method
        /// </summary>
        private void GetRoleDropDown()
        {
            ViewBag.RoleDropDown = _RoleManagementService.RoleDropDown();
        }
        /// <summary>
        /// Get Gender DropDown Method
        /// </summary>
        private void GetGenderDropDown()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            lst.Add(new SelectListItem() { Text = "Female", Value = "Female" });
            ViewBag.GenderDropDown = lst;
        }

    }
}