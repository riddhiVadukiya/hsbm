using HSBM.EntityModel.Front;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.Data;
using HSBM.EntityModel.FrontEnd;
using HSBM.Common.Utils;
using HSBM.EntityModel.FrontReview;
using HSBM.EntityModel.AxisRooms;

namespace HSBM.Repository.Repositories
{
    public class FrontFarmStaysDetailRepository
    {


        public FarmStaysDetail GetFarmStayDetailByFarmStayId(SearchFarmStaysRequest _SearchFarmStays)
        {
            FarmStaysDetail _FarmStaysDetail = new FarmStaysDetail();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"select Name,Address,Description,CheckInTime,CheckOutTime,Bedrooms,Latitude,Longitude,Bathrooms,USPTags,Persuasions,LanguagesSpoken,TypeofFood,Location,
                                    HouseRules,StayPolicy from FarmStays FS where FS.IsActive=1 and  FS.Id =" + _SearchFarmStays.FarmStayId;

                    _FarmStaysDetail = _DbManager.SetCommand(_SqlQuery).ExecuteObject<FarmStaysDetail>();

                    if (_FarmStaysDetail != null)
                    {

                        string _SqlAmenityQuery = @" select AM.AmenityName from FarmStaysAmenities  inner join AmenityMaster AM on AM.Id = AmenityId where FarmStaysId=" + _SearchFarmStays.FarmStayId;

                        _FarmStaysDetail.FarmStaysAmenities = _DbManager.SetCommand(_SqlAmenityQuery).ExecuteScalarList<string>();

                        string _SqlImageQuery = @"select ImageName as ImageURL from FarmStaysImages where FarmStaysId=" + _SearchFarmStays.FarmStayId;

                        _FarmStaysDetail.FarmStaysImages = _DbManager.SetCommand(_SqlImageQuery).ExecuteList<ImageDto>();

                        string _SqlRatingAndReview = @"select rr.Id,rr.FarmStyasId,rr.Customerid,rr.Ratings,rr.Reviews,rr.CreatedDate,rr.OrderNo,rr.FarmStaysRatingsAndReviewGUID,su.FirstName, su.LastName from FarmStaysRatingsAndReview rr
                                                        inner join SystemUsers su on rr.Customerid = su.Id
                                                        where IsApproved = 1 and FarmStyasId = " + _SearchFarmStays.FarmStayId;
                        _FarmStaysDetail.RatingAndReview = _DbManager.SetCommand(_SqlRatingAndReview).ExecuteList<FarmStaysRatingsAndReviewResponse>();


                    }
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _FarmStaysDetail;
        }

