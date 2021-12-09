namespace HSBM.EntityModel.Blogs
{
    using System;
    using System.ComponentModel;
    using BLToolkit.DataAccess;
    using BLToolkit.Mapping;
    using BLToolkit.Validation;
    using System.Collections.Generic;

    public class BlogsRequest
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public bool IncludeIsDeleted { get; set; }

        public bool Popular { get; set; }
    }
}