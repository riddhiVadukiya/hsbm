using BLToolkit.Data;
using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Front;
using HSBM.EntityModel.FrontEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSBM.EntityModel.AxisRooms;
using System.Net.Http;
using System.Web.Script.Serialization;


namespace HSBM.Repository.Repositories
{
    public class FrontFarmStaysBookingRepository
    {
        public BookingResponse GetRoomBookingDetail(SearchFarmStaysRequest _SearchFarmStays)
        {
            BookingResponse _FarmStaysBookingDetail = new BookingResponse();

            try
            {
                int _Days = (Convert.ToDateTime(_SearchFarmStays.CheckOut) - Convert.ToDateTime(_SearchFarmStays.CheckIn)).Days;
                using (DbManager _DbManager = new DbManager())
                {



                    string _SqlAvailabilityQuery = @"select * from GetBookedRoomDetail(@FarmStayId,@RoomId,@CheckIn,@CheckOut,@Guests,@IsSolo,@CurrencyCode,@RatePlanId,@Child) order by price ";

                    _FarmStaysBookingDetail = _DbManager.SetCommand(_SqlAvailabilityQuery,
                                                             _DbManager.Parameter("@FarmStayId", _SearchFarmStays.FarmStayId),
                                                             _DbManager.Parameter("@RoomId", _SearchFarmStays.RoomId),
                                                             _DbManager.Parameter("@CheckIn", Convert.ToDateTime(_SearchFarmStays.CheckIn)),
                                                             _DbManager.Parameter("@CheckOut", Convert.ToDateTime(_SearchFarmStays.CheckOut)),
                                                             _DbManager.Parameter("@Guests", _SearchFarmStays.Guests),
                                                             _DbManager.Parameter("@IsSolo", _SearchFarmStays.IsSolo),
                                                             _DbManager.Parameter("@CurrencyCode", _SearchFarmStays.CurrencyCode),
                                                             _DbManager.Parameter("@RatePlanId", _SearchFarmStays.RatePlanId),
                                                             _DbManager.Parameter("@Child", _SearchFarmStays.Child)).ExecuteObject<BookingResponse>();
                    if (_FarmStaysBookingDetail != null)
                    {
                        _FarmStaysBookingDetail.RatePlanName = Helper.GetEnumDescription((RatePlanEnum)_SearchFarmStays.RatePlanId);
                        if (!_FarmStaysBookingDetail.CancellationPolicyIsNonRefundable)
                        {
                            _FarmStaysBookingDetail.IsApplyCancellationPolicy = (Convert.ToDateTime(_SearchFarmStays.CheckIn) - DateTime.Now.Date).Days > _FarmStaysBookingDetail.RefundableBeforDays ? true : false;
                        }


                        string _SqlTermsAndConditionsQuery = @"SELECT [Description] from [dbo].[CMSPageMaster] where CMSPageId =" + (int)CMSPages.TermsAndConditions;
                        _FarmStaysBookingDetail.TermsAndConditions = _DbManager.SetCommand(_SqlTermsAndConditionsQuery).ExecuteScalar<string>();
                    }
                    if (_FarmStaysBookingDetail != null && _FarmStaysBookingDetail.DiscountPrice > 0)
                    {
                        string _SqlDiscountQuery = @"select Name,IsPercentage,IsEBO,DaysBeforeBooking,
                                                    case when IsPercentage = 1 then DiscountValue else dbo.CurrecncyConversion(@CurrencyCode,DiscountValue) end  as DiscountValue
                                                     from [GetAppliedDiscountOnFarmstays](@FarmStayId,@CheckIn,@CheckOut,@Price,0)";

                        List<DiscountDto> _FarmStaysDiscount = _DbManager.SetCommand(_SqlDiscountQuery,
                                                             _DbManager.Parameter("@FarmStayId", _SearchFarmStays.FarmStayId),
                                                             _DbManager.Parameter("@CheckIn", Convert.ToDateTime(_SearchFarmStays.CheckIn)),
                                                             _DbManager.Parameter("@CheckOut", Convert.ToDateTime(_SearchFarmStays.CheckOut)),
                                                             _DbManager.Parameter("@Price", _FarmStaysBookingDetail.Price),
                                                             _DbManager.Parameter("@CurrencyCode", _SearchFarmStays.CurrencyCode)).ExecuteList<DiscountDto>();

                        if (_FarmStaysDiscount != null)
                        {
                            _FarmStaysBookingDetail.FarmStayDiscount = string.Empty;
                            foreach (var item in _FarmStaysDiscount)
                            {
                                _FarmStaysBookingDetail.FarmStayDiscount = _FarmStaysBookingDetail.FarmStayDiscount + (_FarmStaysBookingDetail.FarmStayDiscount != string.Empty ? ", " : "");
                                if (item.IsEBO)
                                {
                                    string _Discount = "Early Bird Discount Applied " + (item.IsPercentage ? item.DiscountValue + "%" : "₹" + item.DiscountValue);
                                    _FarmStaysBookingDetail.EBDAmount = ((_FarmStaysBookingDetail.Price * item.DiscountValue) / 100);
                                    _FarmStaysBookingDetail.EBDName = _Discount;
                                    _FarmStaysBookingDetail.FarmStayDiscount = _FarmStaysBookingDetail.FarmStayDiscount + _Discount;
                                }
                                else
                                {
                                    string _Discount = item.Name + " " + (item.IsPercentage ? item.DiscountValue + "%" : Helper.GetCurrentCurrencySymbol() + " " + item.DiscountValue) + " off";
                                    _FarmStaysBookingDetail.DiscountAmount = (item.IsPercentage ? ((_FarmStaysBookingDetail.Price * item.DiscountValue) / 100) : item.DiscountValue);
                                    _FarmStaysBookingDetail.DiscountName = _Discount;
                                    _FarmStaysBookingDetail.FarmStayDiscount = _FarmStaysBookingDetail.FarmStayDiscount + _Discount;
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

            return _FarmStaysBookingDetail;
        }


        public int BookFarmStayRoom(BookingResponse BookingResponse)
        {
            int _OrderId = 0;
            int _NoOfRooms = 1;
            FarmStaysBookingDetail _FarmStaysBookingDetail = new FarmStaysBookingDetail();

            try
            {
                int _Days = (Convert.ToDateTime(BookingResponse.CheckOut) - Convert.ToDateTime(BookingResponse.CheckIn)).Days;

                #region OrderNo
                string[] _ListofChar = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

                List<string> _FarmStayName = BookingResponse.FarmStayName.Split(' ').ToList();
                string _FarmStaySortName = string.Empty;

                foreach (var item in _FarmStayName)
                {
                    _FarmStaySortName = _FarmStaySortName + item[0];
                }
                _FarmStaySortName = _FarmStaySortName.Replace(",", "");
                _FarmStaySortName = _FarmStaySortName.Replace("_", "");
                _FarmStaySortName = _FarmStaySortName.Replace("@", "");
                DateTime _Today = DateTime.Now;
                #endregion

                using (DbManager _DbManager = new DbManager())
                {
                    #region Get/Set OrderNo
                    string _SqlOrderIdQuery = @"select count(*) from OrderMaster where Farmstaysid=@Farmstaysid and Cast(OrderDate as date)=Cast(GETDATE() as date)";
                    int _OrderCount = _DbManager.SetCommand(_SqlOrderIdQuery, _DbManager.Parameter("@Farmstaysid", BookingResponse.FarmStayId)).ExecuteScalar<int>();

                    string _OrderNo = _FarmStaySortName + _Today.Year + _Today.Month.ToString("d2") + _Today.Day.ToString("d2") + _ListofChar[_OrderCount];
                    #endregion

                    #region Currency

                    string _Currency = "select Value from CurrencyMaster where CurrencyCode='" + BookingResponse.CurrencyCode + "'";
                    decimal _CurrencyRate = _DbManager.SetCommand(_Currency).ExecuteScalar<decimal>();

                    if (_CurrencyRate != 1)
                    {
                        BookingResponse.Price = Math.Round(BookingResponse.Price / _CurrencyRate);
                        BookingResponse.DiscountPrice = Math.Round(BookingResponse.DiscountPrice / _CurrencyRate);
                        BookingResponse.DiscountAmount = Math.Round(BookingResponse.DiscountAmount / _CurrencyRate);
                        BookingResponse.EBDAmount = Math.Round(BookingResponse.EBDAmount / _CurrencyRate);
                    }

                    #endregion

                    #region Add Order
                    string _SqlOrderQuery = @"INSERT INTO [dbo].[OrderMaster]
                                                   ([OrderNo]
                                                   ,[OrderDate]
                                                   ,[Farmstaysid]
                                                   ,[CustomerId]
                                                   ,[CheckInDate]
                                                   ,[CheckOutDate]
                                                   ,[NoOfPeople]
                                                   ,[Amount]
                                                   ,[EBDAmount]
                                                   ,[EBDName]
                                                   ,[DiscountAmount]
                                                   ,[DiscountName]
                                                   ,[NetAmount]
                                                   ,[FarmStaysName]
                                                   ,[CheckInTime]
                                                   ,[CheckOutTime]
                                                   ,[CancellationPolicyIsNonRefundable]
                                                   ,[RefundablePercentage]
                                                   ,[RefundableBeforDays]
                                                   ,[MarkupPercentage]
                                                   ,[GuestFirstName]
                                                   ,[GuestLastName]
                                                   ,[GuestEmail]
                                                   ,[IsMale]
                                                   ,[GuestCountryId]
                                                   ,[GuestCity]
                                                   ,[GuestMobile]
                                                   ,[GuestAddress]
                                                   ,[Status]
                                                   ,BaseCurrencyCode
                                                   ,BaseCurrencyRate
                                                   ,[Name]
                                                   ,[Type]
                                                   ,[IsSoloBooking])
                                                    OUTPUT Inserted.Id 
                                                     VALUES
                                                           (
                                                     @OrderNo
                                                    ,@OrderDate
                                                    ,@Farmstaysid
                                                    ,@CustomerId
                                                    ,@CheckInDate
                                                    ,@CheckOutDate
                                                    ,@NoOfPeople
                                                    ,@Amount
                                                    ,@EBDAmount
                                                    ,@EBDName
                                                    ,@DiscountAmount
                                                    ,@DiscountName
                                                    ,@NetAmount
                                                    ,@FarmStaysName
                                                    ,@CheckInTime
                                                    ,@CheckOutTime
                                                    ,@CancellationPolicyIsNonRefundable
                                                    ,@RefundablePercentage
                                                    ,@RefundableBeforDays
                                                    ,@MarkupPercentage
                                                    ,@GuestFirstName
                                                    ,@GuestLastName
                                                    ,@GuestEmail
                                                    ,@IsMale
                                                    ,@GuestCountryId
                                                    ,@GuestCity
                                                    ,@GuestMobile
                                                    ,@GuestAddress
                                                    ,@Status
                                                    ,@BaseCurrencyCode
                                                    ,@BaseCurrencyRate
                                                    ,@Name
			                                        ,@Type
			                                        ,@IsSoloBooking)";


                    _OrderId = _DbManager.SetCommand(_SqlOrderQuery,
                                                            _DbManager.Parameter("@OrderNo", _OrderNo),
                                                            _DbManager.Parameter("@OrderDate", DateTime.Now),
                                                            _DbManager.Parameter("@Farmstaysid", BookingResponse.FarmStayId),
                                                            _DbManager.Parameter("@CustomerId", BookingResponse.LeadTraveler.CustomerId),
                                                            _DbManager.Parameter("@CheckInDate", Convert.ToDateTime(BookingResponse.CheckIn)),
                                                            _DbManager.Parameter("@CheckOutDate", Convert.ToDateTime(BookingResponse.CheckOut)),
                                                            _DbManager.Parameter("@NoOfPeople", BookingResponse.Guests),
                                                            _DbManager.Parameter("@Amount", BookingResponse.Price),
                                                            _DbManager.Parameter("@EBDAmount", BookingResponse.EBDAmount),
                                                            _DbManager.Parameter("@EBDName", BookingResponse.EBDName),
                                                            _DbManager.Parameter("@DiscountAmount", BookingResponse.DiscountAmount),
                                                            _DbManager.Parameter("@DiscountName", BookingResponse.DiscountName),
                                                            _DbManager.Parameter("@NetAmount", BookingResponse.DiscountPrice > 0 ? BookingResponse.DiscountPrice : BookingResponse.Price),
                                                            _DbManager.Parameter("@FarmStaysName", BookingResponse.FarmStayName),
                                                            _DbManager.Parameter("@CheckInTime", BookingResponse.CheckInTime),
                                                            _DbManager.Parameter("@CheckOutTime", BookingResponse.CheckOutTime),
                                                            _DbManager.Parameter("@CancellationPolicyIsNonRefundable", BookingResponse.CancellationPolicyIsNonRefundable),
                                                            _DbManager.Parameter("@RefundablePercentage", BookingResponse.RefundablePercentage),
                                                            _DbManager.Parameter("@RefundableBeforDays", BookingResponse.RefundableBeforDays),
                                                            _DbManager.Parameter("@MarkupPercentage", BookingResponse.MarkupPercentage),
                                                            _DbManager.Parameter("@GuestFirstName", BookingResponse.LeadTraveler.GuestFirstName),
                                                            _DbManager.Parameter("@GuestLastName", BookingResponse.LeadTraveler.GuestLastName),
                                                            _DbManager.Parameter("@GuestEmail", BookingResponse.LeadTraveler.GuestEmail),
                                                            _DbManager.Parameter("@IsMale", BookingResponse.LeadTraveler.IsMale.Value),
                                                            _DbManager.Parameter("@GuestCountryId", BookingResponse.LeadTraveler.GuestCountryId),
                                                            _DbManager.Parameter("@GuestCity", BookingResponse.LeadTraveler.GuestCity),
                                                            _DbManager.Parameter("@GuestMobile", BookingResponse.LeadTraveler.GuestMobile),
                                                            _DbManager.Parameter("@GuestAddress", BookingResponse.LeadTraveler.GuestAddress),
                                                            _DbManager.Parameter("@Status", (int)BookingStatus.Pending),
                                                            _DbManager.Parameter("@BaseCurrencyCode", BookingResponse.CurrencyCode),
                                                            _DbManager.Parameter("@BaseCurrencyRate", _CurrencyRate),
                                                            _DbManager.Parameter("@Name", BookingResponse.Name),
                                                            _DbManager.Parameter("@Type", BookingResponse.Type),
                                                            _DbManager.Parameter("@IsSoloBooking", BookingResponse.IsSolo)).ExecuteScalar<int>();

                    #endregion

                    #region Add Inventory
                    if (_OrderId > 0)
                    {
                        DateTime _CheckIn = Convert.ToDateTime(BookingResponse.CheckIn);
                        string _SqlInventoryQuery = @"INSERT INTO [dbo].[InventoryMaster]
                                                       ([FarmstaysId]
                                                       ,[RoomId]
                                                       ,[BookingDate]
                                                       ,[MaxPerson]
                                                       ,[NumberOfPerson]
                                                       ,[Name]
                                                       ,[Type]
                                                       ,[OrderId]
                                                       ,[IsSoloBooking]
                                                       ,[NoOfRooms]
                                                       ,[RatePlanId]
                                                       ,[RatePlanName]
                                                       ,[ExtraBed])
                                                       VALUES
                                                       (
			                                            @FarmstaysId
			                                            ,@RoomId
			                                            ,@BookingDate
			                                            ,@MaxPerson
			                                            ,@NumberOfPerson
			                                            ,@Name
			                                            ,@Type
			                                            ,@OrderId
			                                            ,@IsSoloBooking
                                                        ,@NoOfRooms
			                                            ,@RatePlanId
                                                        ,@RatePlanName
                                                        ,@ExtraBed
			                                            )";
                        for (int i = 0; i < _Days; i++)
                        {
                            int _row = _DbManager.SetCommand(_SqlInventoryQuery,
                                                                 _DbManager.Parameter("@Farmstaysid", BookingResponse.FarmStayId),
                                                                 _DbManager.Parameter("@RoomId", BookingResponse.RoomId),
                                                                 _DbManager.Parameter("@BookingDate", _CheckIn),
                                                                 _DbManager.Parameter("@MaxPerson", BookingResponse.MaxPerson),
                                                                 _DbManager.Parameter("@NumberOfPerson", BookingResponse.Guests),
                                                                 _DbManager.Parameter("@Name", BookingResponse.Name),
                                                                 _DbManager.Parameter("@Type", BookingResponse.Type),
                                                                 _DbManager.Parameter("@OrderId", _OrderId),
                                                                 _DbManager.Parameter("@IsSoloBooking", BookingResponse.IsSolo),
                                                                 _DbManager.Parameter("@NoOfRooms", _NoOfRooms),
                                                                 _DbManager.Parameter("@RatePlanId", Helper.GetValueFromDescription<RatePlanEnum>(BookingResponse.RatePlanName)),
                                                                 _DbManager.Parameter("@RatePlanName", BookingResponse.RatePlanName),
                                                                 _DbManager.Parameter("@ExtraBed", BookingResponse.ExtraBed)).ExecuteNonQuery();

                            _CheckIn = _CheckIn.AddDays(1);
                        }

                        return _OrderId;
                    }
                    #endregion
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return 0;
        }

        public bool ChangeOrderStatus(int Id, int Status, decimal RefundAmount, string CancellationReason)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"update OrderMaster set Status=@Status, UpdatedDate=@UpdatedDate";

                    if (Status == (int)BookingStatus.Cancel)
                        _SqlQuery = _SqlQuery + @", RefundAmount=@RefundAmount,CancellationReason=@CancellationReason ";

                    _SqlQuery = _SqlQuery + " where Id=" + Id;

                    int _Row = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Status", Status),
                         _DbManager.Parameter("@UpdatedDate", DateTime.Now),
                          _DbManager.Parameter("@RefundAmount", RefundAmount),
                          _DbManager.Parameter("@CancellationReason", CancellationReason)
                        ).ExecuteNonQuery();
                    if (Status == (int)BookingStatus.CONFIRMED)
                    {
                        bool _AxisInventory = AxisInventoryDayWise(_DbManager,Id);
                    }
                    return _Row > 0 ? true : false;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return false;
        }

        public bool AxisInventoryDayWise(DbManager _DbManager,int _OrderId)
        {
            try
            {

                string accessKey = @System.Configuration.ConfigurationManager.AppSettings["AccessKey"].ToString();
                string channelId = @System.Configuration.ConfigurationManager.AppSettings["ChannelId"].ToString();

                string _SqlQuery = @"Select O.Id,O.FarmStaysid,O.CheckIndate,O.CheckOutDate,IM.RoomId
                                     from OrderMaster O
                                     inner join InventoryMaster IM on (IM.OrderId = O.Id)
									 WHERE O.Id = @OrderId";

                List<GetResponseByOrderId> _List = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@OrderId", _OrderId)
                    ).ExecuteList<GetResponseByOrderId>();

                DateTime _StartDate = _List.FirstOrDefault().CheckInDate;
                DateTime _EndDate = _List.FirstOrDefault().CheckOutDate;

                List<DateTime> allDates = new List<DateTime>();
                for (DateTime date = _StartDate; date <= _EndDate; date = date.AddDays(1))
                    allDates.Add(date);

                AxisRoomInventoryRequest _AxisRoomInventoryRequest = new AxisRoomInventoryRequest();
                _AxisRoomInventoryRequest.accessKey = accessKey;
                _AxisRoomInventoryRequest.channelId = channelId;

                InventoryHotel _Hotel = new InventoryHotel();
                _Hotel.hotelId = Convert.ToString(Convert.ToInt32(_List.FirstOrDefault().Farmstaysid));

                InventoryRoom _Room = new InventoryRoom();
                _Room.roomId = Convert.ToString(Convert.ToInt32(_List.FirstOrDefault().RoomId));

                List<AvailabilityArray> _AvailabilityList = new List<AvailabilityArray>();

                foreach (DateTime _Date in allDates)
                {
                    AvailabilityArray _AvailabilityArray = new AvailabilityArray();
                    _AvailabilityArray.date = _Date.ToString("yyyy-MM-dd");
                    _AvailabilityArray.free = 0;
                    _AvailabilityList.Add(_AvailabilityArray);
                }
                _Room.availability = _AvailabilityList;
                //_Hotel.rooms = _Room;
                //_AxisRoomInventoryRequest.hotels = _Hotel;

                _AxisRoomInventoryRequest.hotels = new List<InventoryHotel>();
                _AxisRoomInventoryRequest.hotels.Add(_Hotel);
                _AxisRoomInventoryRequest.hotels[0].rooms = new List<InventoryRoom>();
                _AxisRoomInventoryRequest.hotels[0].rooms.Add(_Room);

                HttpClient client = new HttpClient();

                var p = new JavaScriptSerializer().Serialize(_AxisRoomInventoryRequest);

                var response = client.PostAsJsonAsync("https://sandbox-pms.axisrooms.com/api/daywiseInventory", _AxisRoomInventoryRequest).Result;
                    
                    return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return false;
        }

        public bool AxisBookingAPIIntegration(List<AxisBookingResponse> _AxisBookingList)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    int _OrderId = 0;
                    int CPINR = 0;

                    foreach (AxisBookingResponse _AxisBookingResponse in _AxisBookingList)
                    {
                        #region Get Farm Stay Name By Farmstaysid
                        string SQLSelectQuery = "Select Name from FarmStays WHERE Id = @FarmStayId";
                        FarmStaysDetail _FarmStaysDetail = _DbManager.SetCommand(SQLSelectQuery,
                                                                _DbManager.Parameter("FarmStayId", _AxisBookingResponse.BookingDetails.hotelId)).ExecuteObject<FarmStaysDetail>();
                        #endregion

                        #region Get Paid Value
                        int _PaidValue = 0;
                        if (_AxisBookingResponse.CheckinDetails.paid.ToLower() == "True".ToLower())
                        {
                            _PaidValue = (int)AxisPaid.True;
                        }
                        else if (_AxisBookingResponse.CheckinDetails.paid.ToLower() == "False".ToLower())
                        {
                            _PaidValue = (int)AxisPaid.False;
                        }
                        else if (_AxisBookingResponse.CheckinDetails.paid.ToLower() == "Partial".ToLower())
                        {
                            _PaidValue = (int)AxisPaid.Partial;
                        }
                        #endregion

                        #region Add Order
                        string _SqlOrderQuery = @"INSERT INTO [dbo].[OrderMaster]
                                                   ([BookedBy]
                                                   ,[OrderDate]
                                                   ,[OrderNo]
                                                   ,[BookingSource]
                                                   ,[BookingSourceRefId]
                                                   ,[Status]
                                                   ,[Farmstaysid]
                                                   ,[OTA]
                                                   ,[OTARefId]
                                                   ,[AmountToBeCollected]
                                                   ,[CheckInDate]
                                                   ,[CheckOutDate]
                                                   ,[Children]
                                                   ,[Paid]
                                                   ,[CountryCode]
                                                   ,[GuestEmail]
                                                   ,[GuestFirstname]
                                                   ,[GuestMobile]
                                                   ,[CancellationPolicyIsNonRefundable]
                                                   ,[FarmStaysName]
                                                   ,[NoOfPeople]
                                                   ,[CheckInTime]
                                                   ,[CheckOutTime]
                                                   ,[Amount]
                                                   ,[NetAmount])
                                                    OUTPUT Inserted.Id 
                                                     VALUES
                                                           (
                                                     @BookedBy
                                                    ,@OrderDate
                                                    ,@OrderNo
                                                    ,@BookingSource
                                                    ,@BookingSourceRefId
                                                    ,@Status
                                                    ,@Farmstaysid
                                                    ,@OTA
                                                    ,@OTARefId
                                                    ,@AmountToBeCollected
                                                    ,@CheckInDate
                                                    ,@CheckOutDate
                                                    ,@Children
                                                    ,@Paid
                                                    ,@CountryCode
                                                    ,@GuestEmail
                                                    ,@GuestFirstname
                                                    ,@GuestMobile
                                                    ,@CancellationPolicyIsNonRefundable
                                                    ,@FarmStaysName
                                                    ,@NoOfPeople
                                                    ,@CheckInTime
                                                    ,@CheckOutTime
                                                    ,@Amount
                                                    ,@NetAmount)";


                        _OrderId = _DbManager.SetCommand(_SqlOrderQuery,
                                                                _DbManager.Parameter("@BookedBy", _AxisBookingResponse.BookingDetails.bookedBy),
                                                                _DbManager.Parameter("@OrderDate", Convert.ToDateTime(_AxisBookingResponse.BookingDetails.bookingDateTime)),
                                                                _DbManager.Parameter("@OrderNo", _AxisBookingResponse.BookingDetails.bookingNo),
                                                                _DbManager.Parameter("@BookingSource", _AxisBookingResponse.BookingDetails.bookingSource),
                                                                _DbManager.Parameter("@BookingSourceRefId", _AxisBookingResponse.BookingDetails.bookingSourceRefId),
                                                                _DbManager.Parameter("@Status", _AxisBookingResponse.BookingDetails.bookingStatus == "confirmed" ? (int)BookingStatus.CONFIRMED : (int)BookingStatus.Failed),
                                                                _DbManager.Parameter("@Farmstaysid", _AxisBookingResponse.BookingDetails.hotelId),
                                                                _DbManager.Parameter("@OTA", _AxisBookingResponse.BookingDetails.ota),
                                                                _DbManager.Parameter("@OTARefId", _AxisBookingResponse.BookingDetails.otaRefId),
                                                                _DbManager.Parameter("@AmountToBeCollected", _AxisBookingResponse.CheckinDetails.amountToBeCollected),
                                                                _DbManager.Parameter("@CheckInDate", Convert.ToDateTime(_AxisBookingResponse.CheckinDetails.checkInDate)),
                                                                _DbManager.Parameter("@CheckOutDate", Convert.ToDateTime(_AxisBookingResponse.CheckinDetails.checkOutDate)),
                                                                _DbManager.Parameter("@Children", _AxisBookingResponse.CheckinDetails.children),
                                                                _DbManager.Parameter("@Paid", _PaidValue),
                                                                _DbManager.Parameter("@CountryCode", _AxisBookingResponse.GuestDetails.countryCode),
                                                                _DbManager.Parameter("@GuestEmail", _AxisBookingResponse.GuestDetails.emailId),
                                                                _DbManager.Parameter("@GuestFirstname", _AxisBookingResponse.GuestDetails.guestName),
                                                                _DbManager.Parameter("@GuestMobile", _AxisBookingResponse.GuestDetails.mobileNo),
                                                                _DbManager.Parameter("@CancellationPolicyIsNonRefundable", CPINR),
                                                                _DbManager.Parameter("@FarmStaysName", _FarmStaysDetail.Name),
                                                                _DbManager.Parameter("@NoOfPeople", CPINR),
                                                                _DbManager.Parameter("@CheckInTime", DateTime.Now),
                                                                _DbManager.Parameter("@CheckOutTime", DateTime.Now),
                                                                _DbManager.Parameter("@Amount", _AxisBookingResponse.CheckinDetails.totalAmount),
                                                                _DbManager.Parameter("@NetAmount", _AxisBookingResponse.CheckinDetails.totalAmount)).ExecuteScalar<int>();

                        #endregion

                        #region Add Inventory
                        if (_OrderId > 0)
                        {
                            DateTime _CheckIn = Convert.ToDateTime(_AxisBookingResponse.CheckinDetails.checkInDate);
                            string _SqlInventoryQuery = @"INSERT INTO [dbo].[InventoryMaster]
                                                       ([FarmstaysId]
                                                       ,[RoomId]
                                                       ,[OrderId]
                                                       ,[BookingDate]
                                                       ,[NumberOfPerson]
                                                       ,[NoOfRooms]
                                                       ,[RatePlanId]
                                                       ,[RatePlanName]
                                                       ,[IsSoloBooking])
                                                       VALUES
                                                       (
			                                            @FarmstaysId
			                                            ,@RoomId
                                                        ,@OrderId
			                                            ,@BookingDate
			                                            ,@NumberOfPerson
			                                            ,@NoOfRooms
			                                            ,@RatePlanId
			                                            ,@RatePlanName
			                                            ,@IsSoloBooking)";
                            foreach (var RT in _AxisBookingResponse.Rates.roomType)
                            {
                                int _row = _DbManager.SetCommand(_SqlInventoryQuery,
                                                                     _DbManager.Parameter("@Farmstaysid", _AxisBookingResponse.BookingDetails.hotelId),
                                                                     _DbManager.Parameter("@RoomId", RT.id),
                                                                     _DbManager.Parameter("@OrderId", _OrderId),
                                                                     _DbManager.Parameter("@BookingDate", Convert.ToDateTime(_AxisBookingResponse.CheckinDetails.checkInDate)),
                                                                     _DbManager.Parameter("@NumberOfPerson", RT.totalAdults),
                                                                     _DbManager.Parameter("@NoOfRooms", RT.noOfRooms),
                                                                     _DbManager.Parameter("@RatePlanId", RT.ratePlanId),
                                                                     _DbManager.Parameter("@RatePlanName", RT.ratePlanName),
                                                                     _DbManager.Parameter("@IsSoloBooking", false)).ExecuteNonQuery();

                                _CheckIn = _CheckIn.AddDays(1);
                            }
                        }
                        #endregion
                        
                    }

                    return true;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return false;

        }

        public bool AddPaymentHistory(int Id, int Status,string PayuMoneyId, string PaymentResponse)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"INSERT INTO PaymentHistory
                                           ([OrderId]
                                           ,[PaymentId]
                                           ,[Response]
                                           ,[Status])
                                     VALUES
                                           (@OrderId
                                           ,@PaymentId
                                           ,@Response
                                           ,@Status)";

                    int _Row = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Status", (int)BookingStatus.Cancel),
                        //_DbManager.Parameter("@UpdatedDate", DateTime.Now),
                          _DbManager.Parameter("@PaymentId", PayuMoneyId),
                          _DbManager.Parameter("@OrderId", Id),
                          _DbManager.Parameter("@Response", PaymentResponse)
                        ).ExecuteNonQuery();

                    return _Row > 0 ? true : false;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return false;
        }

        public string GetPaymentIdbyOrderId(int Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"select top 1 PaymentId from  PaymentHistory where OrderId=@OrderId";

                    string _Row = _DbManager.SetCommand(_SqlQuery,_DbManager.Parameter("@OrderId", Id) ).ExecuteScalar<string>();

                    return _Row ;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return string.Empty;
        }
    }
}