        public RoomDetails GetAvailableRoom(SearchFarmStaysRequest _SearchFarmStays)
        {
            RoomDetails _RoomDetails = new RoomDetails();

            try
            {
                int _Days = (Convert.ToDateTime(_SearchFarmStays.CheckOut) - Convert.ToDateTime(_SearchFarmStays.CheckIn)).Days;
                using (DbManager _DbManager = new DbManager())
                {



                    string _SqlAvailabilityQuery = @"select RoomId,Name,Type,dbo.CurrecncyConversion(@CurrencyCode,Price) as Price,dbo.CurrecncyConversion(@CurrencyCode,DiscountPrice) as DiscountPrice,
                                                        AvailableFor,IsShared,RatePlanId,MaxPerson from GetRoomRatePlanAvailabilityAndPrice(@FarmStayId,@CheckIn,@CheckOut,@Guests,@IsSolo,@Child) order by RoomId,price ";

                    _RoomDetails.ListofRoom = _DbManager.SetCommand(_SqlAvailabilityQuery,
                                                             _DbManager.Parameter("@FarmStayId", _SearchFarmStays.FarmStayId),
                                                             _DbManager.Parameter("@CheckIn", Convert.ToDateTime(_SearchFarmStays.CheckIn)),
                                                             _DbManager.Parameter("@CheckOut", Convert.ToDateTime(_SearchFarmStays.CheckOut)),
                                                             _DbManager.Parameter("@Guests", _SearchFarmStays.Guests),
                                                             _DbManager.Parameter("@IsSolo", _SearchFarmStays.IsSolo),
                                                             _DbManager.Parameter("@CurrencyCode", _SearchFarmStays.CurrencyCode),
                                                             _DbManager.Parameter("@Child", _SearchFarmStays.Child)
                                                             ).ExecuteList<RoomDetail>();
                    if (_RoomDetails.ListofRoom != null && _RoomDetails.ListofRoom.Count() > 0)
                    {
                        _RoomDetails.ListofRoom.ForEach(x =>
                                                {
                                                    x.Days = _Days;
                                                    x.BookingURL = "FarmStayId=" + _SearchFarmStays.FarmStayId + "&RoomId=" + x.RoomId + "&CheckIn=" + _SearchFarmStays.CheckIn + "&CheckOut=" + _SearchFarmStays.CheckOut + "&Guests=" + _SearchFarmStays.Guests + "&Date=" + DateTime.Now.ToString() + "&Child=" + _SearchFarmStays.Child + "&RatePlanId=" + x.RatePlanId;
                                                    x.NoOfAdults = _SearchFarmStays.Guests;
                                                    x.NoOfChild = _SearchFarmStays.Child;
                                                });

                        bool AvailableFor = _RoomDetails.ListofRoom.Where(t => t.IsShared).Any();

                        _RoomDetails.ListofRoom.ForEach(x =>
                        {
                            x.Days = _Days;
                            if (AvailableFor)
                            {
                                x.BookingURL = Helper.Encrypt(x.BookingURL + "&IsSolo=" + (x.IsShared ? true : false));
                            }
                            else
                            {
                                x.BookingURL = Helper.Encrypt(x.BookingURL + "&IsSolo=" + _SearchFarmStays.IsSolo);
                            }

                        });


                        // _RoomDetails.ListofRoom.ForEach(x => );
                        string _SqlDiscountQuery = @"select Name,IsPercentage,IsEBO,
                                        case when IsPercentage = 1 then DiscountValue else dbo.CurrecncyConversion(@CurrencyCode,DiscountValue) end as DiscountValue
                                        from [GetAppliedDiscountOnFarmstays](@FarmStayId,@CheckIn,@CheckOut,null,0)";

                        List<DiscountDto> _FarmStaysDiscount = _DbManager.SetCommand(_SqlDiscountQuery,
                                                             _DbManager.Parameter("@FarmStayId", _SearchFarmStays.FarmStayId),
                                                             _DbManager.Parameter("@CheckIn", Convert.ToDateTime(_SearchFarmStays.CheckIn)),
                                                             _DbManager.Parameter("@CheckOut", Convert.ToDateTime(_SearchFarmStays.CheckOut).AddDays(-1)),
                                                             _DbManager.Parameter("@CurrencyCode", _SearchFarmStays.CurrencyCode)).ExecuteList<DiscountDto>();

                        if (_FarmStaysDiscount != null)
                        {
                            _RoomDetails.FarmStayDiscount = string.Empty;
                            foreach (var item in _FarmStaysDiscount)
                            {
                                _RoomDetails.FarmStayDiscount = _RoomDetails.FarmStayDiscount + (_RoomDetails.FarmStayDiscount != string.Empty ? ", " : "");
                                if (item.IsEBO)
                                {
                                    _RoomDetails.FarmStayDiscount = _RoomDetails.FarmStayDiscount + "Early Bird Discount Applied " + (item.IsPercentage ? item.DiscountValue + "%" : "₹" + item.DiscountValue);
                                }
                                else
                                {
                                    _RoomDetails.FarmStayDiscount = _RoomDetails.FarmStayDiscount + item.Name + " " + (item.IsPercentage ? item.DiscountValue + "%" : Helper.GetCurrentCurrencySymbol() +" "+ item.DiscountValue) + " off";
                                }
                            }
                        }
                        
                    }

                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _RoomDetails;
        }
    }
}
