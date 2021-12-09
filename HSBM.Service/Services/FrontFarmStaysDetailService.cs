using HSBM.EntityModel.Front;
using HSBM.EntityModel.FrontEnd;
using HSBM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class FrontFarmStaysDetailService
    {
        #region Init
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
        FrontFarmStaysDetailRepository _FrontFarmStaysDetailRepository = new FrontFarmStaysDetailRepository();

        /// <summary>
        /// Get Farmstay Detail by FarmStayId
        /// </summary>
        /// <param name="_SearchFarmStays"></param>
        /// <returns></returns>
        public FarmStaysDetail GetFarmStayDetailByFarmStayId(SearchFarmStaysRequest _SearchFarmStays)
        {
            FarmStaysDetail _FarmStaysDetail = new FarmStaysDetail();
            try
            {
                _FarmStaysDetail = _FrontFarmStaysDetailRepository.GetFarmStayDetailByFarmStayId(_SearchFarmStays);
            }
            catch (Exception ex)
            {
                _ILogger.Error("GetFarmStayDetailByFarmStayId: Error :- ", ex);
            }
            return _FarmStaysDetail;
        }

        /// <summary>
        /// Get Farmstay Room Detail by FarmStayId
        /// </summary>
        /// <param name="_SearchFarmStays"></param>
        /// <returns></returns>
        public RoomDetails GetAvailableRoom(SearchFarmStaysRequest _SearchFarmStays)
        {
            RoomDetails _RoomDetails = new RoomDetails();
            try
            {
                _RoomDetails = _FrontFarmStaysDetailRepository.GetAvailableRoom(_SearchFarmStays);
            }
            catch (Exception ex)
            {
                _ILogger.Error("GetAvailableRoom: Error :- ", ex);
            }
            return _RoomDetails;
        }
    }
}
