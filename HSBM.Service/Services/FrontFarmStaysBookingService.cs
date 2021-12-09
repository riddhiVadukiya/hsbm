using HSBM.EntityModel.Front;
using HSBM.EntityModel.FrontEnd;
using HSBM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSBM.EntityModel.AxisRooms;

namespace HSBM.Service.Services
{
    public class FrontFarmStaysBookingService
    {
        #region Init
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
        FrontFarmStaysBookingRepository _FrontFarmStaysBookingRepository = new FrontFarmStaysBookingRepository();

        /// <summary>
        /// Get Farmstay Room Detail by FarmStayId
        /// </summary>
        /// <param name="_SearchFarmStays"></param>
        /// <returns></returns>
        public BookingResponse GetRoomBookingDetail(SearchFarmStaysRequest _SearchFarmStays)
        {
            BookingResponse _RoomDetails = new BookingResponse();
            try
            {
                _RoomDetails = _FrontFarmStaysBookingRepository.GetRoomBookingDetail(_SearchFarmStays);
            }
            catch (Exception ex)
            {
                _ILogger.Error("GetBookedRoom: Error :- ", ex);
            }
            return _RoomDetails;
        }

        public int BookFarmStayRoom(BookingResponse BookingResponse)
        {
            FarmStaysBookingDetail _RoomDetails = new FarmStaysBookingDetail();
            try
            {
               return _FrontFarmStaysBookingRepository.BookFarmStayRoom(BookingResponse);
            }
            catch (Exception ex)
            {
                _ILogger.Error("GetBookedRoom: Error :- ", ex);

                return 0;
            }
        }

        public bool ChangeOrderStatus(int Id, int Status, decimal RefundAmount=0, string CancellationReason="")
        {
            try
            {
                return _FrontFarmStaysBookingRepository.ChangeOrderStatus(Id, Status, RefundAmount, CancellationReason);
            }
            catch (Exception ex)
            {
                _ILogger.Error("ChangeOrderStatus: Error :- ", ex);

                return false;
            }
        }

        public bool AddPaymentHistory(int Id, int Status,string PayuMoneyId, string PaymentResponse)
        {
            try
            {
                return _FrontFarmStaysBookingRepository.AddPaymentHistory(Id, Status,PayuMoneyId, PaymentResponse);
            }
            catch (Exception ex)
            {
                _ILogger.Error("ChangeOrderStatus: Error :- ", ex);

                return false;
            }
        }

        public string GetPaymentIdbyOrderId(int Id)
        {
            try
            {
                return _FrontFarmStaysBookingRepository.GetPaymentIdbyOrderId(Id);
            }
            catch (Exception ex)
            {
                _ILogger.Error("ChangeOrderStatus: Error :- ", ex);

                return "";
            }
        }
        public bool AxisBokingAPIIntegration(List<AxisBookingResponse> _AxisBookingList)
        {
            try
            {
                return _FrontFarmStaysBookingRepository.AxisBookingAPIIntegration(_AxisBookingList);
            }
            catch (Exception ex)
            {
                _ILogger.Error("ChangeOrderStatus: Error :- ", ex);

                return false;
            }
        }
    }
}
