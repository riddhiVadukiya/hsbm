using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.FarmStays;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service
{
    public class FarmStaysRatingsAndReviewService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        FarmStaysRatingsAndReviewRepository _FarmStaysRatingsAndReviewRepository = new FarmStaysRatingsAndReviewRepository();

        public GridDataResponse GetAllFarmStaysRatingsAndReviewBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, FarmStaysRatingsAndReviewRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
           
            try
            {
                _GridDataResponse = _FarmStaysRatingsAndReviewRepository.GetAllFarmStaysRatingsAndReviewBySearchRequest(p_GridParams, p_SearchRequest);
                if (_GridDataResponse.data != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
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
                _ILogger.Error("FarmStaysService=>GetAllFarmStayssBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public FarmStaysRatingsAndReviewResponse GetRatingsAndReviewDetailByKey(long id)
        {
            FarmStaysRatingsAndReviewResponse _FarmStaysRatingsAndReview = new FarmStaysRatingsAndReviewResponse();

            _FarmStaysRatingsAndReview = _FarmStaysRatingsAndReviewRepository.GetRatingsAndReviewDetailByKey(id);

            if (_FarmStaysRatingsAndReview != null)
            {
                return _FarmStaysRatingsAndReview;
            }
            return null;
        }
        public void AprroveAndUnapproveRatingsAndReview(RequestResponseServiceContext requestResponseServiceContext, int Id, long ApprovedById)
        {
            AmenityMasterRepository _AmenityMasterRepository = new AmenityMasterRepository();
            try
            {
                if (_FarmStaysRatingsAndReviewRepository.AprroveAndUnapproveRatingsAndReview(Id, ApprovedById))
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                else
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.BAD_REQUEST;
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysRatingsAndReviewService=>AprroveAndUnapproveRatingsAndReview: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }


    }
}
