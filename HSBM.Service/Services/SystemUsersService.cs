using NBA.Common.Utils;
using NBA.EntityModel.Common;
using NBA.EntityModel.RoleMaster;
using NBA.EntityModel.SystemUsers;
using NBA.Repository.Contracts;
using NBA.Repository.Repositories;
using NBA.Service.Contracts;
using NBA.Service.ServiceContext;
using NBA.Service.Services.RoleManagementServices;
using NBA.Service.Services.SystemUserServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NBA.Service.Services
{
    public class SystemUsersService : ISystemUsersService
    {

        #region Init
        public static readonly NBA.Common.Logging.ILogger _ILogger = NBA.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       // public readonly ISystemUsersRepository _SystemUsersRepository;

        public SystemUsersService()
        {
            //_SystemUsersRepository = SystemUsersRepository;
        }

        #endregion


        #region Get

        //public GridDataResponse GetAllSystemUsersForGrid(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, SystemUsersRequest p_SearchRequest)
        //{
        //    GridDataResponse _GridDataResponse = new GridDataResponse();

        //    try
        //    {
        //        var SystemUsersList = _SystemUsersRepository.GetAllSystemUsers(p_SearchRequest, p_GridParams);

        //        if (SystemUsersList.Any())
        //        {
        //            // _GridDataResponse.recordsTotal = SystemUsersList.First().RecordsTotal;
        //            _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
        //            _GridDataResponse.data = SystemUsersList;

        //            requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
        //        }
        //        else
        //        {
        //            requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
        //            requestResponseServiceContext.Response.StatusParameters = new string[] { "No records found" };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _ILogger.Error(ex.Message);
        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
        //        requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
        //    }

        //    return _GridDataResponse;
        //}

        //public List<SystemUsers> GetAllSystemUsers(RequestResponseServiceContext requestResponseServiceContext)
        //{
        //    List<SystemUsers> SystemUsersList = new List<SystemUsers>();

        //    try
        //    {
        //        SystemUsersList = _SystemUsersRepository.GetTable().ToList();

        //        if (SystemUsersList.Any())
        //        {
        //            requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
        //        }
        //        else
        //        {
        //            requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
        //            requestResponseServiceContext.Response.StatusParameters = new string[] { "No records found" };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _ILogger.Error(ex.Message);
        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
        //        requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
        //    }

        //    return SystemUsersList;
        //}

        //public List<SystemUsers> GetAllSystemUsers(RequestResponseServiceContext requestResponseServiceContext, SystemUsersRequest p_SearchRequest)
        //{
        //    List<SystemUsers> SystemUsersList = new List<SystemUsers>();

        //    try
        //    {
        //        SystemUsersList = _SystemUsersRepository.GetAllSystemUsers(p_SearchRequest);

        //        if (SystemUsersList.Any())
        //        {
        //            requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
        //        }
        //        else
        //        {
        //            requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
        //            requestResponseServiceContext.Response.StatusParameters = new string[] { "No records found" };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _ILogger.Error(ex.Message);
        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
        //        requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
        //    }

        //    return SystemUsersList;
        //}

        //public SystemUsers GetSystemUserById(RequestResponseServiceContext requestResponseServiceContext, long SystemUserKey)
        //{
        //    SystemUsers SystemUser = new SystemUsers();

        //    try
        //    {
        //        SystemUser = _SystemUsersRepository.GetSystemUserById(SystemUserKey);

        //        if (SystemUser != null)
        //        {
        //            requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
        //        }
        //        else
        //        {
        //            requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
        //            requestResponseServiceContext.Response.StatusParameters = new string[] { "No records found" };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _ILogger.Error(ex.Message);
        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
        //        requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
        //    }

        //    return SystemUser;
        //}

        #endregion


        #region Add/Update

        //public bool AddOrUpdateSystemUser(RequestResponseServiceContext requestResponseServiceContext, SystemUsers p_SystemUsers)
        //{
        //    try
        //    {
        //        _SystemUsersRepository.AddOrUpdateSystemUser(p_SystemUsers);

        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;


        //    }
        //    catch (Exception ex)
        //    {
        //        _ILogger.Error(ex.Message);
        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
        //        requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
        //        return false;
        //    }
        //    return true;
        //}


        //public bool ChangePassword(RequestResponseServiceContext requestResponseServiceContext, long SystemUserKey, string OldPassword, string NewPassword)
        //{
        //    try
        //    {
        //        ChangePassword _ChangePassword = new ChangePassword();
        //        _ChangePassword.ChangeSystemUserPassword(SystemUserKey, OldPassword, NewPassword);
        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
        //    }
        //    catch (Exception ex)
        //    {
        //        _ILogger.Error(ex.Message);
        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
        //        requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
        //        return false;
        //    }
        //    return true;
        //}
        #endregion


        #region Delete

        //public bool DeleteSystemUser(RequestResponseServiceContext requestResponseServiceContext, long SystemUsersKey)
        //{
        //    try
        //    {
        //        _SystemUsersRepository.DeleteSystemUser(SystemUsersKey);
        //    }
        //    catch (Exception ex)
        //    {
        //        _ILogger.Error(ex.Message);
        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
        //        requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
        //        return false;
        //    }
        //    return true;
        //}

        #endregion


        #region Login
        //public SystemUsers Login(RequestResponseServiceContext requestResponseServiceContext, string UserName, string Password)
        //{

        //    //GridParams gridParams = new GridParams();
        //    //gridParams.skip = 1;
        //    //gridParams.take = 2;
        //    //gridParams.sort = new List<GridSort>();
        //    //gridParams.sort.Add(new GridSort() { Dir = "DESC", Field = "RoleName" });

        //    //var xxx = _RoleMasterRepository.GetTable().Where(x => x.IsActive);
        //    //xxx = _RoleMasterRepository.ApplyPagingSorting(gridParams, xxx);
        //    //var z = xxx.ToList();

        //    SystemUsers systemUser = new SystemUsers();

        //    try
        //    {
        //        systemUser = _SystemUsersRepository.GetSystemUserByLoginCredentials(UserName, Password);

        //        if (systemUser != null)
        //        {
        //            RoleMasterRepository _RoleMasterRepository = new RoleMasterRepository();
        //            systemUser.RoleMasterDetails = _RoleMasterRepository.Execute(systemUser.RoleMasterID);
        //            requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
        //        }
        //        else
        //        {
        //            requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
        //            requestResponseServiceContext.Response.StatusParameters = new string[] { "No records found" };
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _ILogger.Error(ex.Message);
        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
        //        requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error: " + ex.Message, "Details: " + ex.StackTrace };
        //    }

        //    return systemUser;
        //}
        #endregion


        //public GridDataResponse GetAllSubUsersForGrid(GridParams p_GridParams, SystemUsersRequest p_SearchRequest)
        //{
        //    GridDataResponse _GridDataResponse = new GridDataResponse();

        //    var _SubUsers = _SystemUsersRepository.GetAllSubuserByParentIdForGrid(p_SearchRequest, p_GridParams);

        //    if (_SubUsers != null && _SubUsers.Count > 0)
        //    {
        //        _GridDataResponse.recordsTotal = _SubUsers.FirstOrDefault().RecordsTotal;
        //        _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
        //        _GridDataResponse.data = _SubUsers;
        //    }

        //    return _GridDataResponse;
        //}

        //public List<SystemUsers> GetAllSubUsersForGrid(long ParentId)
        //{

        //    var _SystemUsers = _SystemUsersRepository.GetTable().Where(t => t.ParentId == ParentId).Join(_RoleMasterRepository.GetTable(),
        //       S => S.RoleMasterID,
        //       R => R.Id,
        //           (User, Role) => new SystemUsers()
        //           {
        //               Id = User.Id,
        //               ParentId = User.ParentId,
        //               UserType = User.UserType,
        //               RoleMasterID = User.RoleMasterID,
        //               UserName = User.UserName,
        //               Password = User.Password,
        //               FirstName = User.FirstName,
        //               LastName = User.LastName,
        //               MiddleName = User.MiddleName,
        //               Email = User.Email,
        //               Telephone = User.Telephone,
        //               Mobile = User.Mobile,
        //               Gender = User.Gender,
        //               Address = User.Address,
        //               Address2 = User.Address2,
        //               CountryMasterID = User.CountryMasterID,
        //               RegionMasterID = User.RegionMasterID,
        //               CityMasterID = User.CityMasterID,
        //               LastLoginDate = User.LastLoginDate,
        //               PhotoAlbumID = User.PhotoAlbumID,
        //               CreatedBy = User.CreatedBy,
        //               CreatedDate = User.CreatedDate,
        //               UpdatedBy = User.UpdatedBy,
        //               UpdatedDate = User.UpdatedDate,
        //               IsActive = User.IsActive,
        //               RoleName = Role.RoleName
        //           }).ToList();

        //    return _SystemUsers.ToList();
        //}

        //public SystemUsers GetSubUsersByIdAndParent(long id, long ParentId)
        //{
        //    return _SystemUsersRepository.GetTable().Where(t => t.Id == id && t.ParentId == ParentId).FirstOrDefault();
        //}

        //public long AddUpdateSubUser(SystemUsers systemUsers)
        //{
        //    if (systemUsers.Id > 0)
        //    {
        //        return _SystemUsersRepository.Update(systemUsers);
        //    }
        //    else
        //    {
        //        return _SystemUsersRepository.Insert(systemUsers);
        //    }
        //}


        //public long ActiveAndInactiveSwitchUpdate(SystemUsers systemUsers)
        //{
        //    return _SystemUsersRepository.UpdateField(systemUsers.Id, t => t.IsActive, systemUsers.IsActive);
        //}

        //public void DeleteSubUser(long Id)
        //{
        //    _SystemUsersRepository.DeleteById(Id);
        //}

    }
}
