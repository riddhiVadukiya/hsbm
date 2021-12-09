using HSBM.EntityModel.Common;
using HSBM.EntityModel.HomeMaster;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class HomeService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get New registerd customer
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="p_GridParams"></param>
        /// <param name="p_Request"></param>
        /// <returns></returns>
        public List<NewCustomersResponse> GetNewCustomersList(RequestResponseServiceContext requestResponseServiceContext)
        {

            HomeRepository _HomeRepository = new HomeRepository();
            List<NewCustomersResponse> _ListNewCustomersResponse = new List<NewCustomersResponse>();
            try
            {
                _ListNewCustomersResponse = _HomeRepository.GetNewCustomersList();
                if (_ListNewCustomersResponse != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _ListNewCustomersResponse;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "No Record Found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> GetNewCustomersList : Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }


        public List<RecentOrder> RecentOrders(RequestResponseServiceContext requestResponseServiceContext)
        {

            HomeRepository _HomeRepository = new HomeRepository();
            List<RecentOrder> _ListRecentOrder = new List<RecentOrder>();
            try
            {
                _ListRecentOrder = _HomeRepository.RecentOrders();
                if (_ListRecentOrder != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _ListRecentOrder;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "No Record Found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> RecentOrders : Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public List<HighestBookedFarmStay> BindHighestBooked(RequestResponseServiceContext requestResponseServiceContext)
        {

            HomeRepository _HomeRepository = new HomeRepository();
            List<HighestBookedFarmStay> _List = new List<HighestBookedFarmStay>();
            try
            {
                _List = _HomeRepository.HighestBooked();
                if (_List != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _List;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "No Record Found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> BindHighestBooked : Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public List<FarmStaySummary> BindFarmStaySummary(RequestResponseServiceContext requestResponseServiceContext, int FarmstayId)
        {

            HomeRepository _HomeRepository = new HomeRepository();
            List<FarmStaySummary> _List = new List<FarmStaySummary>();
            try
            {
                _List = _HomeRepository.FarmStaySummary(FarmstayId);
                if (_List != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _List;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "No Record Found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> BindFarmStaySummary : Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }
        public List<TotalBooking> BindTotalBooking(RequestResponseServiceContext requestResponseServiceContext, int Frequency, DateTime? FromDate, DateTime? ToDate)
        {

            HomeRepository _HomeRepository = new HomeRepository();
            List<TotalBooking> _List = new List<TotalBooking>();
            try
            {
                _List = _HomeRepository.BindTotalBooking(Frequency, FromDate, ToDate);
                if (_List != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _List;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "No Record Found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> BindTotalBooking : Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }
        public List<TotalEarnings> BindTotalEarning(RequestResponseServiceContext requestResponseServiceContext, int Frequency, DateTime? FromDate , DateTime? ToDate)
        {

            HomeRepository _HomeRepository = new HomeRepository();
            List<TotalEarnings> _List = new List<TotalEarnings>();
            try
            {
                _List = _HomeRepository.BindTotalEarning(Frequency, FromDate, ToDate);
                if (_List != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _List;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "No Record Found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> BindTotalEarning : Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public GeneralCalculation BindGeneralCalculation(RequestResponseServiceContext requestResponseServiceContext)
        {

            HomeRepository _HomeRepository = new HomeRepository();
            GeneralCalculation _List = new GeneralCalculation();
            try
            {
                _List = _HomeRepository.GeneralCalculation();
                if (_List != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _List;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "No Record Found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> BindTotalBooking : Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }


        public GridDataResponse ExportNewCustomersList(RequestResponseServiceContext requestResponseServiceContext)
        {

            HomeRepository _HomeRepository = new HomeRepository();
            GridDataResponse _GridDataResponse = new GridDataResponse();
            List<NewCustomersResponse> _ListNewCustomersResponse = new List<NewCustomersResponse>();
            try
            {
                _GridDataResponse = _HomeRepository.ExportGetNewCustomersList();

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
                _ILogger.Error("Home=> GetNewCustomersList : Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }
    }
}
