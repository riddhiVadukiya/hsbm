using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Blogs
{
    public class BlogCategory
    {
        public long Id { get; set; }
        [Required]
        public string Category { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long UpdatedBy { get; set; }

        public int RecordsTotal { get; set; }
    }
}
