using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Blogs;
using HSBM.EntityModel.Common;
using HSBM.Service.Contracts;
using HSBM.Service.ServiceContext;
using HSBM.Service.Services;
using HSBM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class BlogsController : BaseController
    {

        BlogService _BlogService = new BlogService();

        #region Blogs

        [CustomAuthorizeAction(Module.Blogs, ModuleAccess.CanView)]
        public ActionResult Index()
        {
            if (TempData["ToastrMSG"] != null)
            {
                ViewBag.ToastrMSG = (ToastrMSG)TempData["ToastrMSG"];
            }
            TempData["ToastrMSG"] = null;
            return View();
        }

        [CustomAuthorizeAction(Module.Blogs, ModuleAccess.CanView)]
        public JsonResult GetAllBlogsBySearchRequest(BlogsRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Id Desc";

                GridDataResponse _GridDataResponse = _BlogService.GetAllBlogsBySearchRequest(p_GridParams, p_SearchRequest);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        [CustomAuthorizeAction(Module.Blogs, ModuleAccess.CanAdd)]
        public ActionResult AddBlog()
        {            
            return PartialView("AddUpdateBlog", new Blogs());
        }

        [CustomAuthorizeAction(Module.Blogs, ModuleAccess.CanUpdate)]
        public ActionResult UpdateBlog(long Id)
        {
            return PartialView("AddUpdateBlog", _BlogService.GetBlogById(Id));
        }

        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUpdateBlog(Blogs blog, HttpPostedFileBase file)
        {
            var fileNameAuto = string.Empty;
            if (file != null && file.ContentLength > 0)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string _Path = Server.MapPath("~" + MvcApplication.BlogImagePath);
                    if (!System.IO.Directory.Exists(_Path))
                        System.IO.Directory.CreateDirectory(_Path);
                    
                    var fileName = Path.GetFileName(file.FileName);
                    fileNameAuto = Guid.NewGuid() + "-" + Path.GetFileName(file.FileName);
                    fileNameAuto = fileNameAuto.Replace(" ", "");
                    //var path = Server.MapPath("~" + MvcApplication.BlogImagePath);
                    var path = Path.Combine(_Path, fileNameAuto);
                    file.SaveAs(path);
                    blog.Image = fileNameAuto;
                }
            }
            else
            {
                if(blog.Id == 0)
                {
                    ViewBag.MessageImage = "Please select Blog image";
                    return PartialView("AddUpdateBlog", blog);
                }                
            }

            if (blog.Id > 0)
            {
                var _Blog = _BlogService.GetBlogById(blog.Id);
                blog.CreatedBy = _Blog.CreatedBy;
                blog.CreatedDate = _Blog.CreatedDate;
                blog.UpdatedBy = SessionProxy.UserDetails.Id;
                blog.UpdateDate = DateTime.Now;
                blog.IsActive = _Blog.IsActive;

            }
            else
            {
                blog.CreatedBy = SessionProxy.UserDetails.Id;
                blog.CreatedDate = DateTime.Now;
                blog.IsActive = true;                  
            }

            if(fileNameAuto != string.Empty)
            {
                blog.Image = fileNameAuto;
            }            

            int Affected = _BlogService.AddorUpdateBlog(blog);

            if (Affected == 1)
            {
                //ViewBag.Message = "BlogName has been added sucessfully.";
                TempData["ToastrMSG"] = new ToastrMSG() { Message = (blog.Id > 0 ? "Blog has been updated sucessfully." : "Blog has been added sucessfully."), Type = "success", ErrorTitle = "Success" };
                return RedirectToAction("Index");
            }
            else if (Affected == 2)
            {                
                ViewBag.Message = "Blog Name Already Exist";
                return PartialView("AddUpdateBlog", blog);
            }
            else
            {
                //ViewBag.Message = "Error in Add/Update Blog";
                //return PartialView("AddUpdateBlog", blog);
                TempData["ToastrMSG"] = new ToastrMSG() { Message = "Error in Add/Update Blog.", Type = "error", ErrorTitle = "Error" };
                return RedirectToAction("Index");
            }
        }

        [CustomAuthorizeAction(Module.Blogs, ModuleAccess.CanDelete)]
        public ActionResult DeleteBlog(long Id)
        {
            _BlogService.DeleteBlogById(Id);
            return RedirectToAction("Index");
        }

        #endregion

        [CustomAuthorizeAction(Module.BlogsComment, ModuleAccess.CanView)]
        public ActionResult GetBlogCommentsById(long Id)
        {            
            return PartialView("ViewBlogComment", _BlogService.GetBlogCommentsById(Id));
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult ActiveAndInactiveSwitchUpdate(Blogs blog)
        {
            _BlogService.ActiveAndInactiveSwitchUpdateForBlog(blog);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorizeAction(Module.BlogsComment, ModuleAccess.CanView)]
        public ActionResult Comments(long Id)
        {
            return View();
        }

        [CustomAuthorizeAction(Module.BlogsComment, ModuleAccess.CanView)]
        public JsonResult GetAllBlogCommentBySearchRequest(BlogsRequest p_SearchRequest)
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Id Desc";

                if (p_GridParams.DefaultOrderBy.Contains("strCreatedDate"))
                {
                    p_GridParams.DefaultOrderBy = p_GridParams.DefaultOrderBy.Replace("strCreatedDate", "CreatedDate");
                }
                GridDataResponse _GridDataResponse = _BlogService.GetAllBlogCommentBySearchRequest(p_GridParams, p_SearchRequest);               
                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        [CustomAuthorizeAction(Module.BlogsComment, ModuleAccess.CanUpdate)]
        public JsonResult ActiveAndInactiveSwitchUpdateForComment(BlogComment blogComment)
        {
            _BlogService.ActiveAndInactiveSwitchUpdateForComment(blogComment);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorizeAction(Module.BlogsComment, ModuleAccess.CanDelete)]
        public ActionResult DeleteBlogComment(long CommentId, long BlogId)
        {
            _BlogService.DeleteBlogCommentById(CommentId);
            return RedirectToAction("Comments", new { Id = BlogId });
        } 
    }
}