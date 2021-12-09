using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSBM.Web.Helpers
{
    public class UserAccess
    {
        public bool CanView {get; set;}
        public bool CanAdd {get; set;}
        public bool CanUpdate {get; set;}
        public bool CanDelete { get; set; }
    }
}