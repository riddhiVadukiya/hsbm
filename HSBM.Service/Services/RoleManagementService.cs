using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.RoleMaster;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HSBM.Service.Services
{
    public class RoleManagementService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Active/Inactive Role
        /// </summary>
        /// <param name="roleMaster">RoleMaster</param>
        public void ActiveAndInactiveSwitchUpdateForRole(RequestResponseServiceContext requestResponseServiceContext, RoleMaster roleMaster)
        {
            RoleMasterRepository _RoleMasterRepository = new RoleMasterRepository();

            try
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                _RoleMasterRepository.ActiveAndInactiveSwitchUpdateForRole(roleMaster);
            }
            catch (Exception ex)
            {
                _ILogger.Error("RoleManagementService=>ActiveAndInactiveSwitchUpdateForRole: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Add/Update RoleMaster
        /// </summary>
        /// <param name="roleMaster">RoleMaster</param>
        /// <returns> integer </returns>
        public int AddUpdateRoleMaster(RequestResponseServiceContext requestResponseServiceContext, RoleMaster roleMaster)
        {
            RoleMasterRepository _RoleMasterRepository = new RoleMasterRepository();
            int result = 0;
            try
            {

                result = _RoleMasterRepository.AddUpdateRoleMaster(roleMaster);
                if (result == 1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
                else if (result == 2)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Role already exist." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("RoleManagementService=>AddUpdateRoleMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
            return result;
        }

        /// <summary>
        /// Delete RoleMaster ById
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns> true/false </returns>
        public bool DeleteRoleMasterById(long Id)
        {
            RoleMasterRepository _RoleMasterRepository = new RoleMasterRepository();

            bool result = _RoleMasterRepository.DeleteRoleMasterById(Id);

            return result;
        }

        /// <summary>
        /// Get All Role Master For Grid
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="p_GridParams"></param>
        /// <param name="p_SearchRequest"></param>
        /// <returns></returns>
        public GridDataResponse GetAllRoleMasterForGrid(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, RoleMasterRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();

            try
            {
                RoleMasterRepository _RoleMasterRepository = new RoleMasterRepository();

                var SystemUsersList = _RoleMasterRepository.GetAllRoleMaster(p_SearchRequest, p_GridParams);

                if (SystemUsersList.Any() && SystemUsersList.Count > 0)
                {
                    _GridDataResponse.recordsTotal = SystemUsersList.First().RecordsTotal;
                    _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    _GridDataResponse.data = SystemUsersList;

                  //  requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
                else
                {
                    _GridDataResponse.data = SystemUsersList;
                    //requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    //requestResponseServiceContext.Response.StatusParameters = new string[] { "No records found" };
                }

            }
            catch (Exception ex)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
            }

            return _GridDataResponse;
        }

        public RoleMaster GetRoleMasterById(long Id)
        {
            RoleMasterRepository _RoleMasterRepository = new RoleMasterRepository();

            RoleMaster _RoleMaster = new RoleMaster();

            _RoleMaster = _RoleMasterRepository.GetRoleMasterById(Id);

            return _RoleMaster;
        }

        public RoleMaster GetRoleMasterForAdd(bool IsAdmin)
        {
            RoleMasterRepository _RoleMasterRepository = new RoleMasterRepository();

            RoleMaster _RoleMaster = new RoleMaster();

            _RoleMaster = _RoleMasterRepository.GetRoleMasterForAdd(IsAdmin);

            return _RoleMaster;
        }

        public List<SelectListItem> RoleDropDown()
        {
            RoleMasterRepository _RoleMasterRepository = new RoleMasterRepository();

            List<SelectListItem> _RoleList = new List<SelectListItem>();

            _RoleList = _RoleMasterRepository.RoleDropDown();

            return _RoleList;
        }

        public bool UsersExistInRole(RequestResponseServiceContext requestResponseServiceContext, long RoleId)
        {
            bool result = false;
            RoleMasterRepository _RoleMasterRepository = new RoleMasterRepository();
            try
            {
                result = _RoleMasterRepository.UsersExistInRole(RoleId);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
            }
            catch (Exception _Exception)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { _Exception.Message };
            }
            return result;
        }
    }
}
