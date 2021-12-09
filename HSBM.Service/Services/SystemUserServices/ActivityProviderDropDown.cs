using HSBM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HSBM.Service.Services.SystemUserServices
{
    public class ActivityProviderDropDown
    {
        #region Get Activity Provider DropDown                
        /// <summary>
        /// Get Activity Provider DropDown
        /// </summary>
        /// <param name="p_Id">p_Id</param>
        /// <returns> List of SelectListItem </returns>
        public List<SelectListItem> ActivityProviderDropDownList(long p_Id)
        {
            SystemUsersRepository _SystemUsersRepository = new SystemUsersRepository();
            List<SelectListItem> _ListOfSelectListItem = new List<SelectListItem>();
            try
            {
                _SystemUsersRepository.ActivityProviderDropDown(p_Id);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfSelectListItem;
        }
        #endregion
    }
}
