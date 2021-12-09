using HSBM.Common.Enums;
using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Helpers
{
    public class Common
    {
        public static List<SelectListItem> GetEmailTemplateTypeForDropDown()
        {

            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(EmailTemplateType)))
            {
                lst.Add(new SelectListItem() { Text = Helper.GetEnumDescription((EmailTemplateType)(int)item), Value = Convert.ToString((int)item) });
            }

            return lst;
        }        
    }
}