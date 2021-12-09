using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.EmailTemplates;
using HSBM.EntityModel.ForgotPassword;
using HSBM.EntityModel.SiteSettings;
using HSBM.EntityModel.SystemUsers;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Controllers;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Controllers
{
    public class CustomerController : BaseController
    {
        CountryService _CountryService = new CountryService();
        CityService _CityService = new CityService();
        RegionService _RegionService = new RegionService();
        SystemUserService _SystemUserService = new SystemUserService();
        EmailTemplateService _EmailTemplateService = new EmailTemplateService();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Customer login form
        /// </summary>
        /// <returns> View </returns>
        public ActionResult Login()
        {
            LoginUser _LoginUser = new LoginUser();
            try
            {
                if (SessionProxy.CustomerDetails != null)
                {
                    HttpCookie _HttpCookie = Request.Cookies["buyerlogin"];
                    if (_HttpCookie != null)
                    {
                        _LoginUser.UserName = _HttpCookie["Email"];
                        _LoginUser.Password = _HttpCookie["Password"];
                        _LoginUser.RememberMe = true;
                    }
                }
                else
                {
                    return View(new LoginUser());
                }
            }
            catch (Exception exception)
            {

            }
            return View(new LoginUser());
        }

        //public ActionResult ThankYou()
        //{
        //    return View();
        //}
        public ActionResult ThankYou(string UserId)
        {
            if (SessionProxy.CustomerDetails == null || SessionProxy.CustomerDetails.Id <= 0)
            {
                Int64 Id = 0;
                if (!String.IsNullOrEmpty(UserId))
                {
                    Id = Convert.ToInt64(Helper.Decrypt(UserId));
                    if (!_SystemUserService.SetVerifyUser(Id))
                    {
                        ViewBag.ErrorMessage = "User does not exist.";
                    }
                }
                return View(Id);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Customer login post method
        /// </summary>
        /// <param name="p_SystemUser"> Object of SystemUsers </param>
        /// <returns> View </returns>
        [HttpPost]
        public ActionResult Login(LoginUser p_SystemUsers)
        {
            if (!string.IsNullOrEmpty(p_SystemUsers.UserName) && !string.IsNullOrEmpty(p_SystemUsers.Password))
            {
                p_SystemUsers.UserType = (int)UserTypes.Customer;
                p_SystemUsers.Password = Helper.Encrypt(p_SystemUsers.Password);
                SessionProxy.CustomerDetails = _SystemUserService.SystemUserLogin(WebRequestResponseServiceContext, p_SystemUsers);

                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Message = "User not found";
            }
            return View();
        }

        /// <summary>
        /// Method to check if user is logged in
        /// </summary>
        /// <param name="p_SystemUsers"> Object of SystemUsers </param>
        /// <returns> Json of success/error response </returns>
        //[HttpPost]
        //public JsonResult Login_User(SystemUsers p_SystemUsers)
        //{
        //    if (!string.IsNullOrEmpty(p_SystemUsers.UserName) && !string.IsNullOrEmpty(p_SystemUsers.Password))
        //    {
        //        p_SystemUsers.UserType = (int)UserTypes.Customer;
        //        p_SystemUsers.Password = Helper.Encrypt(p_SystemUsers.Password);
        //        SessionProxy.CustomerDetails = _SystemUserService.SystemUserLogin(WebRequestResponseServiceContext, p_SystemUsers);

        //        if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
        //        {
        //            return JsonSuccessResponse("Successfully Logged In", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //            return JsonErrorResponse("Error", JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //        return JsonErrorResponse("Error", JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// Method of customer profile
        /// </summary>
        /// <returns> View </returns>
        [CustomAuthorize]
        public ActionResult Profile()
        {
            if (TempData["MSG"] != null)
            {
                ViewBag.Message = TempData["MSG"];
            }
            TempData["MSG"] = null;
            SystemUserService _SystemUserService = new SystemUserService();
            SystemUsers _SystemUsers = _SystemUserService.GetSystemUserByKey(WebRequestResponseServiceContext, SessionProxy.CustomerDetails.Id);
            //GetCountryDropDown();
            GetGenderDropDown();

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
            //    ViewBag.CityDropDown =new List<SelectListItem>();
            //}

            return View(_SystemUsers);
        }

        /// <summary>
        /// Method to update customer profile
        /// </summary>
        /// <param name="p_SystemUsers"> Object of SystemUsers </param>
        /// <returns> View </returns>
        [CustomAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(SystemUsers p_SystemUsers)
        {
            if (p_SystemUsers != null)
            {
                SystemUserService _SystemUserService = new SystemUserService();

                _SystemUserService.AddUpdateSystemUser(WebRequestResponseServiceContext, p_SystemUsers);

                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    SessionProxy.CustomerDetails.FirstName = p_SystemUsers.FirstName;
                    SessionProxy.CustomerDetails.LastName = p_SystemUsers.LastName;
                    TempData["MSG"] = "Profile has been updated successfully.";
                    return RedirectToAction("Profile", "Customer");
                }
            }
            TempData["MSG"] = "Error in save.";
            return RedirectToAction("Profile", "Customer");
        }

        /// <summary>
        /// Method of customer log out
        /// </summary>
        /// <returns> View </returns>
        public ActionResult Logout()
        {
            SessionProxy.CustomerDetails = null;

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Get customer booking history
        /// </summary>
        /// <returns> View </returns>
        public ActionResult BookingHistory()
        {
            return View();
        }

        /// <summary>
        /// Add gender source in ViewBag
        /// </summary>
        private void GetGenderDropDown()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            lst.Add(new SelectListItem() { Text = "Female", Value = "Female" });
            ViewBag.GenderDropDown = lst;
        }

        /// <summary>
        /// Add all country to ViewBag
        /// </summary>
        private void GetCountryDropDown()
        {
            ViewBag.CountryDropDown = _CountryService.GetAllCountry();
        }

        /// <summary>
        /// Add all region to ViewBag
        /// <param name="p_SystemUsers"> CountryId </param>
        /// </summary>
        private void GetRegionDropDown(long CountryId)
        {
            ViewBag.RegionDropDown = _RegionService.GetAllRegionById(CountryId);
        }

        /// <summary>
        /// Add all city to ViewBag
        /// </summary>
        /// <param name="RegionId"> RegionId </param>
        private void GetCityDropDown(long RegionId)
        {
            ViewBag.CityDropDown = _CityService.CityDropDown(RegionId);
        }

        /// <summary>
        /// Get regions list by Country Id
        /// </summary>
        /// <param name="Id"> Country Id </param>
        /// <returns> Json of Regions list </returns>
        public JsonResult GetRegionList(long Id)
        {
            return Json(_RegionService.GetAllRegionById(Id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get city list by Region Id
        /// </summary>
        /// <param name="Id"> Region Id </param>
        /// <returns> Json of City list </returns>
        public JsonResult GetCityList(long Id)
        {
            return Json(_CityService.CityDropDown(Id), JsonRequestBehavior.AllowGet);
        }

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
            if (!string.IsNullOrEmpty(NewPassword) || !string.IsNullOrEmpty(OldPassword))
            {
                if (OldPassword != NewPassword)
                {

                    SystemUsers user = new SystemUsers();
                    user = _SystemUserService.GetSystemUserByKey(WebRequestResponseServiceContext, SessionProxy.CustomerDetails.Id);
                    bool result = false;

                    if (user != null && WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                    {
                        try
                        {
                        if (SessionProxy.CustomerDetails.IsSocialMedia || user.Password == Helper.Encrypt(OldPassword))
                        {
                            if (_SystemUserService.ChangeSystemUserPassword(SessionProxy.CustomerDetails.Id, Helper.Encrypt(NewPassword)))
                            {
                                SessionProxy.CustomerDetails.IsSocialMedia = false;
                                ViewBag.SuccessMessage = "Password has been updated successfully.";
                            }
                            else
                                ViewBag.ErrorMessage = "Error in change password.";
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Old Password has not been matched";
                        }


                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "User not found.";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Old password and New Password are same.";
                }
            }
            else
            {
                ViewBag.Message = "Password should not be empty.";
            }
            return View();
        }

        /// <summary>
        /// Customer signup
        /// </summary>
        /// <returns> View </returns>
        public ActionResult CustomerRegister()
        {
            GetGenderDropDown();
            GetCountryDropDown();
            GetRegionDropDown(0);
            GetCityDropDown(0);
            SystemUsers _SystemUsers = new SystemUsers();
            return View(_SystemUsers);
        }

        /// <summary>
        /// Customer signup post method
        /// </summary>
        /// <param name="p_SystemUsers"> Object of SystemUsers </param>
        /// <returns> View </returns>
        [HttpPost]
        public ActionResult CustomerRegister(SystemUsers p_SystemUsers)
        {
            p_SystemUsers.IsActive = true;
            p_SystemUsers.CreatedDate = DateTime.Now;
            p_SystemUsers.CreatedBy = 0;
            if (p_SystemUsers.Password != null)
            {
                p_SystemUsers.Password = Helper.Encrypt(p_SystemUsers.Password);
            }
            p_SystemUsers.UserType = (int)UserTypes.Customer;
            p_SystemUsers.RoleMasterID = (int)DefaultRole.General;

            int _Result = _SystemUserService.CheckEmailOrUserNameIsExists(WebRequestResponseServiceContext, p_SystemUsers);
            if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && _Result <= 0)
            {
                _SystemUserService.AddUpdateSystemUser(WebRequestResponseServiceContext, p_SystemUsers);
                int Id = _SystemUserService.CheckEmailOrUserNameIsExists(WebRequestResponseServiceContext, p_SystemUsers);

                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    string _ThankYouUrl = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + @"/Customer/ThankYou";
                    string p_CallBackUrl = _ThankYouUrl + "?UserId=" + Helper.Encrypt(Id.ToString());

                    // http://1.22.161.26:9020/Customer/ThankYou
                    EmailTemplates _EmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.Verification);
                    if (_EmailTemplates != null)
                    {
                        //_EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", p_SystemUsers.UserName);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", p_SystemUsers.FirstName + " " + p_SystemUsers.LastName);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#href#", p_CallBackUrl);
                        Helper.SendMail(p_SystemUsers.Email, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                    }
                    EmailTemplates _AdminEmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.CustomerRegistration);
                    if (_AdminEmailTemplates != null)
                    {
                        string adminEmail = System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString();
                        //_EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", p_SystemUsers.UserName);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", p_SystemUsers.FirstName + " " + p_SystemUsers.LastName);
                        Helper.SendMail(adminEmail, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                    }
                    return RedirectToAction("ThankYou", "Customer");
                }
            }
            //GetCountryDropDown();
            //GetRegionDropDown(p_SystemUsers.CountryMasterID.HasValue ? p_SystemUsers.CountryMasterID.Value : 0);
            //GetCityDropDown(p_SystemUsers.RegionMasterID.HasValue ? p_SystemUsers.RegionMasterID.Value : 0);
            //GetGenderDropDown();
            ViewBag.ToastrMSG = "User already exist.";
            return View(p_SystemUsers);
        }

        #region Forgot Password

        /// <summary>
        /// Customer Forgot Password Form
        /// </summary>
        /// <returns> View </returns>
        public ActionResult ForgotPassword()
        {
            if (TempData["ForgotMSG"] != null)
            {
                ViewBag.Message = TempData["ForgotMSG"];
            }
            TempData["ForgotMSG"] = null;
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
                string _ChangePasswordUrl = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + @"/Customer/ResetPassword";
                _ForgotPasswordModel.UserType = Convert.ToInt32(UserTypes.Customer);
                _ForgotPasswordModel = _SystemUserService.forgotPassword(WebRequestResponseServiceContext, _ForgotPasswordModel);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && _ForgotPasswordModel != null)
                {
                    string p_CallBackUrl = _ChangePasswordUrl + "?p_PasswordKey=" + Helper.Encrypt(_ForgotPasswordModel.Id.ToString());

                    //  p_CallBackUrl = "<a href=\"" + p_CallBackUrl + "\" target='_blank'>Reset Password</a>";

                    EmailTemplates _EmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.ForgotPassword);
                    if (_EmailTemplates != null)
                    {
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", _ForgotPasswordModel.FirstName);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#href#", p_CallBackUrl);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#EmailAddress#", _ForgotPasswordModel.Email);

                        bool _Success = Helper.SendMail(_ForgotPasswordModel.Email, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                        if (_Success)
                        {
                            TempData["ForgotMSG"] = "Successfully sent reset password link to given email address , please verify it";
                            return RedirectToAction("ForgotPassword", "Customer");
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
                    return RedirectToAction("ForgotPassword", "Customer");
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
                            SessionProxy.CustomerDetails = _SystemUserService.SystemUserLogin(WebRequestResponseServiceContext, new LoginUser() { UserName = _systemUsers.UserName, Password = _ResetPasswordModel.ConfirmPassword, UserType = Convert.ToInt32(UserTypes.Customer) });

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


        public ActionResult PartialLogin()
        {
            LoginUser loginUser = new LoginUser();
            try
            {
                if (SessionProxy.CustomerDetails == null)
                {
                    HttpCookie _HttpCookie = Request.Cookies["Customerlogin"];
                    if (_HttpCookie != null)
                    {
                        loginUser.UserName = _HttpCookie["Email"];
                        loginUser.Password = _HttpCookie["Password"];
                        loginUser.RememberMe = true;
                    }
                    return PartialView("Partial/_Login", loginUser);
                }
            }
            catch (Exception exception)
            {
                //iLogger.Error("PartialLogin", exception);
                //Error(Messages.Ex_LogIn);
            }
            return PartialView("Partial/_Login", loginUser);
        }

        [HttpGet]
        public JsonResult JsonPartialLogin(string email, string password, bool rememberMe, bool isSocialMedia=false)
        {
            LoginUser loginUser = new LoginUser();
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                if (rememberMe)
                {
                    HttpCookie _HttpCookie = Response.Cookies["Customerlogin"];
                    if (_HttpCookie != null)
                    {
                        _HttpCookie["Email"] = email.ToLower();
                        _HttpCookie["Password"] = password;
                        _HttpCookie.Expires = DateTime.Now.AddYears(1);
                    }
                }
                else
                {
                    HttpCookie _HttpCookie = Response.Cookies["Customerlogin"];
                    if (_HttpCookie != null)
                        _HttpCookie.Expires = DateTime.Now.AddYears(-1);
                }
                loginUser.UserType = (int)UserTypes.Customer;
                loginUser.UserName = email;
                loginUser.Password = password;
                loginUser.Password = Helper.Encrypt(loginUser.Password);
                loginUser.IsSocialMedia = isSocialMedia;
                SessionProxy.CustomerDetails = _SystemUserService.SystemUserLogin(WebRequestResponseServiceContext, loginUser);

                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return Json(new { Success = true, loginUser }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Success = false, Message = "Email or password is incorrect." }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Signup
        /// </summary>
        /// <returns>partial view of signup</returns>               
        public ActionResult PartialSignup()
        {
            try
            {
                return PartialView("Partial/_Signup");
            }
            catch (Exception exception)
            {

            }
            return PartialView("Partial/_Signup");
        }

        public JsonResult JsonSaveSignUp(string firstName, string LastName, string Email, string Password, string Mobile, bool IsSocialMedia = false)
        {
            try
            {
                SystemUsers SystemUsers = new SystemUsers();

                SystemUsers.IsActive = true;
                SystemUsers.CreatedDate = DateTime.Now;
                SystemUsers.CreatedBy = 0;
                SystemUsers.Email = Email;
                SystemUsers.Password = Password;
                SystemUsers.FirstName = firstName;
                SystemUsers.LastName = LastName;
                SystemUsers.Mobile = Mobile;
                SystemUsers.IsSocialMedia = IsSocialMedia;
                if (SystemUsers.IsSocialMedia)
                    SystemUsers.IsVerify = true;

                if (SystemUsers.Password != null)
                {
                    SystemUsers.Password = Helper.Encrypt(SystemUsers.Password);
                }
                SystemUsers.UserType = (int)UserTypes.Customer;
                SystemUsers.RoleMasterID = (int)DefaultRole.General;

                int _Result = _SystemUserService.CheckEmailOrUserNameIsExists(WebRequestResponseServiceContext, SystemUsers);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && _Result <= 0)
                {
                    _SystemUserService.AddUpdateSystemUser(WebRequestResponseServiceContext, SystemUsers);
                    int Id = _SystemUserService.CheckEmailOrUserNameIsExists(WebRequestResponseServiceContext, SystemUsers);
 
                    if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                    {
                        string IdEncrypted= Helper.Encrypt(Id.ToString());
                        if (!SystemUsers.IsSocialMedia)
                        {
                            string _ThankYouUrl = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + @"/Customer/ThankYou";
                            string p_CallBackUrl = _ThankYouUrl + "?UserId=" +IdEncrypted;

                            // http://1.22.161.26:9020/Customer/ThankYou
                            EmailTemplates _EmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.Verification);
                            if (_EmailTemplates != null)
                            {
                                //_EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", p_SystemUsers.UserName);
                                _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", SystemUsers.FirstName + " " + SystemUsers.LastName);
                                _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#href#", p_CallBackUrl);
                                Helper.SendMail(SystemUsers.Email, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                            }
                            EmailTemplates _AdminEmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.CustomerRegistration);
                            if (_AdminEmailTemplates != null)
                            {
                                string adminEmail = System.Configuration.ConfigurationManager.AppSettings["AdminEmail"].ToString();
                                //_EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", p_SystemUsers.UserName);
                                _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", SystemUsers.FirstName + " " + SystemUsers.LastName);
                                Helper.SendMail(adminEmail, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                            }
                            return Json(new { Success = true, Message = "Successfully Registerd." }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { Success = true, Message = "Successfully Registerd.", Data = IdEncrypted}, JsonRequestBehavior.AllowGet);
                        }
                    }
                }                
                return Json(new { Success = false, Message = "User already exist." }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new { Success = false, Message = "Internal server error: " }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult PartialForgotPassword()
        {
            return PartialView("Partial/_ForgotPassword");
        }

        public ActionResult ForgotPasswordfront(string email)
        {
            try
            {
                ForgotPasswordModel _ForgotPasswordModel = new ForgotPasswordModel();
                string _ChangePasswordUrl = String.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~")) + @"/Customer/ResetPassword";
                _ForgotPasswordModel.Email = email;
                _ForgotPasswordModel.UserType = Convert.ToInt32(UserTypes.Customer);
                _ForgotPasswordModel = _SystemUserService.forgotPassword(WebRequestResponseServiceContext, _ForgotPasswordModel);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS && _ForgotPasswordModel != null)
                {
                    string p_CallBackUrl = _ChangePasswordUrl + "?p_PasswordKey=" + Helper.Encrypt(_ForgotPasswordModel.Id.ToString());

                    //  p_CallBackUrl = "<a href=\"" + p_CallBackUrl + "\" target='_blank'>Reset Password</a>";

                    EmailTemplates _EmailTemplates = _EmailTemplateService.GetEmailTemplateByTypeId(WebRequestResponseServiceContext, (int)EmailTemplateType.ForgotPassword);
                    if (_EmailTemplates != null)
                    {
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#UserName#", _ForgotPasswordModel.FirstName);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#href#", p_CallBackUrl);
                        _EmailTemplates.TemplatesHtml = _EmailTemplates.TemplatesHtml.Replace("#EmailAddress#", _ForgotPasswordModel.Email);

                        bool _Success = Helper.SendMail(_ForgotPasswordModel.Email, _EmailTemplates.Subject, _EmailTemplates.TemplatesHtml, string.Empty, string.Empty);
                        if (_Success)
                        {
                            //return Json(_ForgotPasswordModel, JsonRequestBehavior.AllowGet);
                            return Json(new { Success = true, Message = "Successfully sent reset password link to given email address , please verify it", _ForgotPasswordModel }, JsonRequestBehavior.AllowGet);
                            //TempData["ForgotMSG"] = "Successfully sent reset password link to given email address , please verify it";
                            //return RedirectToAction("ForgotPassword", "Customer");
                        }

                    }

                    //ViewBag.Message = "There is some error in sending reset password link to given email address";
                    //return PartialView("ForgotPassword", _ForgotPasswordModel);
                    return Json(new { Success = false, Message = "There is some error in sending reset password link to given email address", _ForgotPasswordModel }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    //ViewBag.Message = "Email Address With no one user found, please try again later with valid email Address!";
                    //return PartialView("ForgotPassword", _ForgotPasswordModel);
                    //return Json(_ForgotPasswordModel, JsonRequestBehavior.AllowGet);
                    return Json(new { Success = false, Message = "Email Address With no one user found, please try again later with valid email Address!", _ForgotPasswordModel }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception _exception)
            {
                throw _exception;
            }
        }
    }
}