using HSBM.Common.Utils;
using HSBM.EntityModel.RoleMaster;
using HSBM.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HSBM.Web.Areas.Admin.Controllers
{
    public class CommonController : BaseController
    {
        public JsonResult UpdateRole(string roledata)
        {
            try
            {
                var _currentUserData = SessionProxy.UserDetails;
                var _RoleMaster = JsonConvert.DeserializeObject<RoleMaster>(Helper.Decrypt(roledata));
                if (_RoleMaster != null && _RoleMaster.Id == _currentUserData.RoleMasterID)
                {
                    _currentUserData.RoleMasterDetails = _RoleMaster.RoleMasterDetails.Where(t => t.CanView).ToList(); // || t.CanUpdate || t.CanDelete || t.CanAdd).ToList();

                    SessionProxy.UserDetails = _currentUserData;
                    return Json("Success", JsonRequestBehavior.AllowGet);
                    //return Json(new { name = SessionProxy.USERDETAILS, data = Helper.Encrypt(JsonConvert.SerializeObject(_currentUserData)) }
                    //    , JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
            }
            return Json(null, JsonRequestBehavior.AllowGet);

        }

    }
}