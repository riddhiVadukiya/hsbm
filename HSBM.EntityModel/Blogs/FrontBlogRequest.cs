using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Blogs
{
    public class FrontBlogRequest
    {
        public int PageIndex { get; set; }
        public string keyword { get; set; }
        public long CategoryId { get; set; }
        public string ArchivesData { get; set; }
    }
}
