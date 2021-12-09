using BLToolkit.Data;
using HSBM.Common.Utils;
using HSBM.EntityModel.CategoryMaster;
using HSBM.EntityModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HSBM.Repository.Repositories
{
    public class CategoryMasterRepository
    {
        public GridDataResponse GetAllCategoryMastersBySearchRequest(GridParams p_GridParams, CategoryMasterRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" SELECT *, RecordsTotal = COUNT(*) OVER() from [dbo].[CategoryMaster]";

                    bool IsWhereClauseEmpty = true;

                    if (p_Request.CategoryName != null && p_Request.CategoryName != string.Empty)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;

                        _SqlQuery += @" [CategoryName] LIKE '%" + p_Request.CategoryName + @"%'";
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

                    var objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<CategoryMasterResponse>();
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

        public int AddOrUpdateCategoryMaster(CategoryMaster CategoryMaster)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExistBlogName = CheckNameForDuplicate(_DbManager, CategoryMaster);

                    if (_IsExistBlogName)
                    {
                        return 2;
                    }
                    else
                    {
                        if (CategoryMaster.Id != 0)
                        {
                            _SqlQuery = @"
                        UPDATE [dbo].[CategoryMaster] SET 
                        [CategoryName] = @Name
                        ,[IsActive] = @IsActive
                        ,[UpdatedBy] = @UpdatedBy
                        ,[UpdatedDate] = @UpdatedDate
                        WHERE [Id] = @Id";
                        }
                        else
                        {
                            _SqlQuery = @"
INSERT INTO [dbo].[CategoryMaster]
    ([CategoryName]
    ,[IsActive]
    ,[CreatedBy]
    ,[CreatedDate]
    ,[UpdatedBy]
    ,[UpdatedDate])
VALUES
    (@Name
    ,@IsActive
    ,@CreatedBy
    ,@CreatedDate
    ,@UpdatedBy
    ,@UpdatedDate)";
                        }
                        int Affected = _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@Id", CategoryMaster.Id),
                            _DbManager.Parameter("@Name", CategoryMaster.CategoryName),
                            _DbManager.Parameter("@IsActive", CategoryMaster.IsActive),
                            _DbManager.Parameter("@CreatedBy", CategoryMaster.CreatedBy),
                            _DbManager.Parameter("@CreatedDate", CategoryMaster.CreatedDate),
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

        private bool CheckNameForDuplicate(DbManager _DbManager, CategoryMaster CategoryMaster)
        {
            try
            {
                String _SqlQuery = @"SELECT COUNT(Id) FROM [dbo].[CategoryMaster] WHERE [Id] != @Id AND [CategoryName] = @Name";

                int _ExistCount = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", CategoryMaster.Id),
                    _DbManager.Parameter("@Name", CategoryMaster.CategoryName)).ExecuteScalar<int>();

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

        public void ActiveAndInactiveCategoryMaster(CategoryMaster Category)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"UPDATE [dbo].[CategoryMaster] SET [IsActive] = @IsActive WHERE [Id] = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                                                    _DbManager.Parameter("@Id", Category.Id),
                                                    _DbManager.Parameter("@IsActive", Category.IsActive)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public CategoryMaster GetCategoryMasterById(int Id)
        {
            CategoryMaster Category = new CategoryMaster();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT * FROM [dbo].[CategoryMaster] WHERE [Id] = @Id ";

                    Category = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<CategoryMaster>().FirstOrDefault();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return Category;
        }

        public List<SelectListItem> GetAllCategoryMastersForDropDown()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" SELECT CategoryName as Text, Id as Value from [dbo].[CategoryMaster] WHERE [IsActive] = 'true'";

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
