using HSBM.EntityModel.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Front
{
    public class FarmStaysBlogResponse
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public string Categories { get; set; }
        public string Image { get; set; }
        public bool IsPopulerPost { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? UpdatedBy { get; set; }
        public string Author { get; set; }
        public List<BlogComment> listBlogComment { get; set; }
        public string Search { get; set; }
    }
}
