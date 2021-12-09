using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.ForgotPassword;

using HSBM.EntityModel.SystemUsers;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class SystemUserService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Service for ActiveInactive SystemUser Switch Update
        /// <summary>
        /// ActiveInactive SystemUser Switch Update
        /// </summary>
        /// <param name="p_SystemUsers">Objcet of SystemUsers</param>
        /// <returns></returns>
        public bool ActiveAndInactiveSwitchUpdate(SystemUsers p_SystemUsers)
        {
            try
            {
                SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
                return _SystemUsersRepository.ActiveAndInactiveSystemUserSwitchUpdate(p_SystemUsers);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion


        #region Add Update SystemUser
        /// <summary>
        /// Add Update SystemUser
        /// </summary>
        /// <param name="requestResponseServiceContext">requestResponseServiceContext</param>
        /// <param name="p_SystemUsers">Objcet of SystemUsers</param>
        /// <returns></returns>
        public bool AddUpdateSystemUser(RequestResponseServiceContext requestResponseServiceContext, SystemUsers p_SystemUsers)
        {
            try
            {
                SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
                _SystemUsersRepository.AddOrUpdateSystemUser(p_SystemUsers);

                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;


            }
            catch (Exception ex)
            {
                _ILogger.Error(ex.Message);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
                return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Duplication check of User name
        /// </summary>
        /// <param name="requestResponseServiceContext">RequestResponseServiceContext</param>
        /// <param name="p_SystemUsers">SystemUsers</param>
        /// <returns> true/false </returns>
        public int CheckEmailOrUserNameIsExists(RequestResponseServiceContext requestResponseServiceContext,SystemUsers p_SystemUsers)
        {
            int SystemUserId=0;
            try
            {
                SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
               SystemUserId =  _SystemUsersRepository.CheckEmailOrUserNameIsExists(p_SystemUsers);

                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;


            }
            catch (Exception ex)
            {
                _ILogger.Error(ex.Message);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
                return SystemUserId;
            }
            return SystemUserId;
        }

        #region Change Password
        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="p_SystemUsersKey">SystemUsersKey</param>
        /// <param name="p_OldPassword">OldPassword</param>
        /// <param name="p_NewPassword">NewPassword</param>
        /// <returns></returns>
        public bool ChangeSystemUserPassword(long p_SystemUsersKey, string p_NewPassword)
        {
            try
            {
                SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
                return _SystemUsersRepository.ChangePassword(p_SystemUsersKey, p_NewPassword);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion

        #region Delete SystemUser By Id
        /// <summary>
        /// Delete SystemUser By Id
        /// </summary>
        /// <param name="requestResponseServiceContext">requestResponseServiceContext</param>
        /// <param name="SystemUsersKey">SystemUsers Key</param>
        /// <returns></returns>
        public bool DeleteSystemUserByUserId(RequestResponseServiceContext requestResponseServiceContext, long SystemUsersKey)
        {
            try
            {
                SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
                _SystemUsersRepository.DeleteSystemUser(SystemUsersKey);
            }
            catch (Exception ex)
            {
                _ILogger.Error(ex.Message);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
                return false;
            }
            return true;
        }
        #endregion

        #region Get All SubUsers ForGrid
        /// <summary>
        /// Get All SubUsers ForGrid
        /// </summary>
        /// <param name="p_GridParams">Objcet of GridParams</param>
        /// <param name="p_SearchRequest">Objcet of SystemUsersRequest</param>
        /// <returns></returns>
        public GridDataResponse GetAllSubUsersBySearchRequest(GridParams p_GridParams, SystemUsersRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();

            try
            {
                var _SubUsers = _SystemUsersRepository.GetAllSubuserByParentIdForGrid(p_SearchRequest, p_GridParams);

                if (_SubUsers != null && _SubUsers.Count > 0)
                //if (_SubUsers != null)
                {
                    _GridDataResponse.recordsTotal = _SubUsers.FirstOrDefault().RecordsTotal;
                    _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    _GridDataResponse.data = _SubUsers;
                }
                else
                {
                    _GridDataResponse.data = _SubUsers;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _GridDataResponse;
        }
        #endregion

        #region Get SubUsers By Id And Parent
        /// <summary>
        /// Get SubUsers By Id And Parent
        /// </summary>
        /// <param name="p_Id"></param>
        /// <param name="p_ParentId"></param>
        /// <returns></returns>
        public SystemUsers GetSubUsersByIdAndParentId(long p_Id, long p_ParentId)
        {
            SystemUsers _SystemUsers = new SystemUsers();

            try
            {
                SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
                _SystemUsers = _SystemUsersRepository.GetSubUsersByIdAndParent(p_Id, p_ParentId);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _SystemUsers;
            //return _SystemUsersRepository.GetTable().Where(t => t.Id == id && t.ParentId == ParentId).FirstOrDefault();
        }
        #endregion

        public SystemUsers GetSystemUserByKey(RequestResponseServiceContext requestResponseServiceContext, long SystemUserKey)
        {
            SystemUsers _SystemUser = new SystemUsers();
            SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
            try
            {
                _SystemUser = _SystemUsersRepository.GetSystemUserById(SystemUserKey);

                if (_SystemUser != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "No records found" };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error(ex.Message);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
            }

            return _SystemUser;
        }

        #region Service for System User Login
        /// <summary>
        /// System User Login
        /// </summary>
        /// <param name="requestResponseServiceContext">requestResponseServiceContext</param>
        /// <param name="UserName">UserName</param>
        /// <param name="p_Password">Password</param>
        /// <returns></returns>
        public SystemUsers SystemUserLogin(RequestResponseServiceContext requestResponseServiceContext, LoginUser p_SystemUsers)
        {

            SystemUsers systemUser = new SystemUsers();
            SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
            try
            {
                systemUser = _SystemUsersRepository.GetSystemUserByLoginCredentials(p_SystemUsers);

                if (systemUser != null)
                {
                    if (systemUser.Password == p_SystemUsers.Password)
                    {
                        RoleMasterRepository _RoleMasterRepository = new RoleMasterRepository();
                        systemUser.RoleMasterDetails = _RoleMasterRepository.Execute(systemUser.RoleMasterID);
                        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    }
                    else
                    {
                        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.CURRENT_PASSWORD_INCORRECT;
                        requestResponseServiceContext.Response.StatusParameters = new string[] { "Password not matched." };
                    }
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "No user found." };
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error(ex.Message);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
            }

            return systemUser;
        }
        #endregion

        public ForgotPasswordModel forgotPassword(RequestResponseServiceContext WebRequestResponseServiceContext, ForgotPasswordModel p_ForgotPasswordModel)
        {
            ForgotPasswordModel _ForgotPasswordModel = new ForgotPasswordModel();

            try
            {
                SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();

                _ForgotPasswordModel = _SystemUsersRepository.GetSystemUserByEmail(p_ForgotPasswordModel);

                if (_ForgotPasswordModel != null)
                {
                    WebRequestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
            }
            catch (Exception exception)
            {
                WebRequestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                WebRequestResponseServiceContext.Response.StatusParameters = new string[] { "Unexpected Error" };
            }

            return _ForgotPasswordModel;
        }

        /// <summary>
        /// Reset Password ById
        /// </summary>
        /// <param name="WebRequestResponseServiceContext">RequestResponseServiceContext</param>
        /// <param name="_ResetPasswordModel">ResetPasswordModel</param>
        /// <returns> true/false </returns>
        public bool ResetPasswordById(RequestResponseServiceContext WebRequestResponseServiceContext, ResetPasswordModel _ResetPasswordModel)
        {
            try
            {
                SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();

                bool result = _SystemUsersRepository.ResetPasswordById(_ResetPasswordModel);

                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                WebRequestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                WebRequestResponseServiceContext.Response.StatusParameters = new string[] { "Unexpected Error" };
            }
            return false;
        }

        
        public int GetUserType(RequestResponseServiceContext requestResponseServiceContext, SystemUsers p_SystemUser)
        {
            int _Usertype = 0;
            SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
            try
            {
                _Usertype = _SystemUsersRepository.GetUserType(p_SystemUser);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
            }
            catch (Exception _Exception)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error" };
                return 0;
            }
            return _Usertype;
        }

        public bool DuplicateUsernameOrEmail(RequestResponseServiceContext requestResponseServiceContext, SystemUsers p_SystemUsers)
        {
            try
            {

                SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
               int Id= _SystemUsersRepository.CheckEmailOrUserNameIsExists(p_SystemUsers);
               bool result = Id > 0 ? true : false;
                if(result)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }                
            }
            catch (Exception ex)
            {
                _ILogger.Error(ex.Message);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
                return false;
            }
            return true;
        }

        public bool SetVerifyUser(long p_SystemUsersKey)
        {
            try
            {
                SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
                return _SystemUsersRepository.SetVerifyUser(p_SystemUsersKey);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
