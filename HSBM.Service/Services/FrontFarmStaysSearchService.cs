using HSBM.EntityModel.Front;
using HSBM.EntityModel.FrontEnd;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HSBM.Service.Services
{
    public class FrontFarmStaysSearchService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SearchFarmStaysResult GetFarmStayslist(RequestResponseServiceContext requestResponseServiceContext, SearchFarmStaysRequest req)
        {
            FrontFarmStaysSearchRepository _FrontFarmStaysSearchRepository = new FrontFarmStaysSearchRepository();

            SearchFarmStaysResult obj = new SearchFarmStaysResult();
            try
            {
                obj = _FrontFarmStaysSearchRepository.GetFarmStayslist(req);
                if (obj.SearchFarmStays.Any())
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return obj;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not Found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FrontFarmStaysSearchService=>GetFarmStayslist: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public List<SelectListItem> GetFarmStaysForDropDown()
        {
            List<SelectListItem> _ListFarmStays = new List<SelectListItem>();
            FrontFarmStaysSearchRepository _FarmStaysRepository = new FrontFarmStaysSearchRepository();
            //FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            try
            {
                _ListFarmStays = _FarmStaysRepository.GetFarmStaysForDropDown();
                if (_ListFarmStays != null)
                {
                    return _ListFarmStays;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FrontFarmStaysSearchService=>GetFarmStaysForDropDown: Error :- ", ex);
                return null;
            }
        }
    }
}
