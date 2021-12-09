using BLToolkit.Mapping;
using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Blogs
{
    public class BlogComment
    {
        public long Id { get; set; }
        public long BlogId { get; set; }
        public long UserId { get; set; }
        [DisplayName("Blog Comment")]
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        [DisplayName("Comment Date")]
        public DateTime CreatedDate { get; set; }
        
        [MapIgnore]
        [DisplayName("Comment Date")]
        public string strCreatedDate
        {
            get
            {
                try
                { return CreatedDate.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture); }
                catch (Exception)
                { return ""; }
            }
        }

        public string CommentBy { get; set; }
        public int RecordsTotal { get; set; }
        [DisplayName("Blog Title")]
        public string Title { get; set; }
    }
}
