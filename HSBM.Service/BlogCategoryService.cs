using HSBM.Common.Utils;
using HSBM.EntityModel.Blogs;
using HSBM.EntityModel.Common;
using HSBM.Repository.Repositories;
using System.Linq;

namespace HSBM.Service
{
    public class BlogCategoryService
    {
        public void ActiveAndInactiveSwitchUpdateForBlogCategory(EntityModel.Blogs.BlogCategory blogCategory)
        {
            BlogCategoryRepository _BlogCategoryRepository = new BlogCategoryRepository();

            _BlogCategoryRepository.ActiveAndInactiveSwitchUpdateForBlogCategory(blogCategory);

        }

        public int AddorUpdateBlogCategory(EntityModel.Blogs.BlogCategory blogCategory)
        {
            BlogCategoryRepository _BlogCategoryRepository = new BlogCategoryRepository();

            if (blogCategory.Id > 0)
            {
                return _BlogCategoryRepository.AddorUpdateBlogCategory(blogCategory);
            }
            else
            {
                return _BlogCategoryRepository.AddorUpdateBlogCategory(blogCategory);
            }
        }

        public void DeleteBlogCategoryById(long Id)
        {
            BlogCategoryRepository _BlogCategoryRepository = new BlogCategoryRepository();

            _BlogCategoryRepository.DeleteBlogCategoryById(Id);
        }

        public GridDataResponse GetAllBlogCategoryBySearchRequest(GridParams p_GridParams)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();

            BlogCategoryRepository _BlogCategoryRepository = new BlogCategoryRepository();

            var _Country = _BlogCategoryRepository.GetAllBlogCategoryBySearchRequest(p_GridParams);

            if (_Country != null && _Country.Count > 0)
            {
                _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
                _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
            }
            _GridDataResponse.data = _Country;

            return _GridDataResponse;
        }

        public BlogCategory GetBlogCategoryById(long Id)
        {
            BlogCategory _BlogCategoryModel = new BlogCategory();

            BlogCategoryRepository _BlogCategoryRepository = new BlogCategoryRepository();

            _BlogCategoryModel = _BlogCategoryRepository.GetBlogCategoryById(Id);

            return _BlogCategoryModel;
        }
    }
}
