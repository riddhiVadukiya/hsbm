using HSBM.EntityModel.Blogs;
using HSBM.EntityModel.Front;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Controllers
{
    public class BlogsController : BaseController
    {
        BlogService _BlogService = new BlogService();

        public ActionResult Index()
        {
            try
            {
                FrontBlogRequest _FrontBlogRequest = new FrontBlogRequest();
                _FrontBlogRequest.PageIndex = 1;

                List<FrontBlogResponse> _BlogList = new List<FrontBlogResponse>();
                _BlogList = _BlogService.BlogListFront(WebRequestResponseServiceContext, _FrontBlogRequest);
                ViewBag.FrontBlogResponse = _BlogList;

                List<FrontBlogResponse> _PopularBlogList = new List<FrontBlogResponse>();
                _PopularBlogList = _BlogService.GetAllPopularBlog(WebRequestResponseServiceContext);
                ViewBag.PopularBlogList = _PopularBlogList;

                //List<BlogCategory> _List = _BlogService.GetAllBlogCategory(WebRequestResponseServiceContext);
                //_List.Insert(0, new BlogCategory { Id = 0, Category = "All" });
                //ViewBag.CategoryList = _List;
                List<FrontBlogArchivesResponse> _BlogArchivesList = new List<FrontBlogArchivesResponse>();
                ViewBag.BlogArchivesList = _BlogService.BlogListArchivesFront(WebRequestResponseServiceContext);
                
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public JsonResult SearchBlogs(FrontBlogRequest p_FrontBlogRequest)
        {
            List<FrontBlogResponse> _List = new List<FrontBlogResponse>();
            try
            {
                _List = _BlogService.BlogListFront(WebRequestResponseServiceContext, p_FrontBlogRequest);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return JsonSuccessResponse(_List, string.Empty);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public JsonResult GetBlogCommentsByBlogId(int BlogId)
        {
            List<BlogComment> _List = new List<BlogComment>();
            try
            {
                _List = _BlogService.GetBlogCommentsByBlogId(WebRequestResponseServiceContext, BlogId);                
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return JsonSuccessResponse(_List, string.Empty);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }        
        public JsonResult AddBlogComment(BlogComment blogComment)
        {
            try
            {
                blogComment.UserId = SessionProxy.CustomerDetails.Id;
                blogComment.CreatedDate = DateTime.UtcNow;
                blogComment.IsApproved = true;

                _BlogService.AddBlogComment(WebRequestResponseServiceContext, blogComment);
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return JsonSuccessResponse(string.Empty);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        public ActionResult Detail(long Id)
        {
            FarmStaysBlogResponse _Blogs = new FarmStaysBlogResponse();
            FrontBlogRequest _FrontBlogRequest = new FrontBlogRequest();
            _FrontBlogRequest.PageIndex = 1;
            try
            {
                _Blogs = _BlogService.GetBlogByIdForFront(WebRequestResponseServiceContext,Id);

                List<FrontBlogResponse> _BlogList = new List<FrontBlogResponse>();
                _BlogList = _BlogService.BlogListFront(WebRequestResponseServiceContext, _FrontBlogRequest);
                ViewBag.FrontBlogResponse = _BlogList;

                List<FrontBlogResponse> _PopularBlogList = new List<FrontBlogResponse>();
                _PopularBlogList = _BlogService.GetAllPopularBlog(WebRequestResponseServiceContext);
                ViewBag.PopularBlogList = _PopularBlogList;

                List<FrontBlogArchivesResponse> _BlogArchivesList = new List<FrontBlogArchivesResponse>();
                ViewBag.BlogArchivesList = _BlogService.BlogListArchivesFront(WebRequestResponseServiceContext);
                
                if (WebRequestResponseServiceContext.Response.StatusCode == StandardStatusCodes.SUCCESS)
                {
                    return View(_Blogs);//JsonSuccessResponse(_Blogs, string.Empty);
                }
                else
                {
                    return JsonErrorResponse(WebRequestResponseServiceContext.Response.StatusMessage);
                }
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Index(string Search, string ArchivesData)
        {
            try
            {
                FrontBlogRequest _FrontBlogRequest = new FrontBlogRequest();
                _FrontBlogRequest.PageIndex = 1;
                if(!string.IsNullOrEmpty(Search))
                {
                    _FrontBlogRequest.keyword = Search;
                    ViewBag.SearchKeyword = Search;
                }
                if (!string.IsNullOrEmpty(ArchivesData))
                    _FrontBlogRequest.ArchivesData = ArchivesData;

                List<FrontBlogResponse> _BlogList = new List<FrontBlogResponse>();
                _BlogList = _BlogService.BlogListFront(WebRequestResponseServiceContext, _FrontBlogRequest);
                ViewBag.FrontBlogResponse = _BlogList;

                List<FrontBlogResponse> _PopularBlogList = new List<FrontBlogResponse>();
                _PopularBlogList = _BlogService.GetAllPopularBlog(WebRequestResponseServiceContext);
                ViewBag.PopularBlogList = _PopularBlogList;

                //List<BlogCategory> _List = _BlogService.GetAllBlogCategory(WebRequestResponseServiceContext);
                //_List.Insert(0, new BlogCategory { Id = 0, Category = "All" });
                //ViewBag.CategoryList = _List;
                List<FrontBlogArchivesResponse> _BlogArchivesList = new List<FrontBlogArchivesResponse>();
                ViewBag.BlogArchivesList = _BlogService.BlogListArchivesFront(WebRequestResponseServiceContext);
                                
                return View("index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }        
    }
}