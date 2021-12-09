using HSBM.Common.Utils;
using HSBM.EntityModel.Blogs;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.Front;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class BlogService
    {
        /// <summary>
        /// Active/Inactive Blog 
        /// </summary>
        /// <param name="blog">Blog</param>
        public void ActiveAndInactiveSwitchUpdateForBlog(EntityModel.Blogs.Blogs blog)
        {
            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            _BlogsMasterRepository.ActiveAndInactiveSwitchUpdateForBlog(blog);

        }

        /// <summary>
        /// Add / Update Blog
        /// </summary>
        /// <param name="blog">Blogs</param>
        /// <returns> integer </returns>
        public int AddorUpdateBlog(EntityModel.Blogs.Blogs blog)
        {
            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            if (blog.Id > 0)
            {
                return _BlogsMasterRepository.AddorUpdateBlog(blog);
            }
            else
            {
                return _BlogsMasterRepository.AddorUpdateBlog(blog);
            }
        }

        /// <summary>
        /// Delete Blog By Id
        /// </summary>
        /// <param name="Id">Id</param>
        public void DeleteBlogById(long Id)
        {
            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            _BlogsMasterRepository.DeleteBlogById(Id);
        }

        /// <summary>
        /// Grid of Blogs
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">BlogsRequest</param>
        /// <returns> Object of GridDataResponse </returns>
        public GridDataResponse GetAllBlogsBySearchRequest(GridParams p_GridParams, BlogsRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();

            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            var _Country = _BlogsMasterRepository.GetAllBlogsBySearchRequest(p_GridParams, p_SearchRequest);

            if (_Country != null && _Country.Count > 0)
            {
                _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
                _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;                
            }
            _GridDataResponse.data = _Country;

            return _GridDataResponse;
        }

        /// <summary>
        /// Get Blog By Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns> Object of Blogs </returns>
        public Blogs GetBlogById(long Id)
        {
            Blogs _BlogsModel = new Blogs();

            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            _BlogsModel = _BlogsMasterRepository.GetBlogById(Id);

            return _BlogsModel;
        }

        public BlogComment GetBlogCommentsById(long Id)
        {
            BlogComment _BlogsModel = new BlogComment();

            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            _BlogsModel = _BlogsMasterRepository.GetBlogCommentsById(Id);

            return _BlogsModel;
        }

        //public List<BlogCategory> GetAllBlogCategory(RequestResponseServiceContext requestResponseServiceContext)
        //{
        //    List<BlogCategory> _List = new List<BlogCategory>();
        //    BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

        //    try
        //    {
        //        _List = _BlogsMasterRepository.GetAllBlogCategory();
        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
        //    }
        //    catch (Exception _Exception)
        //    {
        //        requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
        //        requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error" };
        //    }

        //    return _List;
        //}

        public List<FrontBlogResponse> BlogListFront(RequestResponseServiceContext requestResponseServiceContext, FrontBlogRequest p_FrontBlogRequest)
        {
            List<FrontBlogResponse> _List = new List<FrontBlogResponse>();
            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            try
            {
                _List = _BlogsMasterRepository.BlogListFront(p_FrontBlogRequest);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
            }
            catch (Exception _Exception)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error" };
            }

            return _List;
        }       

        public List<BlogComment> GetBlogCommentsByBlogId(RequestResponseServiceContext requestResponseServiceContext, int BlogId)
        {
            List<BlogComment> _List = new List<BlogComment>();
            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            try
            {
                _List = _BlogsMasterRepository.GetBlogCommentsByBlogId(BlogId);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
            }
            catch (Exception _Exception)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error" };
            }

            return _List;
        }
        public void AddBlogComment(RequestResponseServiceContext requestResponseServiceContext, EntityModel.Blogs.BlogComment blogComment)
        {
            try
            {
                BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

                int result = _BlogsMasterRepository.AddBlogComment(blogComment);
                if(result > 0)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error" };
                }
            }
            catch(Exception ex)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal server error" };
            }
            
        }

        public GridDataResponse GetAllBlogCommentBySearchRequest(GridParams p_GridParams, BlogsRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();

            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            var _List = _BlogsMasterRepository.GetAllBlogCommentBySearchRequest(p_GridParams, p_SearchRequest);

            if (_List != null && _List.Count > 0)
            {
                _GridDataResponse.recordsTotal = _List.FirstOrDefault().RecordsTotal;
                _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
            }
            _GridDataResponse.data = _List;

            return _GridDataResponse;
        }

        public void ActiveAndInactiveSwitchUpdateForComment(EntityModel.Blogs.BlogComment BlogComment)
        {
            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            _BlogsMasterRepository.ActiveAndInactiveSwitchUpdateForComment(BlogComment);

        }

        public void DeleteBlogCommentById(long Id)
        {
            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            _BlogsMasterRepository.DeleteBlogCommentById(Id);
        }

        public List<FrontBlogResponse> GetAllPopularBlog(RequestResponseServiceContext requestResponseServiceContext)
        {
            List<FrontBlogResponse> _List = new List<FrontBlogResponse>();
            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            try
            {
                _List = _BlogsMasterRepository.GetAllPopularBlog();
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
            }
            catch (Exception _Exception)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error" };
            }

            return _List;

        }

        public List<FrontBlogArchivesResponse> BlogListArchivesFront(RequestResponseServiceContext requestResponseServiceContext)
        {
            List<FrontBlogArchivesResponse> _List = new List<FrontBlogArchivesResponse>();
            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            try
            {
                _List = _BlogsMasterRepository.BlogListArchivesFront();
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
            }
            catch (Exception _Exception)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error" };
            }

            return _List;
        }



        public FarmStaysBlogResponse GetBlogByIdForFront(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            FarmStaysBlogResponse _FarmStaysBlogResponse = new FarmStaysBlogResponse();
            BlogsMasterRepository _BlogsMasterRepository = new BlogsMasterRepository();

            try
            {
                _FarmStaysBlogResponse = _BlogsMasterRepository.GetBlogByIdForFront(Id);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
            }
            catch (Exception _Exception)
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error" };
            }

            return _FarmStaysBlogResponse;
        }
    }
}
