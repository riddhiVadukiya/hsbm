using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.DiscountMaster;
using HSBM.EntityModel.FarmStays;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class FarmStaysService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get Farm Stays by search request
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="p_GridParams"></param>
        /// <param name="p_Request"></param>
        /// <returns></returns>
        public GridDataResponse GetAllFarmStaysBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, FarmStaysRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            try
            {
                _GridDataResponse = _FarmStaysRepository.GetAllFarmStaysBySearchRequest(p_GridParams, p_SearchRequest);
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

        public int AddOrUpdateFarmStayBasicDetail(RequestResponseServiceContext requestResponseServiceContext, FarmStaysBasicDetail FarmStaysBasicDetail)
        {

            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            int result = 0;

            try
            {
                result = _FarmStaysRepository.AddOrUpdateFarmStayBasicDetail(FarmStaysBasicDetail);
               
                if (result > 0)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    if (FarmStaysBasicDetail.Id > 0)
                        requestResponseServiceContext.Response.StatusParameters = new string[] { "FarmStay has been updated successfully." };
                    else
                        requestResponseServiceContext.Response.StatusParameters = new string[] { "FarmStay has been added successfully." };
                }
                else if (result == -1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "FarmStay already exist." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>AddOrUpdateFarmStays: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }

        /// <summary>
        /// Active or inactive amenity master
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="amenity"></param>
        public void ActiveAndInactiveFarmStay(RequestResponseServiceContext requestResponseServiceContext, FarmStays amenity)
        {
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            try
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                _FarmStaysRepository.ActiveAndInactiveFarmStay(amenity);
            }
            catch (Exception ex)
            {
                _ILogger.Error("AmenityMasterService=>ActiveAndInactiveAmenityMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Get amenity master by id
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public FarmStays GetFarmStayBasicDetailById(RequestResponseServiceContext requestResponseServiceContext, int Id)
        {
            FarmStays _FarmStays = new FarmStays();
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            try
            {
                _FarmStays = _FarmStaysRepository.GetFarmStayBasicDetailById(Id);
                if (_FarmStays != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _FarmStays;
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
                _ILogger.Error("FarmStaysService=>GetFarmStayBasicDetailById: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }

        }

        public int SaveFarmStayAmenities(RequestResponseServiceContext requestResponseServiceContext, List<FarmStaysAmenities> Amenities)
        {

            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            int result = 0;

            try
            {
                result = _FarmStaysRepository.SaveFarmStayAmenities(Amenities);
                if (result == 1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "FarmStay has been updated successfully." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>AddOrUpdateFarmStaysAmenity: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }

        public List<FarmStaysImages> SaveFarmStayImages(RequestResponseServiceContext requestResponseServiceContext, List<FarmStaysImages> FarmStaysImages)
        {
            List<FarmStaysImages> _ListofFarmStaysImages = new List<FarmStaysImages>();
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();

            try
            {
                _ListofFarmStaysImages = _FarmStaysRepository.SaveFarmStayImages(FarmStaysImages);
                if (_ListofFarmStaysImages != null && _ListofFarmStaysImages.Count() > 0)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Image has been added/deleted successfully." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>AddOrUpdateFarmStaysImages: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return _ListofFarmStaysImages;
        }

        public int SaveFarmStayPolicy(RequestResponseServiceContext requestResponseServiceContext, FarmStaysPolicyDetail FarmStaysPolicyDetail)
        {

            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            int result = 0;

            try
            {
                result = _FarmStaysRepository.SaveFarmStayPolicy(FarmStaysPolicyDetail);
                if (result == 1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "FarmStay policy has been updated successfully." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>AddOrUpdateFarmStaysPolicy: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }

        #region Room
        public int SaveFarmStayRoom(RequestResponseServiceContext requestResponseServiceContext, FarmStaysRooms FarmStaysRooms)
        {
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            int result = 0;
            try
            {

                result = _FarmStaysRepository.SaveFarmStayRoom(FarmStaysRooms);
                if (result > 0)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    if (FarmStaysRooms.Id > 0)
                        requestResponseServiceContext.Response.StatusParameters = new string[] { "Room has been updated successfully." };
                    else
                        requestResponseServiceContext.Response.StatusParameters = new string[] { "Room has been added successfully." };
                }
                else if (result == -1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Room already exist." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>AddOrUpdateFarmStaysRoom: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }

        public List<FarmStaysRooms> GetRoomByFarmStayId(RequestResponseServiceContext requestResponseServiceContext, int Id)
        {
            List<FarmStaysRooms> _FarmStays = new List<FarmStaysRooms>();
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            try
            {
                _FarmStays = _FarmStaysRepository.GetRoomByFarmStayId(Id);
               
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _FarmStays;
                
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>GetRoomByFarmStayId: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
            return _FarmStays;
        }

        public int DeleteFarmStayRoom(RequestResponseServiceContext requestResponseServiceContext, int Id)
        {
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            int result = 0;
            try
            {

                result = _FarmStaysRepository.DeleteFarmStayRoom(Id);
                if (result > 0)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Room has been deleted successfully." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>AddOrUpdateFarmStaysImages: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }
        #endregion

        #region Season
        public int SaveFarmStaySeason(RequestResponseServiceContext requestResponseServiceContext, FarmStaysSeasons FarmStaysSeasons)
        {
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            int result = 0;
            try
            {

                result = _FarmStaysRepository.SaveFarmStaySeason(FarmStaysSeasons);
                if (result == 1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    if (FarmStaysSeasons.GroupId != Guid.Empty)
                        requestResponseServiceContext.Response.StatusParameters = new string[] { "Season has been updated successfully." };
                    else
                        requestResponseServiceContext.Response.StatusParameters = new string[] { "Season has been added successfully." };
                }
                else if (result == 2)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Season already exist." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>AddOrUpdateFarmStaysSeason: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }

        public List<FarmStaysSeasons> GetSeasonByRoomId(RequestResponseServiceContext requestResponseServiceContext, int Id)
        {
            List<FarmStaysSeasons> _FarmStays = new List<FarmStaysSeasons>();
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            try
            {
                _FarmStays = _FarmStaysRepository.GetSeasonByRoomId(Id);

                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                return _FarmStays;


            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>GetSeasonByFarmStayId: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public int DeleteFarmStaySeason(RequestResponseServiceContext requestResponseServiceContext, Guid GroupId)
        {
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            int result = 0;
            try
            {

                result = _FarmStaysRepository.DeleteFarmStaySeason(GroupId);
                if (result > 0)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Season has been deleted successfully." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>AddOrUpdateFarmStaysImages: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }
        #endregion

        public List<DiscountMaster> GetDiscountByFarmStayId(RequestResponseServiceContext requestResponseServiceContext, int Id)
        {
            List<DiscountMaster> _FarmStays = new List<DiscountMaster>();
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            try
            {
                _FarmStays = _FarmStaysRepository.GetDiscountByFarmStayId(Id);
                   requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _FarmStays;
              
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>GetSeasonByFarmStayId: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
            return _FarmStays;
        }

        public List<FarmStays> GetFarmStaysForDropDown()
        {
            List<FarmStays> _ListFarmStays = new List<FarmStays>();
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
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
                _ILogger.Error("FarmStaysService=>GetFarmStaysForDropDown: Error :- ", ex);                
                return null;
            }
        }

        public SeasonListResponse GetSeasonByBookingDateRoomId(RequestResponseServiceContext requestResponseServiceContext, SeasonRequest p_SeasonRequest)
        {
            SeasonListResponse _SeasonListResponse = new SeasonListResponse();
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            try
            {
                _SeasonListResponse = _FarmStaysRepository.GetSeasonByBookingDateRoomId(p_SeasonRequest);

                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                return _SeasonListResponse;


            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>GetSeasonByBookingDateRoomId: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        public int UpdateSeason(RequestResponseServiceContext requestResponseServiceContext, SeasonListResponse p_SeasonListResponse)
        {
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            int result = 0;
            try
            {

                result = _FarmStaysRepository.UpdateSeason(p_SeasonListResponse);
                if (result == 1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                        requestResponseServiceContext.Response.StatusParameters = new string[] { "Season has been updated successfully along with Axis Rooms." };
                }
                else if (result == 3)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Error in updating season in Axis Rooms." };
                }
                else if (result == 2)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Error in updating season." };
                }
                else if (result == 4)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Season has been updated successfully." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>UpdateSeason: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }

        public int AddSeason(RequestResponseServiceContext requestResponseServiceContext, AddSeasonResponse p_AddSeasonResponse)
        {
            FarmStaysRepository _FarmStaysRepository = new FarmStaysRepository();
            int result = 0;
            try
            {

                result = _FarmStaysRepository.AddSeason(p_AddSeasonResponse);
                if (result == 1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Season has been added successfully along with Axis Rooms." };
                }
                else if (result == 3)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Error in adding season in Axis Rooms." };
                }
                else if (result == 2)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Error in adding season." };
                }
                else if (result == 4)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Season has been added successfully." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("FarmStaysService=>AddSeason: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }
    }
}
