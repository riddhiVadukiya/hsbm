using HSBM.EntityModel.SystemUsers;
using HSBM.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSBM.Common.Utils;
using BLToolkit.Data;
using HSBM.EntityModel.Common;
using System.Web.Mvc;
using HSBM.Common.Enums;
using HSBM.EntityModel.ForgotPassword;
using DocumentFormat.OpenXml.Office2010.ExcelAc;


namespace HSBM.Repository.Repositories
{
    public class SystemUsersRepository : Repository<SystemUsers>
    {
        public List<SystemUsers> GetAllSystemUsers(SystemUsersRequest filter)
        {
            return GetSystemUsers(filter);
        }

        public List<SystemUsers> GetAllSystemUsers(SystemUsersRequest filter, GridParams p_GridParams)
        {
            return GetSystemUsers(filter, p_GridParams);
        }

        public SystemUsers GetSystemUserById(long p_SystemUsersKey)
        {
            SystemUsers _SystemUsers = new SystemUsers();
            try
            {
                using (DbManager _DBManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT [Id],[ParentId],[UserType],[RoleMasterId],[UserName],[Password],[FirstName],[LastName],[MiddleName],[Email],
                                                [Telephone],[Mobile],[Gender],[Address],[Address2],[CountryMasterID],[RegionMasterID],[CityMasterID],[LastLoginDate],
                                                [PhotoAlbumID],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate],[IsActive], [CompanyName], [BaseCurrency], [ImageUrl] FROM [dbo].[SystemUsers] 
                                                Where Id = @SystemUsersKey";

                    _SystemUsers = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@SystemUsersKey", p_SystemUsersKey)).ExecuteObject<SystemUsers>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _SystemUsers;

            //return GetSystemUsers(new SystemUsersRequest() { Id = SystemUserKey }).FirstOrDefault();
        }

        public SystemUsers GetSystemUserByLoginCredentials(LoginUser p_SystemUsers)
        {
            SystemUsers _SystemUsers = new SystemUsers();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    //                    String _SqlQuery = @"
                    //SELECT 
                    //    [Id],[ParentId],[UserType],[RoleMasterId],[UserName],[Password],[FirstName],[LastName],[MiddleName],[Email],[Telephone],[Mobile],[Gender],[Address],
                    //    [Address2],[CountryMasterID],[RegionMasterID],[CityMasterID],[LastLoginDate],[PhotoAlbumID],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate],[IsActive] 
                    //FROM [dbo].[SystemUsers] 
                    //Where (UserName = @UserName or Email = @UserName) and IsActive = 1 and IsVerify = 1
                    //and Password = @Password 
                    //and UserType = @UserType";
                    String _SqlQuery = @"
                                    SELECT 
                                        [Id],[ParentId],[UserType],[RoleMasterId],[UserName],[Password],[FirstName],[LastName],[MiddleName],[Email],[Telephone],[Mobile],[Gender],[Address],[IsSocialMedia],
                                        [Address2],[CountryMasterID],[RegionMasterID],[CityMasterID],[LastLoginDate],[PhotoAlbumID],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate],[IsActive] 
                                    FROM [dbo].[SystemUsers] 
                                    Where (Email = @UserName) and IsActive = 1 and IsVerify = 1
                                    and Password = @Password 
                                    and UserType = @UserType";

                    _SystemUsers = _DbManager.SetCommand(_SqlQuery,
                                                            _DbManager.Parameter("@Password", p_SystemUsers.Password),
                        _DbManager.Parameter("@UserName", p_SystemUsers.UserName),
                        _DbManager.Parameter("@UserType", p_SystemUsers.UserType)).ExecuteObject<SystemUsers>();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _SystemUsers;
            // return GetSystemUsers(new SystemUsersRequest() { UserName = UserName, Password = Password }).FirstOrDefault();
        }

