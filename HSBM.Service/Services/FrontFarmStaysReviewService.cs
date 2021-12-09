using HSBM.EntityModel.FrontReview;
using HSBM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class FrontFarmStaysReviewService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public FarmStaysRatingsAndReviewResponse GetReviewDetailByOrderNo(string OrderNo)
        {
            FrontFarmStaysReviewRepository _FrontFarmStaysReviewRepository = new FrontFarmStaysReviewRepository();

            FarmStaysRatingsAndReviewResponse obj = new FarmStaysRatingsAndReviewResponse();
            try
            {
                obj = _FrontFarmStaysReviewRepository.GetReviewDetailByOrderNo(OrderNo);
                if (obj != null)
                {                    
                    return obj;
                }
                else
                {                    
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FrontFarmStaysReviewService=>GetReviewDetailByGuid: Error :- ", ex);               
                return null;
            }
        }
        public int AddRateAndReviews(FarmStaysRatingsAndReviewRequest p_FarmStaysRatingsAndReviewRequest)
        {
            FrontFarmStaysReviewRepository _FrontFarmStaysReviewRepository = new FrontFarmStaysReviewRepository();            
            try
            {
                int count = _FrontFarmStaysReviewRepository.AddRateAndReviews(p_FarmStaysRatingsAndReviewRequest);
                if (count > 0)
                {                    
                    return 1;
                }
                else
                {                    
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FrontFarmStaysReviewService=>AddRateAndReviews: Error :- ", ex);               
                return 0;
            }
        }

        public string GetFarmStayNameById(long p_farmstayId)
        {
            FrontFarmStaysReviewRepository _FrontFarmStaysReviewRepository = new FrontFarmStaysReviewRepository();
            try
            {
                string FarmstayName = _FrontFarmStaysReviewRepository.GetFarmStayNameById(p_farmstayId);
                if (!string.IsNullOrEmpty(FarmstayName))
                {
                    return FarmstayName;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FrontFarmStaysReviewService=>GetFarmStayNameById: Error :- ", ex);
                return string.Empty;
            }
        }
    }
}
