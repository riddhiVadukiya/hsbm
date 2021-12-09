using HSBM.Common.Utils;
using HSBM.EntityModel.Blogs;
using HSBM.EntityModel.Common;
using HSBM.Service;
using HSBM.Web.Helpers;
using System;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class BlogCategoryController : BaseController
    {
        BlogCategoryService _BlogCategoryService = new BlogCategoryService();

        #region BlogCategory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddBlogCategory()
        {
            return PartialView("AddUpdateBlogCategory", new BlogCategory());
        }

        public ActionResult UpdateBlogCategory(long Id)
        {
            return PartialView("AddUpdateBlogCategory", _BlogCategoryService.GetBlogCategoryById(Id));
        }

        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUpdateBlogCategory(BlogCategory blogCategory)
        {
            if (blogCategory.Id > 0)
            {
                var _BlogCategory = _BlogCategoryService.GetBlogCategoryById(blogCategory.Id);
                blogCategory.CreatedBy = _BlogCategory.CreatedBy;
                blogCategory.CreatedDate = _BlogCategory.CreatedDate;
                blogCategory.UpdatedBy = SessionProxy.UserDetails.Id;
                blogCategory.UpdateDate = DateTime.Now;
            }
            else
            {
                blogCategory.CreatedBy = SessionProxy.UserDetails.Id;
                blogCategory.CreatedDate = DateTime.Now;
            }

            int Affected = _BlogCategoryService.AddorUpdateBlogCategory(blogCategory);

            if (Affected == 1)
            {
                return RedirectToAction("Index");
            }
            else if (Affected == 2)
            {
                ViewBag.Message = "Blog Category Already Exist";
                return PartialView("AddUpdateBlogCategory", blogCategory);
            }
            else
            {
                ViewBag.Message = "Error in Add/Update Blog Category";
                return PartialView("AddUpdateBlogCategory", blogCategory);
            }

        }

        public ActionResult DeleteBlogCategory(long Id)
        {
            _BlogCategoryService.DeleteBlogCategoryById(Id);
            return RedirectToAction("Index");
        }
        #endregion

        public JsonResult GetAllBlogCategoryBySearchRequest()
        {
            try
            {
                GridParams p_GridParams = Helpers.Utility.GetGridParams(Request.Params);

                if (Convert.ToInt16(Request.Params["order[0][column]"]) == 0)
                    p_GridParams.DefaultOrderBy = "Category";

                GridDataResponse _GridDataResponse = _BlogCategoryService.GetAllBlogCategoryBySearchRequest(p_GridParams);

                return Json(_GridDataResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception _Exception)
            {
                return JsonErrorResponse(_Exception);
            }
        }


        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ActiveAndInactiveSwitchUpdate(BlogCategory blogCategory)
        {
            _BlogCategoryService.ActiveAndInactiveSwitchUpdateForBlogCategory(blogCategory);
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}