        private List<SystemUsers> GetSystemUsers(SystemUsersRequest filter, GridParams p_GridParams = null)
        {
            List<SystemUsers> objSystemUsers = new List<SystemUsers>();
            try
            {
                var _SystemUsers = GetTable().Select(t => t);

                if (filter.Id > 0)
                {
                    _SystemUsers = _SystemUsers.Where(t => t.Id == filter.Id);
                }

                if (filter.UserType > 0)
                {
                    _SystemUsers = _SystemUsers.Where(t => t.UserType == filter.UserType);
                }

                if (!string.IsNullOrEmpty(filter.Password))
                {
                    //_SystemUsers = _SystemUsers.Where(t => t.Password == filter.Password && (t.UserName == filter.UserName || t.Email == filter.Email));
                    _SystemUsers = _SystemUsers.Where(t => t.Password == filter.Password && (t.Email == filter.Email));
                }
                else
                {
                    //if (!string.IsNullOrEmpty(filter.UserName))
                    //{
                    //    _SystemUsers = _SystemUsers.Where(t => t.UserName.Contains(filter.UserName));
                    //}
                    if (!string.IsNullOrEmpty(filter.Email))
                    {
                        _SystemUsers = _SystemUsers.Where(t => t.Email.Contains(filter.Email));
                    }
                }

                if (!string.IsNullOrEmpty(filter.FirstName))
                {
                    _SystemUsers = _SystemUsers.Where(t => t.FirstName.Contains(filter.FirstName));
                }

                if (!string.IsNullOrEmpty(filter.LastName))
                {
                    _SystemUsers = _SystemUsers.Where(t => t.LastName.Contains(filter.LastName));
                }

                if (!string.IsNullOrEmpty(filter.Telephone))
                {
                    _SystemUsers = _SystemUsers.Where(t => t.Telephone.Contains(filter.Telephone));
                }
                if (!string.IsNullOrEmpty(filter.Mobile))
                {
                    _SystemUsers = _SystemUsers.Where(t => t.Mobile.Contains(filter.Mobile));
                }
                if (!string.IsNullOrEmpty(filter.Gender))
                {
                    _SystemUsers = _SystemUsers.Where(t => t.Gender == filter.Gender);
                }

                if (filter.CountryMasterID != null)
                {
                    _SystemUsers = _SystemUsers.Where(t => t.CountryMasterID == filter.CountryMasterID);
                }

                if (filter.StateMasterID != null)
                {
                    _SystemUsers = _SystemUsers.Where(t => t.RegionMasterID == filter.StateMasterID);
                }

                if (filter.CityMasterID != null)
                {
                    _SystemUsers = _SystemUsers.Where(t => t.CityMasterID == filter.CityMasterID);
                }

                if (!filter.IncludeIsDeleted)
                {
                    _SystemUsers = _SystemUsers.Where(t => t.IsActive);
                }

                if (p_GridParams != null)
                {
                    _SystemUsers = ApplyPagingSorting(p_GridParams, _SystemUsers);
                }

                objSystemUsers = _SystemUsers.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objSystemUsers;
        }

        public bool AddOrUpdateSystemUser(SystemUsers p_SystemUser)
        {
            try
            {
                //p_SystemUser.Id = new Guid("A300324D-4AD6-4A3D-B61F-F99706E2757D");
                //p_SystemUser.FirstName = "Big";
                //Delete(p_SystemUser);

                using (DbManager _DBManager = new DbManager())
                {
                    String _SqlQuery = string.Empty;

                    if (p_SystemUser != null)
                    {
                        if (p_SystemUser.Id > 0)
                        {

                            SystemUsers _SystemUsers = GetSystemUserById(p_SystemUser.Id);
                            if (_SystemUsers != null)
                            {
                                p_SystemUser.CreatedDate = _SystemUsers.CreatedDate;
                                p_SystemUser.CreatedBy = _SystemUsers.CreatedBy;
                                p_SystemUser.UpdatedDate = DateTime.Now;
                                p_SystemUser.UpdatedBy = _SystemUsers.Id;
                                p_SystemUser.UserType = _SystemUsers.UserType;
                                //p_SystemUser.RoleMasterID = _SystemUsers.RoleMasterID;
                                //p_SystemUser.RoleMasterID = _SystemUsers.RoleMasterID;
                                //p_SystemUser.IsActive = _SystemUsers.IsActive;

                            }

                            _SqlQuery = @"UPDATE [dbo].[SystemUsers] SET [ParentId] = @ParentId,
                                                [UserType] = @UserType,
                                                [RoleMasterId] = @RoleMasterId,                                                                                             
                                                [FirstName] = @FirstName,
                                                [LastName] = @LastName,
                                                [MiddleName] = @MiddleName,
                                                [Email] = @Email,
                                                [Telephone] = @Telephone,
                                                [Mobile] = @Mobile,
                                                [Gender] = @Gender,
                                                [Address] = @Address,
                                                [Address2] = @Address2,
                                                [CountryMasterID] = @CountryMasterID,
                                                [RegionMasterID] = @RegionMasterID,
                                                [CityMasterID] = @CityMasterID,
                                                [LastLoginDate] = @LastLoginDate,
                                                [PhotoAlbumID] = @PhotoAlbumID,                                                
                                                [UpdatedBy] = @UpdatedBy,
                                                [UpdatedDate] = @UpdatedDate,
                                                [IsActive] = @IsActive,
                                                [CompanyName] = @CompanyName,
                                                [BaseCurrency] = @BaseCurrency,
                                                [ImageUrl] = @ImageUrl
                                                WHERE Id = @Id";

                            int result = _DBManager.SetCommand(_SqlQuery,
                          _DBManager.Parameter("@ParentId", p_SystemUser.ParentId),
                          _DBManager.Parameter("@UserType", p_SystemUser.UserType),
                          _DBManager.Parameter("@RoleMasterId", p_SystemUser.RoleMasterID),
                                //_DBManager.Parameter("@UserName", p_SystemUser.UserName),
                          _DBManager.Parameter("@Password", p_SystemUser.Password),
                          _DBManager.Parameter("@FirstName", p_SystemUser.FirstName),
                          _DBManager.Parameter("@LastName", p_SystemUser.LastName),
                          _DBManager.Parameter("@MiddleName", p_SystemUser.MiddleName),
                          _DBManager.Parameter("@Email", p_SystemUser.Email),
                          _DBManager.Parameter("@Telephone", p_SystemUser.Telephone),
                          _DBManager.Parameter("@Mobile", p_SystemUser.Mobile),
                          _DBManager.Parameter("@Gender", p_SystemUser.Gender),
                          _DBManager.Parameter("@Address", p_SystemUser.Address),
                          _DBManager.Parameter("@Address2", p_SystemUser.Address2),
                          _DBManager.Parameter("@CountryMasterID", p_SystemUser.CountryMasterID),
                          _DBManager.Parameter("@RegionMasterID", p_SystemUser.RegionMasterID),
                          _DBManager.Parameter("@CityMasterID", p_SystemUser.CityMasterID),
                          _DBManager.Parameter("@LastLoginDate", p_SystemUser.LastLoginDate),
                          _DBManager.Parameter("@PhotoAlbumID", p_SystemUser.PhotoAlbumID),
                          _DBManager.Parameter("@CreatedBy", p_SystemUser.CreatedBy),
                          _DBManager.Parameter("@CreatedDate", p_SystemUser.CreatedDate),
                          _DBManager.Parameter("@UpdatedBy", p_SystemUser.UpdatedBy),
                          _DBManager.Parameter("@UpdatedDate", p_SystemUser.UpdatedDate),
                          _DBManager.Parameter("@IsActive", p_SystemUser.IsActive),
                          _DBManager.Parameter("@Id", p_SystemUser.Id),
                          _DBManager.Parameter("@CompanyName", p_SystemUser.CompanyName),
                          _DBManager.Parameter("@BaseCurrency", p_SystemUser.BaseCurrency),
                          _DBManager.Parameter("@IsVerify", p_SystemUser.IsVerify),
                          _DBManager.Parameter("@ImageUrl", p_SystemUser.ImageUrl)
                          ).ExecuteNonQuery();

                           return result > 0? true:false;
                            
                        }
                        else
                        {
                            _SqlQuery = @" INSERT INTO [dbo].[SystemUsers]
                                                               ([ParentId]
                                                               ,[UserType]
                                                               ,[RoleMasterId]                                                               
                                                               ,[Password]
                                                               ,[FirstName]
                                                               ,[LastName]
                                                               ,[MiddleName]
                                                               ,[Email]
                                                               ,[Telephone]
                                                               ,[Mobile]
                                                               ,[Gender]
                                                               ,[Address]
                                                               ,[Address2]
                                                               ,[CountryMasterID]
                                                               ,[RegionMasterID]
                                                               ,[CityMasterID]                                                               
                                                               ,[PhotoAlbumID]
                                                               ,[CreatedBy]
                                                               ,[CreatedDate]                                                               
                                                               ,[IsActive]
                                                               ,[IsVerify]
                                                               ,[CompanyName]
                                                               ,[BaseCurrency]
                                                               ,[IsSocialMedia]
                                                            )
                                                         OUTPUT Inserted.Id 
                                                         VALUES
                                                               (@ParentId,
                                                               @UserType,
                                                               @RoleMasterId,                                                               
                                                               @Password,
                                                               @FirstName,
                                                               @LastName,
                                                               @MiddleName,
                                                               @Email,
                                                               @Telephone,
                                                               @Mobile,
                                                               @Gender,
                                                               @Address,
                                                               @Address2,
                                                               @CountryMasterID,
                                                               @RegionMasterID,
                                                               @CityMasterID,
                                                               @PhotoAlbumID,
                                                               @CreatedBy,
                                                               @CreatedDate,
                                                               @IsActive,
                                                               @IsVerify,
                                                               @CompanyName,
                                                               @BaseCurrency,
                                                               @IsSocialMedia)";

                            int _Row = _DBManager.SetCommand(_SqlQuery,
                                _DBManager.Parameter("@ParentId", p_SystemUser.ParentId),
                                _DBManager.Parameter("@UserType", p_SystemUser.UserType),
                                _DBManager.Parameter("@RoleMasterId", p_SystemUser.RoleMasterID),
                                //_DBManager.Parameter("@UserName", p_SystemUser.UserName),
                                _DBManager.Parameter("@Password", p_SystemUser.Password),
                                _DBManager.Parameter("@FirstName", p_SystemUser.FirstName),
                                _DBManager.Parameter("@LastName", p_SystemUser.LastName),
                                _DBManager.Parameter("@MiddleName", p_SystemUser.MiddleName),
                                _DBManager.Parameter("@Email", p_SystemUser.Email),
                                _DBManager.Parameter("@Telephone", p_SystemUser.Telephone),
                                _DBManager.Parameter("@Mobile", p_SystemUser.Mobile),
                                _DBManager.Parameter("@Gender", p_SystemUser.Gender),
                                _DBManager.Parameter("@Address", p_SystemUser.Address),
                                _DBManager.Parameter("@Address2", p_SystemUser.Address2),
                                _DBManager.Parameter("@CountryMasterID", p_SystemUser.CountryMasterID),
                                _DBManager.Parameter("@RegionMasterID", p_SystemUser.RegionMasterID),
                                _DBManager.Parameter("@CityMasterID", p_SystemUser.CityMasterID),
                                _DBManager.Parameter("@LastLoginDate", p_SystemUser.LastLoginDate),
                                _DBManager.Parameter("@PhotoAlbumID", p_SystemUser.PhotoAlbumID),
                                _DBManager.Parameter("@CreatedBy", p_SystemUser.CreatedBy),
                                _DBManager.Parameter("@CreatedDate", p_SystemUser.CreatedDate),
                                _DBManager.Parameter("@UpdatedBy", p_SystemUser.UpdatedBy),
                                _DBManager.Parameter("@UpdatedDate", p_SystemUser.UpdatedDate),
                                _DBManager.Parameter("@IsActive", p_SystemUser.IsActive),
                                _DBManager.Parameter("@Id", p_SystemUser.Id),
                                _DBManager.Parameter("@CompanyName", p_SystemUser.CompanyName),
                                _DBManager.Parameter("@BaseCurrency", p_SystemUser.BaseCurrency),
                                _DBManager.Parameter("@IsVerify", p_SystemUser.IsVerify),
                                _DBManager.Parameter("@ImageUrl", p_SystemUser.ImageUrl),
                                _DBManager.Parameter("@IsSocialMedia", p_SystemUser.IsSocialMedia)
                                ).ExecuteScalar<int>();

                            if (_Row > 0)
                            {
                                string _Customer = "Update OrderMaster set CustomerId=@Id where GuestEmail=@GuestEmail";
                               int result= _DBManager.SetCommand(_Customer, _DBManager.Parameter("@Id", _Row), _DBManager.Parameter("@GuestEmail", p_SystemUser.Email)).ExecuteNonQuery();
                            }
                            return _Row > 0 ? true : false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }


        public bool ChangePassword(long p_SystemUsersKey, string NewPassword)
        {
            try
            {
                String _SqlQuery = string.Empty;
                int _Result = 0;
                using (DbManager _DBManager = new DbManager())
                {
                    //                    _SqlQuery = @"Select [Id],[ParentId],[UserType],[RoleMasterId],[UserName],[Password],[FirstName],[LastName],[MiddleName],[Email],
                    //                                                [Telephone],[Mobile],[Gender],[Address],[Address2],[CountryMasterID],[RegionMasterID],[CityMasterID],[LastLoginDate],
                    //                                                [PhotoAlbumID],[UpdatedBy],[UpdatedDate],[IsActive] FROM [dbo].[SystemUsers] Where Id= @SystemUsersKey";

                    //                    SystemUsers _SystemUsers = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@SystemUsersKey", p_SystemUsersKey)).ExecuteObject<SystemUsers>();

                    // if (_SystemUsers != null && _SystemUsers.Password == OldPassword)
                    // {
                    _SqlQuery = "Update SystemUsers set Password = @Password,IsSocialMedia=0 Where Id = @SystemUsersKey";

                    _Result = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@SystemUsersKey", p_SystemUsersKey),
                        _DBManager.Parameter("@Password", NewPassword)).ExecuteNonQuery();

                    //  }

                    if (_Result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteSystemUser(long p_SystemUsersKey)
        {
            try
            {
                using (DbManager _DBManager = new DbManager())
                {
                    String _SqlQuery = "Delete from SystemUsers Where Id = @SystemUsersKey";

                    int _Result = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@SystemUsersKey", p_SystemUsersKey)).ExecuteNonQuery();

                    if (_Result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CheckEmailOrUserNameIsExists(SystemUsers p_SystemUsers)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    //String _SqlQuery = @"Select Id from systemUsers where (Email= @email or UserName= @userName) and Id!=@Id";
                    String _SqlQuery = @"Select Id from systemUsers where Email= @email and Id!=@Id";

                    return _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@email", p_SystemUsers.Email),
                        //_DbManager.Parameter("@userName", p_SystemUsers.UserName),
                          _DbManager.Parameter("@Id", p_SystemUsers.Id)).ExecuteScalarList<int>().FirstOrDefault();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<SystemUsersResponse> GetAllSubuserByParentIdForGrid(SystemUsersRequest p_SearchRequest, GridParams p_GridParams)
        {

            List<SystemUsersResponse> objList = new List<SystemUsersResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"select SystemUsers.*,RM.RoleName as RoleName,RecordsTotal = COUNT(*) OVER() from SystemUsers 
                                        inner join RoleMaster RM on SystemUsers.RoleMasterId = RM.id";

                    bool IsWhereClauseEmpty = true;
                    if (!string.IsNullOrEmpty(p_SearchRequest.UserName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += "( SystemUsers.FirstName like '%" + p_SearchRequest.UserName + "%' or SystemUsers.LastName like '%" + p_SearchRequest.UserName + "%' )";
                    }
                    if (p_SearchRequest.ParentId > 0)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " SystemUsers.ParentId = " + p_SearchRequest.ParentId + " ";
                    }

                    if (!p_SearchRequest.IncludeIsDeleted)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " SystemUsers.IsActive = 1";
                    }

                    if (p_SearchRequest.UserType > 0)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " SystemUsers.UserType = " + p_SearchRequest.UserType + " ";
                    }

                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<SystemUsersResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }


        #region Active Inactive SysteUser ById
        /// <summary>
        /// Active Inactive SysteUser ById
        /// </summary>
        /// <param name="p_SystemUsers">Objcet of SystemUsers</param>
        /// <returns></returns>
        public bool ActiveAndInactiveSystemUserSwitchUpdate(SystemUsers p_SystemUsers)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = "Update SystemUsers set IsActive = @IsActive Where Id = @Id";

                    int _Result = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@IsActive", p_SystemUsers.IsActive),
                         _DbManager.Parameter("@Id", p_SystemUsers.Id)).ExecuteNonQuery();

                    if (_Result > 0)
                    {
                        return true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return false;
        }
        #endregion

        //        #region Get
        //        /// <summary>
        //        /// Get all users list
        //        /// </summary>
        //        /// <returns>Return collection</returns>
        //        public List<SystemUsersResponse> GetAllSystemUsers()
        //        {
        //            return GetSystemUsers(new SystemUsersRequest());
        //        }

        //        /// <summary>
        //        /// Get all users by filter
        //        /// </summary>
        //        /// <param name="filter"></param>
        //        /// <returns>Return collection</returns>
        //        public List<SystemUsersResponse> GetAllSystemUsers(SystemUsersRequest filter)
        //        {
        //            return GetSystemUsers(filter);
        //        }

        //        /// <summary>
        //        /// Get all user by filter and grid params 
        //        /// </summary>
        //        /// <param name="filter"></param>
        //        /// <param name="p_GridParams"></param>
        //        /// <returns>Return collection</returns>
        //        public List<SystemUsersResponse> GetAllSystemUsers(SystemUsersRequest filter, GridParams p_GridParams)
        //        {
        //            return GetSystemUsers(filter, p_GridParams);
        //        }

        //        /// <summary>
        //        /// Get singl user
        //        /// </summary>
        //        /// <param name="SystemUserKey"></param>
        //        /// <returns>Return object</returns>
        //        public SystemUsersResponse GetSystemUserById(Guid SystemUserKey)
        //        {
        //            return GetSystemUsers(new SystemUsersRequest() { SystemUsersKey = SystemUserKey }).FirstOrDefault();
        //        }

        //        /// <summary>
        //        /// Login 
        //        /// </summary>
        //        /// <param name="UserName"></param>
        //        /// <param name="Password"></param>
        //        /// <returns>System user object</returns>
        //        public SystemUsersResponse GetSystemUserByLoginCredentials(string UserName, string Password)
        //        {
        //            return GetSystemUsers(new SystemUsersRequest() { UserName = UserName, Password = Password }).FirstOrDefault();
        //        }

        //        private List<SystemUsersResponse> GetSystemUsers(SystemUsersRequest filter, GridParams p_GridParams = null)
        //        {
        //            List<SystemUsersResponse> objSystemUsers = new List<SystemUsersResponse>();
        //            try
        //            {

        //                using (_DbManager = new DbManager())
        //                {
        //                    String _SqlQuery = @"
        //                       SELECT [SystemUsersKey],[RoleMasterID],[UserName],[Password],[FirstName],[LastName],[MiddleName],[Email],
        //                       [Telephone],[Mobile],[Gender],[Address],[Address2],[CountryMasterID],[StateMasterID],[CityMasterID],[LastLoginDate],
        //                       [PhotoAlbumID],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate],[IsActive],RecordsTotal = COUNT(*) OVER()
        //                       FROM [dbo].[SystemUsers] ";


        //                    bool IsWhereClauseEmpty = true;
        //                    if (filter.SystemUsersKey != null && filter.SystemUsersKey != Guid.Empty)
        //                    {
        //                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                        IsWhereClauseEmpty = false;
        //                        _SqlQuery += " SystemUsersKey ='" + filter.SystemUsersKey + "' ";
        //                    }

        //                    if (!string.IsNullOrEmpty(filter.Password))
        //                    {
        //                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                        IsWhereClauseEmpty = false;
        //                        _SqlQuery += " Password='" + filter.Password + "' AND (UserName='" + filter.UserName + "' OR Email='" + filter.Email + "') ";
        //                    }
        //                    else
        //                    {
        //                        if (!string.IsNullOrEmpty(filter.UserName))
        //                        {
        //                            if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                            IsWhereClauseEmpty = false;
        //                            _SqlQuery += " UserName like '%" + filter.UserName + "%' ";
        //                        }

        //                        if (!string.IsNullOrEmpty(filter.Email))
        //                        {
        //                            if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                            IsWhereClauseEmpty = false;
        //                            _SqlQuery += " Email like '%" + filter.Email + "%' ";
        //                        }
        //                    }

        //                    if (!string.IsNullOrEmpty(filter.FirstName))
        //                    {
        //                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                        IsWhereClauseEmpty = false;
        //                        _SqlQuery += " FirstName like '%" + filter.FirstName + "%' ";
        //                    }

        //                    if (!string.IsNullOrEmpty(filter.LastName))
        //                    {
        //                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                        IsWhereClauseEmpty = false;
        //                        _SqlQuery += " LastName like '%" + filter.LastName + "%' ";
        //                    }

        //                    if (!string.IsNullOrEmpty(filter.Telephone))
        //                    {
        //                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                        IsWhereClauseEmpty = false;
        //                        _SqlQuery += " Telephone like '%" + filter.Telephone + "%' ";
        //                    }

        //                    if (!string.IsNullOrEmpty(filter.Mobile))
        //                    {
        //                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                        IsWhereClauseEmpty = false;
        //                        _SqlQuery += " Mobile like '%" + filter.Mobile + "%' ";
        //                    }

        //                    if (!string.IsNullOrEmpty(filter.Gender))
        //                    {
        //                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                        IsWhereClauseEmpty = false;
        //                        _SqlQuery += " Gender ='" + filter.Gender + "' ";
        //                    }

        //                    if (filter.CountryMasterID != null && filter.CountryMasterID != Guid.Empty)
        //                    {
        //                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                        IsWhereClauseEmpty = false;
        //                        _SqlQuery += " CountryMasterID ='" + filter.CountryMasterID + "' ";
        //                    }

        //                    if (filter.StateMasterID != null && filter.StateMasterID != Guid.Empty)
        //                    {
        //                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                        IsWhereClauseEmpty = false;
        //                        _SqlQuery += " StateMasterID ='" + filter.StateMasterID + "' ";
        //                    }

        //                    if (filter.CityMasterID != null && filter.CityMasterID != Guid.Empty)
        //                    {
        //                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                        IsWhereClauseEmpty = false;
        //                        _SqlQuery += " CityMasterID ='" + filter.CityMasterID + "' ";
        //                    }

        //                    if (!filter.IncludeIsDeleted)
        //                    {
        //                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
        //                        IsWhereClauseEmpty = false;
        //                        _SqlQuery += " IsActive = 1";
        //                    }

        //                    if (p_GridParams != null)
        //                    {
        //                        _SqlQuery += @"ORDER BY " + p_GridParams.DefaultOrderBy + "OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY";
        //                    }

        //                    objSystemUsers = _DbManager.SetCommand(_SqlQuery).ExecuteList<SystemUsersResponse>();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }

        //            return objSystemUsers;
        //        }

        //        #endregion


        //        #region Add/Update

        //        /// <summary>
        //        /// Add/Update User 
        //        /// </summary>
        //        /// <param name="p_SystemUser"></param>
        //        /// <returns>Return inserted or updated guid</returns>
        //        public Guid AddOrUpdateSystemUser(SystemUsers p_SystemUser)
        //        {
        //            try
        //            {
        //                using (_DbManager = new DbManager())
        //                {
        //                    String _SqlQuery = string.Empty;

        //                    if (p_SystemUser.SystemUsersKey != Guid.Empty)
        //                    {

        //                        #region Update
        //                        _SqlQuery = @"UPDATE [dbo].[SystemUsers]
        //                                   SET [RoleMasterID] = @RoleMasterID,[UserName] = @UserName
        //                                      ,[Password] = @Password,[FirstName] = @FirstName
        //                                      ,[LastName] = @LastName,[MiddleName] = @MiddleName
        //                                      ,[Email] = @Email,[Telephone] = @Telephone
        //                                      ,[Mobile] = @Mobile,[Gender] = @Gender
        //                                      ,[Address] = @Address,[Address2] = @Address2
        //                                      ,[CountryMasterID] = @CountryMasterID,[StateMasterID] = @StateMasterID
        //                                      ,[CityMasterID] = @CityMasterID,[PhotoAlbumID] = @PhotoAlbumID
        //                                      ,[UpdatedBy] = @UpdatedBy,[UpdatedDate] = @UpdatedDate
        //                                      ,[IsActive] = @IsActive
        //                                      ,[UserType] = @UserType
        //                                 WHERE [SystemUsersKey] = @SystemUsersKey";

        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        #region Insert

        //                        _SqlQuery = @"
        //                        INSERT INTO [dbo].[SystemUsers]
        //                       ([SystemUsersKey],[RoleMasterID],[UserName],[Password],[FirstName],[LastName],[MiddleName],[Email],
        //                        [Telephone],[Mobile],[Gender],[Address],[Address2],[CountryMasterID],[StateMasterID],
        //                        [CityMasterID],[PhotoAlbumID],[CreatedBy],[CreatedDate],[IsActive],[UserType])
        //                         VALUES
        //                       (@SystemUsersKey,@RoleMasterID,@UserName,@Password,@FirstName,@LastName,@MiddleName,@Email,
        //                        @Telephone,@Mobile,@Gender,@Address,@Address2,@CountryMasterID,@StateMasterID,
        //                        @CityMasterID,@PhotoAlbumID,@CreatedBy,@CreatedDate,@IsActive,@UserType)";

        //                        p_SystemUser.SystemUsersKey = Guid.NewGuid();
        //                        #endregion

        //                    }

        //                    int Affected = _DbManager.SetCommand(_SqlQuery,
        //                            _DbManager.Parameter("@RoleMasterID", p_SystemUser.RoleMasterID),
        //                            _DbManager.Parameter("@UserName", p_SystemUser.UserName),
        //                            _DbManager.Parameter("@Password", p_SystemUser.Password),
        //                            _DbManager.Parameter("@FirstName", p_SystemUser.FirstName),
        //                            _DbManager.Parameter("@LastName", p_SystemUser.LastName),
        //                            _DbManager.Parameter("@MiddleName", p_SystemUser.MiddleName),
        //                            _DbManager.Parameter("@Email", p_SystemUser.Email),
        //                            _DbManager.Parameter("@Telephone", p_SystemUser.Telephone),
        //                            _DbManager.Parameter("@Mobile", p_SystemUser.Mobile),
        //                            _DbManager.Parameter("@Gender", p_SystemUser.Gender),
        //                            _DbManager.Parameter("@Address", p_SystemUser.Address),
        //                            _DbManager.Parameter("@Address2", p_SystemUser.Address2),
        //                            _DbManager.Parameter("@CountryMasterID", p_SystemUser.CountryMasterID),
        //                            _DbManager.Parameter("@StateMasterID", p_SystemUser.StateMasterID),
        //                            _DbManager.Parameter("@CityMasterID", p_SystemUser.CityMasterID),
        //                            _DbManager.Parameter("@PhotoAlbumID", p_SystemUser.PhotoAlbumID),
        //                            _DbManager.Parameter("@CreatedBy", p_SystemUser.CreatedBy),
        //                            _DbManager.Parameter("@CreatedDate", CommonDateTimeFunction.GetCurrentCstDateTime()),
        //                            _DbManager.Parameter("@UpdatedBy", p_SystemUser.UpdatedBy),
        //                            _DbManager.Parameter("@UpdatedDate", CommonDateTimeFunction.GetCurrentCstDateTime()),
        //                            _DbManager.Parameter("@IsActive", p_SystemUser.IsActive),
        //                            _DbManager.Parameter("@UserType", p_SystemUser.UserType),
        //                            _DbManager.Parameter("@SystemUsersKey", p_SystemUser.SystemUsersKey)).ExecuteNonQuery();

        //                    if (Affected <= 0)
        //                    {
        //                        throw new Exception(Affected + " row(s) affected");
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }

        //            return p_SystemUser.SystemUsersKey;
        //        }

        //        /// <summary>
        //        /// Add/Update User 
        //        /// </summary>
        //        /// <param name="p_SystemUser"></param>
        //        /// <returns>Return inserted or updated guid</returns>
        //        public Guid ChangePassword(Guid SystemUserKey, string OldPassword, string NewPassword)
        //        {
        //            try
        //            {
        //                using (_DbManager = new DbManager())
        //                {
        //                    String _SqlQuery = string.Empty;

        //                    if (SystemUserKey != Guid.Empty)
        //                    {
        //                        _SqlQuery = @"UPDATE [dbo].[SystemUsers]
        //                                   SET [Password] = @NewPassword
        //                                 WHERE [SystemUsersKey] = @SystemUsersKey AND [Password] = @OldPassword";

        //                        int Affected = _DbManager.SetCommand(_SqlQuery,
        //                            _DbManager.Parameter("@SystemUsersKey", SystemUserKey),
        //                            _DbManager.Parameter("@OldPassword", OldPassword),
        //                            _DbManager.Parameter("@NewPassword", NewPassword)
        //                            ).ExecuteNonQuery();

        //                        if (Affected <= 0)
        //                        {
        //                            throw new Exception(Affected + " row(s) affected");
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }

        //            return SystemUserKey;
        //        }



        //        #endregion


        //        #region Delete
        //        /// <summary>
        //        /// Delete user (It will updating only staus not hard )
        //        /// </summary>
        //        /// <param name="SystemUsersKey"></param>
        //        /// <returns></returns>
        //        public Guid DeleteSystemUser(Guid SystemUsersKey)
        //        {
        //            try
        //            {
        //                using (_DbManager = new DbManager())
        //                {
        //                    String _SqlQuery = string.Empty;

        //                    _SqlQuery = @"UPDATE [dbo].[SystemUsers] SET [IsActive] = 0 WHERE [SystemUsersKey] = @SystemUsersKey";


        //                    int Affected = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@SystemUsersKey", SystemUsersKey)).ExecuteNonQuery();

        //                    if (Affected <= 0)
        //                    {
        //                        throw new Exception(Affected + " row(s) affected");
        //                    }

        //                }


        //            }
        //            catch (Exception ex)
        //            {
        //                throw;
        //            }

        //            return SystemUsersKey;
        //        }
        //        #endregion

        public SystemUsers GetSubUsersByIdAndParent(long p_SystemUsersKey, long p_ParentId)
        {
            SystemUsers _SystemUsers = new SystemUsers();
            try
            {
                using (DbManager _DBManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT [Id],[ParentId],[UserType],[RoleMasterId],[UserName],[Password],[FirstName],[LastName],[MiddleName],[Email],
                                                [Telephone],[Mobile],[Gender],[Address],[Address2],[CountryMasterID],[RegionMasterID],[CityMasterID],[LastLoginDate],
                                                [PhotoAlbumID],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate],[IsActive],[CompanyName],[BaseCurrency] FROM [dbo].[SystemUsers] 
                                                Where Id = @SystemUsersKey and ParentId =@ParentId";

                    _SystemUsers = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@SystemUsersKey", p_SystemUsersKey),
                                                _DBManager.Parameter("@ParentId", p_ParentId)).ExecuteObject<SystemUsers>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _SystemUsers;
        }

        public List<SelectListItem> ActivityProviderDropDown(long p_Id)
        {
            List<SelectListItem> _ListOfSystemUsers = new List<SelectListItem>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Select Concat(FirstName, LastName) as Text, cast(Id as varchar(10)) as Value from SystemUsers
                                                Where IsActive= 1 and Id!= @Id and UserType =" + (int)UserTypes.User;
                    _ListOfSystemUsers = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@Id", p_Id)).ExecuteList<SelectListItem>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfSystemUsers;
        }

        public ForgotPasswordModel GetSystemUserByEmail(ForgotPasswordModel p_ForgotPasswordModel)
        {

            ForgotPasswordModel _ForgotPasswordModel = new ForgotPasswordModel();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Select FirstName,Id,Email from SystemUsers
                                                Where Email = @Email and UserType = @UserType";
                    _ForgotPasswordModel = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@Email", p_ForgotPasswordModel.Email),
                        _DbManager.Parameter("@UserType", p_ForgotPasswordModel.UserType)).ExecuteList<ForgotPasswordModel>().FirstOrDefault();

                    if (_ForgotPasswordModel != null)
                    {
                        return _ForgotPasswordModel;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public bool ResetPasswordById(ResetPasswordModel _ResetPasswordModel)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Update SystemUsers set Password = @Password Where Id = @Id";
                    int Affected = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", _ResetPasswordModel.Id),
                        _DbManager.Parameter("@Password", _ResetPasswordModel.ConfirmPassword)).ExecuteNonQuery();

                    if (Affected == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public int GetUserType(SystemUsers p_SystemUser)
        {
            int _Usertype = 0;
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    //String _SqlQuery = @"select UserType from SystemUsers where (UserName=@UserName or Email=@UserName )and Password=@Password";
                    //_Usertype = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("UserName", p_SystemUser.UserName), _DbManager.Parameter("Password", p_SystemUser.Password)).ExecuteScalar<int>();
                    String _SqlQuery = @"select UserType from SystemUsers where Email=@UserName and Password=@Password";
                    _Usertype = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("UserName", p_SystemUser.UserName), _DbManager.Parameter("Password", p_SystemUser.Password)).ExecuteScalar<int>();

                }
            }
            catch (Exception _Exception)
            {
                return 0;
            }

            return _Usertype;
        }

        public bool SetVerifyUser(long p_SystemUsersKey)
        {
            try
            {
                String _SqlQuery = string.Empty;
                int _Result = 0;
                using (DbManager _DBManager = new DbManager())
                {

                    if (p_SystemUsersKey != null && p_SystemUsersKey > 0)
                    {
                        _SqlQuery = "Update SystemUsers set IsVerify = 1 Where Id = @SystemUsersKey";

                        _Result = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@SystemUsersKey", p_SystemUsersKey)).ExecuteNonQuery();

                    }

                    if (_Result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
