using BLToolkit.Data;
using HSBM.Common.Utils;
using HSBM.EntityModel.AmenityMaster;
using HSBM.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HSBM.Repository.Repositories
{
    public class AmenityMasterRepository
    {
        public GridDataResponse GetAllAmenityMastersBySearchRequest(GridParams p_GridParams, AmenityMasterRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" SELECT *, RecordsTotal = COUNT(*) OVER() from [dbo].[AmenityMaster]";

                    bool IsWhereClauseEmpty = true;

                    if (p_Request.AmenityName != null && p_Request.AmenityName != string.Empty)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;

                        _SqlQuery += @" [AmenityName] LIKE '%" + p_Request.AmenityName + @"%'";
                    }

                    if (p_Request.IncludeIsDeleted!=true)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;

                        _SqlQuery += @" [IsActive] = 'true'";
                    }

                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    var objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<AmenityMasterResponse>();
                    if (objList.Any())
                    {
                        _GridDataResponse.recordsTotal = objList.FirstOrDefault().RecordsTotal;
                        _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    }
                    _GridDataResponse.data = objList;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _GridDataResponse;
        }

        public int AddOrUpdateAmenityMaster(AmenityMaster amenityMaster)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExistBlogName = CheckNameForDuplicate(_DbManager, amenityMaster);

                    if (_IsExistBlogName)
                    {
                        return 2;
                    }
                    else
                    {
                        if (amenityMaster.Id != 0)
                        {
                            _SqlQuery = @"
UPDATE [dbo].[AmenityMaster] SET 
[AmenityName] = @Name
,[ImageUrl] = @ImageUrl
,[IsActive] = @IsActive
,[UpdatedBy] = @UpdatedBy
,[UpdatedDate] = @UpdatedDate
WHERE [Id] = @Id";
                        }
                        else
                        {
                            _SqlQuery = @"
INSERT INTO [dbo].[AmenityMaster]
    ([AmenityName]
    ,[ImageUrl]
    ,[IsActive]
    ,[CreatedBy]
    ,[CreatedDate]
    ,[UpdatedBy]
    ,[UpdatedDate])
VALUES
    (@Name
    ,@ImageUrl
    ,@IsActive
    ,@CreatedBy
    ,@CreatedDate
    ,@UpdatedBy
    ,@UpdatedDate)";
                        }
                        int Affected = _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@Id", amenityMaster.Id),
                            _DbManager.Parameter("@Name", amenityMaster.AmenityName),
                            _DbManager.Parameter("@ImageUrl", amenityMaster.ImageUrl),
                            _DbManager.Parameter("@IsActive", amenityMaster.IsActive),
                            _DbManager.Parameter("@CreatedBy", amenityMaster.CreatedBy),
                            _DbManager.Parameter("@CreatedDate", amenityMaster.CreatedDate),
                            _DbManager.Parameter("@UpdatedBy", null),
                            _DbManager.Parameter("@UpdatedDate", null)).ExecuteNonQuery();

                        if (Affected > 0)
                        {
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

        private bool CheckNameForDuplicate(DbManager _DbManager, AmenityMaster amenityMaster)
        {
            try
            {
                String _SqlQuery = @"SELECT COUNT(Id) FROM [dbo].[AmenityMaster] WHERE [Id] != @Id AND [AmenityName] = @Name";

                int _ExistCount = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", amenityMaster.Id),
                    _DbManager.Parameter("@Name", amenityMaster.AmenityName)).ExecuteScalar<int>();

                if (_ExistCount > 0)
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

        public void ActiveAndInactiveAmenityMaster(AmenityMaster amenity)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"UPDATE [dbo].[AmenityMaster] SET [IsActive] = @IsActive WHERE [Id] = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                                                    _DbManager.Parameter("@Id", amenity.Id),
                                                    _DbManager.Parameter("@IsActive", amenity.IsActive)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public AmenityMaster GetAmenityMasterById(int Id)
        {
            AmenityMaster amenity = new AmenityMaster();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT * FROM [dbo].[AmenityMaster] WHERE [Id] = @Id ";

                    amenity = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<AmenityMaster>().FirstOrDefault();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return amenity;
        }

        public List<SelectListItem> GetAllAmenityMastersForDropDown()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" SELECT AmenityName as Text, Id as Value from [dbo].[AmenityMaster] WHERE [IsActive] = 'true'";

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<SelectListItem>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

    }
}
