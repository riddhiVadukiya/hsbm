using HSBM.Repository.Contracts;
using HSBM.EntityModel.Blogs;
using System.Collections.Generic;
using HSBM.Common.Utils;
using BLToolkit.Data;
using System;
using System.Linq;
using HSBM.EntityModel.Front;

namespace HSBM.Repository.Repositories
{
    public class BlogsMasterRepository
    {
        public List<BlogsResponse> GetAllBlogsBySearchRequest(GridParams p_GridParams, BlogsRequest p_SearchRequest)
        {
            List<BlogsResponse> objList = new List<BlogsResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT *,(select Count(*) from BlogComment C where C.BlogId=B.Id) as BlogCommentCount,RecordsTotal = COUNT(*) OVER() from Blogs B";

                    bool IsWhereClauseEmpty = true;

                    if (!string.IsNullOrEmpty( p_SearchRequest.Title))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " B.Title like '%" + p_SearchRequest .Title+ "%' ";
                    }

                    if (!p_SearchRequest.IncludeIsDeleted)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " B.IsActive = 1 ";
                    }
                    if (p_SearchRequest.Popular)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " B.IsPopulerPost = 1 ";
                    }
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY B." + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<BlogsResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public int AddorUpdateBlog(Blogs blog)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExistBlogName = CheckNameForDuplicate(_DbManager, blog);

                    if (_IsExistBlogName)
                    {
                        return 2;
                    }
                    else
                    {
                        if (blog.Id != 0)
                        {
                            _SqlQuery = @"update Blogs set Title=@Title
                                                                       ,Description = @Description
                                                                       ,MetaTitle = @MetaTitle
                                                                       ,MetaKeyword = @MetaKeyword
                                                                       ,MetaDescription = @MetaDescription
                                                                        ,Categories = @Categories
                                                                        ,Image = @Image
                                                                        ,IsPopulerPost = @IsPopulerPost
                                                                       ,IsActive = @IsActive
                                                                       ,UpdatedBy = @UpdatedBy
                                                                       ,UpdateDate = @UpdateDate
                                                                        where (Id = @Id)";
                        }
                        else
                        {
                            _SqlQuery = @"insert into Blogs
                                                        (Title
                                                        ,Description
                                                        ,MetaTitle
                                                        ,MetaKeyword
                                                        ,MetaDescription
                                                        ,Categories
                                                        ,Image
                                                        ,IsPopulerPost
                                                        ,IsActive
                                                        ,CreatedBy
                                                        ,CreatedDate
                                                        ,UpdatedBy
                                                        ,UpdateDate)
                                                   values
                                                        (@Title
                                                        ,@Description
                                                        ,@MetaTitle
                                                        ,@MetaKeyword
                                                        ,@MetaDescription
                                                        ,@Categories
                                                        ,@Image
                                                        ,@IsPopulerPost
                                                        ,@IsActive
                                                        ,@CreatedBy
                                                        ,@CreatedDate
                                                        ,@UpdatedBy
                                                        ,@UpdateDate)";
                        }
                        int Affected = _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@Title", blog.Title),
                            _DbManager.Parameter("@Description", blog.Description),
                            _DbManager.Parameter("@MetaTitle", blog.MetaTitle),
                            _DbManager.Parameter("@MetaKeyword", blog.MetaKeyword),
                            _DbManager.Parameter("@MetaDescription", blog.MetaDescription),
                            _DbManager.Parameter("@Image", blog.Image),
                            _DbManager.Parameter("@Categories", blog.Categories),
                            _DbManager.Parameter("@IsPopulerPost", blog.IsPopulerPost),
                            _DbManager.Parameter("@IsActive", blog.IsActive),
                            _DbManager.Parameter("@CreatedBy", blog.CreatedBy),
                            _DbManager.Parameter("@CreatedDate", blog.CreatedDate),
                            _DbManager.Parameter("@UpdatedBy", blog.UpdatedBy),
                            _DbManager.Parameter("@UpdateDate", blog.UpdateDate),
                            _DbManager.Parameter("@Id", blog.Id)).ExecuteNonQuery();

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

        private bool CheckNameForDuplicate(DbManager _DbManager, Blogs blog)
        {
            try
            {
                String _SqlQuery = @"select Count(Id) from Blogs where Id != @Id and Title = @Title";

                int _ExistCount = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", blog.Id),
                    _DbManager.Parameter("@Title",blog.Title)).ExecuteScalar<int>();

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

        public void ActiveAndInactiveSwitchUpdateForBlog(Blogs blog)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Update Blogs set IsActive = @IsActive where Id = @Id ";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", blog.Id),
                        _DbManager.Parameter("@IsActive", blog.IsActive)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void DeleteBlogById(long Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Delete Blogs where Id = @Id ";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public Blogs GetBlogById(long Id)
        {
            Blogs _Blogs = new Blogs();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Select Id,Title,Description,MetaTitle,MetaKeyword,MetaDescription,IsActive,CreatedDate,CreatedBy,Categories,Image,IsPopulerPost from Blogs where Id = @Id ";

                    _Blogs = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<Blogs>().FirstOrDefault();

                    //if(_Blogs != null && _Blogs.Categories != null && _Blogs.Categories != string.Empty)
                    //{
                    //    _SqlQuery = @"Select Id,Category from BlogCategory where Id in (" + _Blogs.Categories + ")";

                    //    _Blogs.CategoriesWithName = _DbManager.SetCommand(_SqlQuery).ExecuteList<BlogCategory>();
                    //}

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _Blogs;
        }

        public BlogComment GetBlogCommentsById(long BlogCommentId)
        {
            BlogComment objList = new BlogComment();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" Select B.*, isnull(S.firstname,'') + ' ' + isnull(S.lastname,'') as CommentBy, 
                                            (select Title from Blogs  where Id = B.BlogId ) as Title from BlogComment B 
                                             Left Join SystemUsers S on B.UserId = S.Id Where  B.id = " + BlogCommentId ;

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteObject<BlogComment>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public List<BlogCategory> GetAllBlogCategory()
        {
            List<BlogCategory> _List = new List<BlogCategory>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"
select Id, Category from BlogCategory Where IsActive = 1 order by Category";

                    _List = _DbManager.SetCommand(_SqlQuery).ExecuteList<BlogCategory>();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _List;
        }

        public List<FrontBlogResponse> BlogListFront(FrontBlogRequest p_FrontBlogRequest)
        {
            List<FrontBlogResponse> objList = new List<FrontBlogResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT *,(select count(*) from blogcomment where blogid=blogs.id and IsApproved=1) as CommentCount
                                        ,(select isnull(firstname,'') + ' ' + isnull(lastname,'') from SystemUsers where id=blogs.createdby) as Author                            
                                        ,RecordsTotal = COUNT(*) OVER() from [dbo].[Blogs] Where IsActive = 1 ";

                    if (p_FrontBlogRequest.keyword != null && p_FrontBlogRequest.keyword != string .Empty)
                    {
                        _SqlQuery += " and (Title like '%" + p_FrontBlogRequest.keyword + "%' OR Description like '%" + p_FrontBlogRequest.keyword + "%') ";
                    }
                    if (p_FrontBlogRequest.CategoryId != null && p_FrontBlogRequest.CategoryId != 0)
                    {                       
                        _SqlQuery += " and categories like '%" + p_FrontBlogRequest.CategoryId + "%' ";
                    }   
                    if (!string.IsNullOrEmpty(p_FrontBlogRequest.ArchivesData))
                    {
                        _SqlQuery += " AND UPPER((DATENAME(MONTH,CreatedDate))) + ' '+ CAST(YEAR(CreatedDate) AS VARCHAR(4)) = '" + p_FrontBlogRequest.ArchivesData + "' ";
                    }   
                                  
                    _SqlQuery += @" ORDER BY Blogs.Id Desc OFFSET " + ((p_FrontBlogRequest.PageIndex - 1) * 6) + " ROWS FETCH NEXT 6 ROWS ONLY ";

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<FrontBlogResponse>();

                    //foreach (FrontBlogResponse _FrontBlogResponse in objList)
                    //{
                    //    if(!string.IsNullOrEmpty(_FrontBlogResponse.Categories))
                    //    {
                    //        _SqlQuery = @"Select Category from BlogCategory where Id in (" + _FrontBlogResponse.Categories + ")";

                    //        string cat = string.Empty;
                    //        foreach (string str in _DbManager.SetCommand(_SqlQuery).ExecuteScalarList<string>())
                    //        {
                    //            if(cat != string.Empty)
                    //            {
                    //                cat += ", ";
                    //            }
                    //            cat += str;
                    //        }
                    //        _FrontBlogResponse.Categories = cat;
                    //    }                        
                    //}                    
                    
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }     

        public List<BlogComment> GetBlogCommentsByBlogId(int BlogId)
        {
            List<BlogComment> objList = new List<BlogComment>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"Select B.*, isnull(S.firstname,'') + ' ' + isnull(S.lastname,'') as CommentBy from BlogComment B Left Join SystemUsers S on B.UserId = S.Id 
                                        Where B.IsApproved = 1 And B.BlogId = " + BlogId + " Order by B.Id desc";

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<BlogComment>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public int AddBlogComment(BlogComment blogComment)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {

                    _SqlQuery = @"insert into BlogComment
                                                        (BlogId                                                        
                                                        ,UserId
                                                        ,Comment
                                                        ,IsApproved
                                                        ,CreatedDate
                                                        )
                                                   values
                                                        (@BlogId                                                        
                                                        ,@UserId
                                                        ,@Comment
                                                        ,@IsApproved
                                                        ,@CreatedDate
                                                        )";
                    int Affected = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@BlogId", blogComment.BlogId),
                        _DbManager.Parameter("@UserId", blogComment.UserId),
                        _DbManager.Parameter("@Comment", blogComment.Comment),
                        _DbManager.Parameter("@IsApproved", blogComment.IsApproved),
                        _DbManager.Parameter("@CreatedDate", blogComment.CreatedDate)).ExecuteNonQuery();

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
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<BlogComment> GetAllBlogCommentBySearchRequest(GridParams p_GridParams, BlogsRequest p_SearchRequest)
        {
            List<BlogComment> objList = new List<BlogComment>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT A.*,B.Title,isnull(c.firstname,'') + ' ' + isnull(c.lastname,'') as CommentBy,  RecordsTotal = COUNT(*) OVER() from [dbo].[BlogComment] a
                                        inner Join Blogs b on a.BlogId=b.Id and b.Id="+p_SearchRequest.Id+ " join SystemUsers c on a.UserId=c.Id";

                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<BlogComment>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public void ActiveAndInactiveSwitchUpdateForComment(BlogComment blogComment)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Update blogComment set IsApproved = @IsApproved where Id = @Id ";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", blogComment.Id),
                        _DbManager.Parameter("@IsApproved", blogComment.IsApproved)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void DeleteBlogCommentById(long Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Delete blogComment where Id = @Id ";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<FrontBlogResponse> GetAllPopularBlog()
        {
            List<FrontBlogResponse> objList = new List<FrontBlogResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT Top 5 * from [dbo].[Blogs] where IsPopulerPost = 1 and IsActive = 1";

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<FrontBlogResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }
        public List<FrontBlogArchivesResponse> BlogListArchivesFront()
        {
            List<FrontBlogArchivesResponse> objList = new List<FrontBlogArchivesResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT UPPER((DATENAME(MONTH,CreatedDate))) + ' ' + CAST(YEAR(CreatedDate) AS VARCHAR(4)) AS ArchivesData
                                        FROM [Blogs]
                                        WHERE IsActive = 1
                                        GROUP BY UPPER((DATENAME(MONTH,CreatedDate))) + ' '+ CAST(YEAR(CreatedDate) AS VARCHAR(4))";

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<FrontBlogArchivesResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public FarmStaysBlogResponse GetBlogByIdForFront(long Id)
        {
            FarmStaysBlogResponse _FarmStaysBlogResponse = new FarmStaysBlogResponse();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"Select Id,Title,Description,MetaTitle,MetaKeyword,MetaDescription,IsActive,CreatedDate,CreatedBy,Categories,Image,IsPopulerPost from Blogs where Id = @Id ";

                    _FarmStaysBlogResponse = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteObject<FarmStaysBlogResponse>();

                    _SqlQuery = @"Select B.*, isnull(S.firstname,'') + ' ' + isnull(S.lastname,'') as CommentBy from BlogComment B Left Join SystemUsers S on B.UserId = S.Id 
                                        Where B.IsApproved = 1 And B.BlogId = " + Id + " Order by B.Id desc";

                    _FarmStaysBlogResponse.listBlogComment = _DbManager.SetCommand(_SqlQuery).ExecuteList<BlogComment>();

                    return _FarmStaysBlogResponse;

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _FarmStaysBlogResponse;
        }
        
    }
}