using HSBM.EntityModel.Common;
using HSBM.EntityModel.Front;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class FrontFarmStaysHomeService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GridDataResponse GetAllBannerBySearchRequest(RequestResponseServiceContext requestResponseServiceContext)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();

            try
            {
                FrontFarmStaysHomeRepository _FrontFarmStaysHomeRepository = new FrontFarmStaysHomeRepository();
                var _FarmStaysHome = _FrontFarmStaysHomeRepository.GetAllBannerMasterBySearchRequest();

                if (_FarmStaysHome != null && _FarmStaysHome.Count > 0)
                {                    
                    _GridDataResponse.data = _FarmStaysHome;
                    //requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _GridDataResponse;
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
                _ILogger.Error("Front Farmstay Home =>GetAllBannerBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public List<FarmStaysPopularFarmstayResponse> GetAllPopularFarmStay(RequestResponseServiceContext requestResponseServiceContext)
        {
            List<FarmStaysPopularFarmstayResponse> _ListFarmStaysPopularFarmstayResponse = new List<FarmStaysPopularFarmstayResponse>();
            try
            {
                FrontFarmStaysHomeRepository _FrontFarmStaysHomeRepository = new FrontFarmStaysHomeRepository();
                var _FarmStaysHome = _FrontFarmStaysHomeRepository.GetAllPopularFarmStay();

                if (_FarmStaysHome != null && _FarmStaysHome.Count > 0)
                {
                    _ListFarmStaysPopularFarmstayResponse = _FarmStaysHome;
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _ListFarmStaysPopularFarmstayResponse;
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
                _ILogger.Error("Front Farmstay Home =>GetAllBannerBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }
        public List<FarmStaysDiscountResponse> GetAllFarmStaysDiscount(RequestResponseServiceContext requestResponseServiceContext)
        {
            List<FarmStaysDiscountResponse> _ListFarmStaysDiscountResponse = new List<FarmStaysDiscountResponse>();
            try
            {
                FrontFarmStaysHomeRepository _FrontFarmStaysHomeRepository = new FrontFarmStaysHomeRepository();
                var _FarmStaysHome = _FrontFarmStaysHomeRepository.GetAllDiscountOnFarmstays();

                if (_FarmStaysHome != null && _FarmStaysHome.Count > 0)
                {
                    _ListFarmStaysDiscountResponse = _FarmStaysHome;
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _ListFarmStaysDiscountResponse;
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
                _ILogger.Error("Front Farmstay Home =>GetAllBannerBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }
    }
}
