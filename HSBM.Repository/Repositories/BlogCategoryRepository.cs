using HSBM.EntityModel.Blogs;
using System.Collections.Generic;
using HSBM.Common.Utils;
using BLToolkit.Data;
using System;
using System.Linq;

namespace HSBM.Repository.Repositories
{
    public class BlogCategoryRepository
    {
        public List<BlogCategory> GetAllBlogCategoryBySearchRequest(GridParams p_GridParams)
        {
            List<BlogCategory> objList = new List<BlogCategory>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT *,RecordsTotal = COUNT(*) OVER() from [dbo].[BlogCategory]";

                    bool IsWhereClauseEmpty = true;
                   
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<BlogCategory>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public int AddorUpdateBlogCategory(BlogCategory blogCategory)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExistBlogName = CheckNameForDuplicate(_DbManager, blogCategory);

                    if (_IsExistBlogName)
                    {
                        return 2;
                    }
                    else
                    {
                        if (blogCategory.Id != 0)
                        {
                            _SqlQuery = @"update BlogCategory set Category=@Category                                                                       
                                                                       ,IsActive = @IsActive
                                                                       ,UpdatedBy = @UpdatedBy
                                                                       ,UpdateDate = @UpdateDate
                                                                        where (Id = @Id)";
                        }
                        else
                        {
                            _SqlQuery = @"insert into BlogCategory
                                                        (Category                                                        
                                                        ,IsActive
                                                        ,CreatedBy
                                                        ,CreatedDate
                                                        )
                                                   values
                                                        (@Category                                                        
                                                        ,@IsActive
                                                        ,@CreatedBy
                                                        ,@CreatedDate
                                                        )";
                        }
                        int Affected = _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@Category", blogCategory.Category),
                            _DbManager.Parameter("@IsActive", blogCategory.IsActive),
                            _DbManager.Parameter("@CreatedBy", blogCategory.CreatedBy),
                            _DbManager.Parameter("@CreatedDate", blogCategory.CreatedDate),
                            _DbManager.Parameter("@UpdatedBy", blogCategory.UpdatedBy),
                            _DbManager.Parameter("@UpdateDate", blogCategory.UpdateDate),
                            _DbManager.Parameter("@Id", blogCategory.Id)).ExecuteNonQuery();

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

        private bool CheckNameForDuplicate(DbManager _DbManager, BlogCategory blogCategory)
        {
            try
            {
                String _SqlQuery = @"select Count(Id) from BlogCategory where Id != @Id and Category = @Category";

                int _ExistCount = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", blogCategory.Id),
                    _DbManager.Parameter("@Category", blogCategory.Category)).ExecuteScalar<int>();

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

        public void ActiveAndInactiveSwitchUpdateForBlogCategory(BlogCategory blogCategory)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Update BlogCategory set IsActive = @IsActive where Id = @Id ";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", blogCategory.Id),
                        _DbManager.Parameter("@IsActive", blogCategory.IsActive)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void DeleteBlogCategoryById(long Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Delete BlogCategory where Id = @Id ";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public BlogCategory GetBlogCategoryById(long Id)
        {
            BlogCategory _BlogCategory = new BlogCategory();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Select * from BlogCategory where Id = @Id ";

                    _BlogCategory = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<BlogCategory>().FirstOrDefault();                    
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _BlogCategory;
        }
    }
}
