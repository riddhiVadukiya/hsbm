using BLToolkit.Data;
using HSBM.EntityModel.RoleMaster;
using HSBM.EntityModel.RoleMasterDetails;
using HSBM.EntityModel.RoleModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HSBM.Repository.Repositories
{
    public class RoleMasterRepository
    {
        public EntityModel.RoleMaster.RoleMaster GetRoleMasterForAdd(bool IsAdmin)
        {
            RoleMaster _RoleMaster = new RoleMaster();
            String _SqlQuery = string.Empty;

            try
            {
                using (DbManager _DbManager = new DbManager())
                {

                    _SqlQuery = @"select distinct RoleModuleID , RMD.CanAdd , RMD.CanUpdate , RMD.CanDelete , RMD.CanView from RoleModule as RM
                                             left join RoleMasterDetails as RMD on RMD.RoleMasterId != 0 and RM.Id = RMD.RoleModuleID 
                                             where RMD.CanAdd = 0 and RMD.CanUpdate = 0
											 and RMD.CanDelete = 0 and RMD.CanView = 0 And RM.IsAdmin = " + (IsAdmin ? 1 : 0);

                    _SqlQuery = @"Select RM.Id as RoleModuleID, 0 as CanAdd , 0 as CanUpdate , 0 as CanDelete , 0 as CanView From RoleModule as RM
                                Where  RM.IsAdmin = " + (IsAdmin ? 1 : 0);

                    _RoleMaster.RoleMasterDetails = _DbManager.SetCommand(_SqlQuery).ExecuteList<RoleMasterDetails>();

                    if (_RoleMaster.RoleMasterDetails.Count > 0)
                    {
                        foreach (var m in _RoleMaster.RoleMasterDetails)
                        {
                            _SqlQuery = @"select ModuleName, ID from RoleModule where Id = @Id";

                            m.RoleModule = _DbManager.SetCommand(_SqlQuery,
                                _DbManager.Parameter("@Id", m.RoleModuleID)).ExecuteList<RoleModule>().FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _RoleMaster;
        }

        public List<RoleMasterResponse> GetAllRoleMaster(RoleMasterRequest p_SearchRequest, Common.Utils.GridParams p_GridParams)
        {
            List<RoleMasterResponse> objList = new List<RoleMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[RoleName],[IsActive],[Isdefault],RecordsTotal = COUNT(*) OVER() FROM [dbo].[RoleMaster] ";

                    bool IsWhereClauseEmpty = true;
                    if (p_SearchRequest.IncludeIsDeleted != true)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;

                        _SqlQuery += @" [IsActive] = 'true'";
                    }



                    if (!string.IsNullOrEmpty(p_SearchRequest.RoleName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += @" RoleName like '%" + p_SearchRequest.RoleName + "%'";
                    }

                    if (p_SearchRequest.Isdefault)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;

                        _SqlQuery += @" Isdefault = 1";
                    }
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<RoleMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public int AddUpdateRoleMaster(RoleMaster roleMaster)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExistRoleName = CheckNameForDuplicate(_DbManager, roleMaster);

                    if (_IsExistRoleName)
                    {
                        return 2;
                    }
                    else
                    {
                        if (roleMaster.Id != 0)
                        {
                            _SqlQuery = @"
update RoleMaster set RoleName=@RoleName
,IsActive = @IsActive
,IsAdmin = @IsAdmin
where (Id = @Id)";
                        }
                        else
                        {
                            _SqlQuery = @"
insert into RoleMaster
     (RoleName
     ,IsActive     
     ,IsAdmin)
OUTPUT INSERTED.Id
values
    (@RoleName
    ,@IsActive
    ,@IsAdmin)";
                        }
                        int id = _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@RoleName", roleMaster.RoleName),
                            _DbManager.Parameter("@IsActive", roleMaster.IsActive),                            
                            _DbManager.Parameter("@IsAdmin", roleMaster.IsAdmin),
                            _DbManager.Parameter("@Id", roleMaster.Id)).ExecuteScalar<int>();

                        if (roleMaster.Id == 0)
                        {
                            roleMaster.Id = id;
                        }

                        if (roleMaster.Id > 0)
                        {
                            if (roleMaster.RoleMasterDetails != null)
                            {
                                _SqlQuery = @"Delete RoleMasterDetails where RoleMasterId = @Id";

                                _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@Id", roleMaster.Id)).ExecuteNonQuery();

                                foreach (RoleMasterDetails m in roleMaster.RoleMasterDetails)
                                {
                                    _SqlQuery = @"
insert into RoleMasterDetails
    (RoleMasterId
    ,RoleModuleID
    ,CanAdd
    ,CanUpdate
    ,CanDelete
    ,CanView)
values
    (@RoleMasterId
    ,@RoleModuleID
    ,@CanAdd
    ,@CanUpdate
    ,@CanDelete
    ,@CanView)";

                                    _DbManager.SetCommand(_SqlQuery,
                                       _DbManager.Parameter("@RoleMasterID", roleMaster.Id),
                                       _DbManager.Parameter("@RoleModuleID", m.RoleModuleID),
                                       _DbManager.Parameter("@CanAdd", m.CanAdd),
                                       _DbManager.Parameter("@CanUpdate", m.CanUpdate),
                                       _DbManager.Parameter("@CanDelete", m.CanDelete),
                                       _DbManager.Parameter("@CanView", m.CanView),
                                       _DbManager.Parameter("@Id", m.Id)).ExecuteNonQuery();
                                }
                            }
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private bool CheckNameForDuplicate(DbManager _DbManager, RoleMaster roleMaster)
        {
            try
            {
                String _SqlQuery = @"Select Count(Id) from RoleMaster where Id!=@Id and RoleName=@RoleName";

                int Affected = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", roleMaster.Id),
                    _DbManager.Parameter("@RoleName", roleMaster.RoleName)).ExecuteScalar<int>();

                if (Affected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public RoleMaster GetRoleMasterById(long Id)
        {
            RoleMaster _RoleMaster = new RoleMaster();
            RoleMasterDetails _RoleMasterDetails = new RoleMasterDetails();
            try
            {
                string _SqlQuery = string.Empty;
                using (DbManager _DbManager = new DbManager())
                {
                    _SqlQuery = @"SELECT Id , RoleName, IsActive, Isdefault from RoleMaster where Id = @Id";

                    _RoleMaster = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", Id)).ExecuteList<RoleMaster>().FirstOrDefault();

                    if (_RoleMaster != null)
                    {
                        _SqlQuery = @"Select CanAdd, CanDelete, CanUpdate ,CanView, RoleMasterId,RoleModuleId FROM RoleMasterDetails
                                        where RoleMasterId = @Id";

                        _RoleMaster.RoleMasterDetails = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", _RoleMaster.Id)).ExecuteList<RoleMasterDetails>();
                    }

                    if (_RoleMaster.RoleMasterDetails != null && _RoleMaster.RoleMasterDetails.Count > 0)
                    {
                        foreach (var m in _RoleMaster.RoleMasterDetails)
                        {
                            _SqlQuery = @"SELECT Id,ModuleName from RoleModule where Id=@Id ";

                            m.RoleModule = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@Id", m.RoleModuleID)).ExecuteList<RoleModule>().FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _RoleMaster;
        }

        public bool DeleteRoleMasterById(long Id)
        {
            try
            {
                String _sqlQuery = string.Empty;

                using (DbManager _DbManager = new DbManager())
                {
                    _sqlQuery = @"Delete RoleMasterDetails where RoleMasterId = @Id ";

                    _DbManager.SetCommand(_sqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                    _sqlQuery = @"Delete RoleMaster where Id = @Id ";

                    int Affected = _DbManager.SetCommand(_sqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void ActiveAndInactiveSwitchUpdateForRole(RoleMaster roleMaster)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Update RoleMaster set IsActive = @IsActive where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", roleMaster.Id),
                        _DbManager.Parameter("@IsActive", roleMaster.IsActive)).ExecuteNonQuery();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<RoleMasterDetails> Execute(long RoleMasterId)
        {
            List<RoleMasterDetails> _RoleMasterDetailList = new List<RoleMasterDetails>();

            try
            {
                String _SqlQuery = string.Empty;

                using (DbManager _DbManager = new DbManager())
                {

                    _SqlQuery = @"Select CanAdd, CanDelete, CanUpdate ,CanView, RoleMasterId,RoleModuleId FROM RoleMasterDetails
                                        where RoleMasterId = @Id And RoleMasterId in (select id from rolemaster where IsActive=1)";

                    _RoleMasterDetailList = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", RoleMasterId)).ExecuteList<RoleMasterDetails>();

                    if (_RoleMasterDetailList != null && _RoleMasterDetailList.Count > 0)
                    {
                        foreach (var m in _RoleMasterDetailList)
                        {
                            _SqlQuery = @"SELECT Id,ModuleName from RoleModule where Id=@Id";

                            m.RoleModule = _DbManager.SetCommand(_SqlQuery,
                                 _DbManager.Parameter("@Id", m.RoleModuleID)).ExecuteList<RoleModule>().FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _RoleMasterDetailList;
        }

        public List<SelectListItem> RoleDropDown()
        {

            List<SelectListItem> _RoleList = new List<SelectListItem>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Select RoleName as Text, Id as Value from RoleMaster where IsActive = 1";

                    _RoleList = _DbManager.SetCommand(_SqlQuery).ExecuteList<SelectListItem>();

                    if (_RoleList.Count > 0)
                    {
                        return _RoleList;
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

        public bool UsersExistInRole(long RoleId)
        {
            bool result = false;
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"select count(*) from rolemaster where id=@RoleMasterId and id in (select rolemasterid from SystemUsers)";
                    int _Count = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@RoleMasterId", RoleId)).ExecuteScalar<int>();
                    if (_Count > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return result;
        }
    }
}
