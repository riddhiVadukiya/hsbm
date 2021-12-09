using BLToolkit.Data;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.DiscountMaster;
using HSBM.EntityModel.FarmStays;
using HSBM.EntityModel.AxisRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSBM.Common.Enums;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace HSBM.Repository.Repositories
{
    public class FarmStaysRepository
    {

        public GridDataResponse GetAllFarmStaysBySearchRequest(GridParams p_GridParams, FarmStaysRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" SELECT *, RecordsTotal = COUNT(*) OVER() from [dbo].[FarmStays]";

                    bool IsWhereClauseEmpty = true;

                    if (p_Request.Name != null && p_Request.Name != string.Empty)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;

                        _SqlQuery += @" [Name] LIKE '%" + p_Request.Name + @"%'";
                    }

                    if (p_Request.IncludeIsDeleted == false)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;

                        _SqlQuery += @" [IsActive] = 'true'";
                    }

                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    var objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<FarmStaysRequest>();
                    if (objList.Any())
                    {
                        _GridDataResponse.recordsTotal = objList.FirstOrDefault().RecordsTotal;
                        _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    }
                    _GridDataResponse.data = objList;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _GridDataResponse;
        }


        private bool CheckNameForDuplicate(DbManager _DbManager, FarmStaysBasicDetail FarmStaysBasicDetail)
        {
            try
            {
                String _SqlQuery = @"SELECT COUNT(Id) FROM [dbo].[FarmStays] WHERE [Id] != @Id AND [Name] = @Name";

                int _ExistCount = _DbManager.SetCommand(_SqlQuery,
                _DbManager.Parameter("@Id", FarmStaysBasicDetail.Id),
                _DbManager.Parameter("@Name", FarmStaysBasicDetail.Name)).ExecuteScalar<int>();

                if (_ExistCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public FarmStays GetFarmStayBasicDetailById(int Id)
        {
            FarmStays FarmStays = new FarmStays();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT * FROM [dbo].[FarmStays] WHERE [Id] = @Id ";

                    FarmStays = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<FarmStays>().FirstOrDefault();


                    String _CategorySqlQuery = @"SELECT CategoryId FROM FarmStaysCategories where FarmStaysId = @Id ";
                    int[] _CategoryList = _DbManager.SetCommand(_CategorySqlQuery, _DbManager.Parameter("@Id", Id)).ExecuteScalarList<int>().ToArray();

                    if (_CategoryList != null && _CategoryList.Length > 0)
                    {
                        FarmStays.CategoryIds = string.Join(",", _CategoryList);
                    }

                    String _AmenitySqlQuery = @"SELECT * FROM FarmStaysAmenities where FarmStaysId = @Id ";
                    FarmStays.FarmStaysAmenities = _DbManager.SetCommand(_AmenitySqlQuery,
                       _DbManager.Parameter("@Id", Id)).ExecuteList<FarmStaysAmenities>();

                    String _ImageSqlQuery = @"SELECT * FROM FarmStaysImages where FarmStaysId = @Id ";
                    FarmStays.FarmStaysImages = _DbManager.SetCommand(_ImageSqlQuery,
                       _DbManager.Parameter("@Id", Id)).ExecuteList<FarmStaysImages>();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return FarmStays;
        }


        public void ActiveAndInactiveFarmStay(FarmStays FarmStays)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"UPDATE [dbo].[FarmStays] SET [IsActive] = @IsActive WHERE [Id] = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                                                    _DbManager.Parameter("@Id", FarmStays.Id),
                                                    _DbManager.Parameter("@IsActive", FarmStays.IsActive)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public int AddOrUpdateFarmStayBasicDetail(FarmStaysBasicDetail FarmStaysBasicDetail)
        {
            try
            {
                String _SqlQuery;
                int _FarmStayId = 0;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExist = CheckNameForDuplicate(_DbManager, FarmStaysBasicDetail);

                    if (!_IsExist)
                    {
                        if (FarmStaysBasicDetail.Id != 0)
                        {
                            _SqlQuery = @"
                                        UPDATE FarmStays SET Name = @Name,Email=@Email,Description = @Description,Bedrooms = @Bedrooms,CheckInTime = @CheckInTime,CheckOutTime = @CheckOutTime,Telephone = @Telephone,Mobile = @Mobile,
                                       ExtraBedPrice = @ExtraBedPrice,Address = @Address,MarkupPercentage = @MarkupPercentage,Latitude=@Latitude,Longitude=@Longitude,
   Bathrooms = @Bathrooms,USPTags = @USPTags,Persuasions = @Persuasions,LanguagesSpoken=@LanguagesSpoken,TypeofFood=@TypeofFood,OrderIndex=@OrderIndex,IsPackage=@IsPackage,Location=@Location,Neighborhood=@Neighborhood,
                                        UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE[Id] = @Id";
                            if (_DbManager.SetCommand(_SqlQuery,
                     _DbManager.Parameter("@Id", FarmStaysBasicDetail.Id),
                     _DbManager.Parameter("@Name", FarmStaysBasicDetail.Name),
                                //_DbManager.Parameter("@CategoryId", FarmStaysBasicDetail.CategoryId),
                     _DbManager.Parameter("@Email ", FarmStaysBasicDetail.Email),
                     _DbManager.Parameter("@Description ", FarmStaysBasicDetail.Description),
                     _DbManager.Parameter("@Bedrooms", FarmStaysBasicDetail.Bedrooms),
                     _DbManager.Parameter("@CheckInTime ", FarmStaysBasicDetail.CheckInTime),
                     _DbManager.Parameter("@CheckOutTime", FarmStaysBasicDetail.CheckOutTime),
                     _DbManager.Parameter("@Telephone ", FarmStaysBasicDetail.Telephone),
                     _DbManager.Parameter("@Mobile", FarmStaysBasicDetail.Mobile),
                     _DbManager.Parameter("@ExtraBedPrice ", FarmStaysBasicDetail.ExtraBedPrice),
                     _DbManager.Parameter("@Address ", FarmStaysBasicDetail.Address),
                     _DbManager.Parameter("@MarkupPercentage", FarmStaysBasicDetail.MarkupPercentage),
                     _DbManager.Parameter("@Latitude", FarmStaysBasicDetail.Latitude),
                     _DbManager.Parameter("@Longitude", FarmStaysBasicDetail.Longitude),
                     _DbManager.Parameter("@IsActive", FarmStaysBasicDetail.IsActive),
                     _DbManager.Parameter("@Bathrooms", FarmStaysBasicDetail.Bathrooms),
                     _DbManager.Parameter("@USPTags", FarmStaysBasicDetail.USPTags),
                     _DbManager.Parameter("@Persuasions", FarmStaysBasicDetail.Persuasions),
                     _DbManager.Parameter("@LanguagesSpoken", FarmStaysBasicDetail.LanguagesSpoken),
                     _DbManager.Parameter("@TypeofFood", FarmStaysBasicDetail.TypeofFood),
                     _DbManager.Parameter("@OrderIndex", FarmStaysBasicDetail.OrderIndex),
                      _DbManager.Parameter("@IsPackage", FarmStaysBasicDetail.IsPackage),
                        _DbManager.Parameter("@Location", FarmStaysBasicDetail.Location),
                        _DbManager.Parameter("@Neighborhood", FarmStaysBasicDetail.Neighborhood),
                     _DbManager.Parameter("@UpdatedBy", null),
                     _DbManager.Parameter("@UpdatedDate", null)).ExecuteNonQuery() > 0)
                            {
                                _FarmStayId = Convert.ToInt32(FarmStaysBasicDetail.Id);
                                string _deleteCategory = @"Delete from FarmStaysCategories where FarmStaysId=" + _FarmStayId;
                                _DbManager.SetCommand(_deleteCategory).ExecuteNonQuery();
                            }

                        }
                        else
                        {
                            _SqlQuery = @"
                                    INSERT INTO FarmStays(Name,Email,Description,Bedrooms,CheckInTime,CheckOutTime,Telephone,Mobile,ExtraBedPrice
,Address,MarkupPercentage,Latitude,Longitude,IsActive,CreatedBy,
Bathrooms,USPTags,Persuasions,LanguagesSpoken,TypeofFood,OrderIndex,IsPackage,Location,Neighborhood,
CreatedDate,UpdatedBy,UpdatedDate) 
                                     output 
                                    Inserted.Id VALUES (@Name,@Email,@Description,@Bedrooms,@CheckInTime,@CheckOutTime,@Telephone,@Mobile,@ExtraBedPrice
,@Address,@MarkupPercentage,@Latitude,@Longitude,@IsActive,@CreatedBy
,@Bathrooms,@USPTags,@Persuasions,@LanguagesSpoken,@TypeofFood,@OrderIndex,@IsPackage,@Location,@Neighborhood,
@CreatedDate,@UpdatedBy,@UpdatedDate)";
                            _FarmStayId = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", FarmStaysBasicDetail.Id),
                        _DbManager.Parameter("@Name", FarmStaysBasicDetail.Name),
                                //_DbManager.Parameter("@CategoryId", FarmStaysBasicDetail.CategoryId),
                        _DbManager.Parameter("@Email ", FarmStaysBasicDetail.Email),
                        _DbManager.Parameter("@Description ", FarmStaysBasicDetail.Description),
                        _DbManager.Parameter("@Bedrooms", FarmStaysBasicDetail.Bedrooms),
                        _DbManager.Parameter("@CheckInTime ", FarmStaysBasicDetail.CheckInTime),
                        _DbManager.Parameter("@CheckOutTime", FarmStaysBasicDetail.CheckOutTime),
                        _DbManager.Parameter("@Telephone ", FarmStaysBasicDetail.Telephone),
                        _DbManager.Parameter("@Mobile", FarmStaysBasicDetail.Mobile),
                        _DbManager.Parameter("@ExtraBedPrice ", FarmStaysBasicDetail.ExtraBedPrice),
                        _DbManager.Parameter("@Address ", FarmStaysBasicDetail.Address),
                        _DbManager.Parameter("@MarkupPercentage", FarmStaysBasicDetail.MarkupPercentage),
                        _DbManager.Parameter("@Latitude", FarmStaysBasicDetail.Latitude),
                        _DbManager.Parameter("@Longitude", FarmStaysBasicDetail.Longitude),
                        _DbManager.Parameter("@IsActive", FarmStaysBasicDetail.IsActive),
                        _DbManager.Parameter("@Bathrooms", FarmStaysBasicDetail.Bathrooms),
                        _DbManager.Parameter("@USPTags", FarmStaysBasicDetail.USPTags),
                        _DbManager.Parameter("@Persuasions", FarmStaysBasicDetail.Persuasions),
                        _DbManager.Parameter("@LanguagesSpoken", FarmStaysBasicDetail.LanguagesSpoken),
                        _DbManager.Parameter("@TypeofFood", FarmStaysBasicDetail.TypeofFood),
                        _DbManager.Parameter("@OrderIndex", FarmStaysBasicDetail.OrderIndex),
                        _DbManager.Parameter("@IsPackage", FarmStaysBasicDetail.IsPackage),
                        _DbManager.Parameter("@Location", FarmStaysBasicDetail.Location),
                        _DbManager.Parameter("@Neighborhood", FarmStaysBasicDetail.Neighborhood),
                        _DbManager.Parameter("@CreatedBy", FarmStaysBasicDetail.CreatedBy),
                        _DbManager.Parameter("@CreatedDate", FarmStaysBasicDetail.CreatedDate),
                        _DbManager.Parameter("@UpdatedBy", null),
                        _DbManager.Parameter("@UpdatedDate", null)).ExecuteScalar<Int32>();
                        }

                        if (_FarmStayId > 0)
                        {
                            string[] _CategoryList = FarmStaysBasicDetail.CategoryIds.Split(',');

                            string _InsertCategory = @"INSERT INTO [dbo].[FarmStaysCategories]([FarmStaysId],[CategoryId]) VALUES (@FarmStaysId, @CategoryId)";
                            foreach (var item in _CategoryList)
                            {
                                _DbManager.SetCommand(_InsertCategory, _DbManager.Parameter("@FarmStaysId", _FarmStayId),
                                                         _DbManager.Parameter("@CategoryId", item)).ExecuteNonQuery();
                            }
                        }

                        String _ExistOrderSqlQuery = @"select count(*) from FarmStays where OrderIndex=" + FarmStaysBasicDetail.OrderIndex;
                        int _ExistOrder = _DbManager.SetCommand(_ExistOrderSqlQuery).ExecuteScalar<int>();

                        if (_ExistOrder > 1)
                        {
                            if (FarmStaysBasicDetail.OldOrderIndex == 0)
                            {
                                string _UpdateOdrerIndex = @"update [FarmStays] set OrderIndex=OrderIndex+1 where OrderIndex is not null and OrderIndex>=@OrderIndex and Id != @FarmStaysId";
                                _DbManager.SetCommand(_UpdateOdrerIndex, _DbManager.Parameter("@FarmStaysId", _FarmStayId),
                                                             _DbManager.Parameter("@OrderIndex", FarmStaysBasicDetail.OrderIndex)).ExecuteNonQuery();
                            }
                            else if (FarmStaysBasicDetail.OldOrderIndex < FarmStaysBasicDetail.OrderIndex)
                            {
                                string _UpdateOdrerIndex = @"update [FarmStays] set OrderIndex=OrderIndex-1 where OrderIndex is not null and OrderIndex>=@OldOrderIndex and OrderIndex<=@OrderIndex and Id != @FarmStaysId";
                                _DbManager.SetCommand(_UpdateOdrerIndex, _DbManager.Parameter("@FarmStaysId", _FarmStayId),
                                                            _DbManager.Parameter("@OldOrderIndex", FarmStaysBasicDetail.OldOrderIndex),
                                                             _DbManager.Parameter("@OrderIndex", FarmStaysBasicDetail.OrderIndex)).ExecuteNonQuery();
                            }
                            else if (FarmStaysBasicDetail.OldOrderIndex > FarmStaysBasicDetail.OrderIndex)
                            {
                                string _UpdateOdrerIndex = @"update [FarmStays] set OrderIndex=OrderIndex+1 where OrderIndex is not null and OrderIndex<=@OldOrderIndex and OrderIndex>=@OrderIndex and Id != @FarmStaysId";
                                _DbManager.SetCommand(_UpdateOdrerIndex, _DbManager.Parameter("@FarmStaysId", _FarmStayId),
                                                            _DbManager.Parameter("@OldOrderIndex", FarmStaysBasicDetail.OldOrderIndex),
                                                             _DbManager.Parameter("@OrderIndex", FarmStaysBasicDetail.OrderIndex)).ExecuteNonQuery();
                            }

                        }

                        return _FarmStayId;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
                return 0;
            }
        }

        public int SaveFarmStayAmenities(List<FarmStaysAmenities> Amenities)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {


                    string _DeleteSqlQuery = @"Delete FarmStaysAmenities where FarmStaysId = @Id";
                    _DbManager.SetCommand(_DeleteSqlQuery, _DbManager.Parameter("@Id", Amenities[0].FarmStaysId)).ExecuteNonQuery();

                    string _SqlQuery = @"
                                    Insert into FarmStaysAmenities(AmenityId,FarmStaysId) values(@AmenityId,@FarmStaysId)";

                    foreach (FarmStaysAmenities Amenity in Amenities)
                    {
                        int Affected = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@FarmStaysId", Amenity.FarmStaysId),
                        _DbManager.Parameter("@AmenityId ", Amenity.AmenityId)).ExecuteNonQuery();
                    }

                    return 1;

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
                return 0;
            }
        }

        public int SaveFarmStayPolicy(FarmStaysPolicyDetail FarmStaysPolicyDetail)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {


                    _SqlQuery = @"
                                 UPDATE FarmStays SET HouseRules = @HouseRules,StayPolicy = @StayPolicy,CancellationPolicyIsNonRefundable = @CancellationPolicyIsNonRefundable,
                                 RefundableBeforDays = @RefundableBeforDays,RefundablePercentage = @RefundablePercentage WHERE[Id] = @Id";

                    int Affected = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", FarmStaysPolicyDetail.Id),
                    _DbManager.Parameter("@HouseRules", FarmStaysPolicyDetail.HouseRules),
                    _DbManager.Parameter("@StayPolicy ", FarmStaysPolicyDetail.StayPolicy),
                    _DbManager.Parameter("@CancellationPolicyIsNonRefundable", FarmStaysPolicyDetail.CancellationPolicyIsNonRefundable),
                    _DbManager.Parameter("@RefundableBeforDays ", FarmStaysPolicyDetail.RefundableBeforDays),
                    _DbManager.Parameter("@RefundablePercentage", FarmStaysPolicyDetail.RefundablePercentage)).ExecuteNonQuery();


                    if (Affected > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<FarmStaysImages> SaveFarmStayImages(List<FarmStaysImages> FarmStaysImages)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _DeleteSqlQuery = @"Delete FarmStaysImages where Id = @Id";
                    string _InsertSqlQuery = @"Insert into FarmStaysImages(FarmStaysId,ImageName)  Output  Inserted.Id values(@FarmStaysId,@ImageName)";

                    foreach (var Image in FarmStaysImages)
                    {
                        if (Image.Id > 0 && Image.IsDeleted)
                        {
                            _DbManager.SetCommand(_DeleteSqlQuery, _DbManager.Parameter("@Id", Image.Id)).ExecuteNonQuery();

                        }
                        else if (Image.Id <= 0 && !Image.IsDeleted)
                        {
                            Image.Id = _DbManager.SetCommand(_InsertSqlQuery,
                       _DbManager.Parameter("@FarmStaysId", Image.FarmStaysId),
                       _DbManager.Parameter("@ImageName ", Image.ImageName)).ExecuteScalar<Int32>();
                        }
                    }


                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return FarmStaysImages;
        }

        #region Room
        public int SaveFarmStayRoom(FarmStaysRooms FarmStaysRooms)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    int _IsExist;
                    int _Availability = 0;

                    string _ExistQuery = @"select Count(*) from FarmStaysRooms where FarmStaysId=@FarmStaysId and Name=@Name and Id!=@Id";

                    _IsExist = _DbManager.SetCommand(_ExistQuery,
                        _DbManager.Parameter("@FarmStaysId", FarmStaysRooms.FarmStaysId),
                        _DbManager.Parameter("@Name", FarmStaysRooms.Name),
                        _DbManager.Parameter("@Id", FarmStaysRooms.Id)).ExecuteScalar<int>();

                    if (_IsExist <= 0)
                    {
                        string _SqlQuery;


                        if (FarmStaysRooms.Id > 0)
                        {
                            _SqlQuery = @"Update FarmStaysRooms set FarmStaysId=@FarmStaysId,Name=@Name,Type=@Type,MaxPerson=@MaxPerson,IsSolo=@IsSolo where Id=@Id";
                            return _DbManager.SetCommand(_SqlQuery,
                                     _DbManager.Parameter("@FarmStaysId", FarmStaysRooms.FarmStaysId),
                                     _DbManager.Parameter("@Name ", FarmStaysRooms.Name),
                                     _DbManager.Parameter("@Type", FarmStaysRooms.Type),
                                     _DbManager.Parameter("@MaxPerson", FarmStaysRooms.MaxPerson > 0 ? FarmStaysRooms.MaxPerson : _Availability),
                                     _DbManager.Parameter("@IsSolo", FarmStaysRooms.IsSolo),
                                     _DbManager.Parameter("@Id", FarmStaysRooms.Id)).ExecuteNonQuery();

                        }
                        else
                        {
                            _SqlQuery = @"Insert into FarmStaysRooms(FarmStaysId,Name,Type,MaxPerson,IsSolo) Output Inserted.Id 
                                                    values(@FarmStaysId,@Name,@Type,@MaxPerson,@IsSolo)";
                            return _DbManager.SetCommand(_SqlQuery,
                                     _DbManager.Parameter("@FarmStaysId", FarmStaysRooms.FarmStaysId),
                                     _DbManager.Parameter("@Name ", FarmStaysRooms.Name),
                                     _DbManager.Parameter("@Type", FarmStaysRooms.Type),
                                     _DbManager.Parameter("@MaxPerson", FarmStaysRooms.MaxPerson > 0 ? FarmStaysRooms.MaxPerson : _Availability),
                                     _DbManager.Parameter("@IsSolo", FarmStaysRooms.IsSolo),
                                     _DbManager.Parameter("@Id", FarmStaysRooms.Id)).ExecuteScalar<Int32>();
                        }


                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
                return 0;
            }

        }

        public List<FarmStaysRooms> GetRoomByFarmStayId(int Id)
        {
            List<FarmStaysRooms> FarmStays = new List<FarmStaysRooms>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {

                    String _RoomsSqlQuery = @"select * from FarmStaysRooms where FarmStaysId=@Id";
                    FarmStays = _DbManager.SetCommand(_RoomsSqlQuery,
                       _DbManager.Parameter("@Id", Id)).ExecuteList<FarmStaysRooms>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return FarmStays;
        }

        public int DeleteFarmStayRoom(int Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _DeleteSeasonSqlQuery = @"delete FarmStaysSeasons  where RoomId =@Id";
                    _DbManager.SetCommand(_DeleteSeasonSqlQuery, _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                    string _DeleteSqlQuery = @"Delete FarmStaysRooms where Id = @Id";
                    return _DbManager.SetCommand(_DeleteSqlQuery, _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
                return 0;
            }

        }
        #endregion

        #region Season
        public int SaveFarmStaySeason(FarmStaysSeasons FarmStaysSeasons)
        {
            try
            {
                TimeSpan ts = Convert.ToDateTime(FarmStaysSeasons.EndDate) - Convert.ToDateTime(FarmStaysSeasons.StartDate);
                int differenceInDays = ts.Days;
                using (DbManager _DbManager = new DbManager())
                {
                    List<DateTime> _CheckinDate = new List<DateTime>();
                    bool _IsExist = false;

                    DateTime ObjcheckInDate = Convert.ToDateTime(FarmStaysSeasons.StartDate);

                    string _SqlQuery = @"SELECT BookingDate from FarmStaysSeasons where RoomId=@RoomId and groupid!=@groupid";

                    _CheckinDate = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@RoomId", FarmStaysSeasons.RoomId),
                        _DbManager.Parameter("@groupid", FarmStaysSeasons.GroupId)).ExecuteScalarList<DateTime>();


                    for (int i = 0; i < differenceInDays; i++)
                    {
                        int _MatchedData = _CheckinDate.Where(x => x == ObjcheckInDate).Count();
                        ObjcheckInDate = ObjcheckInDate.AddDays(1);
                        if (_MatchedData > 0)
                        {
                            _IsExist = true;
                            break;
                        }
                    }

                    if (!_IsExist)
                    {
                        string _DeleteSqlQuery = @"Delete FarmStaysSeasons where GroupId = @GroupId";
                        string _InsertSqlQuery = @"Insert into FarmStaysSeasons(RoomId,BookingDate,Price,GroupId) Output Inserted.Id 
                                                values(@RoomId,@BookingDate,@Price,@GroupId)";


                        _DbManager.SetCommand(_DeleteSqlQuery, _DbManager.Parameter("@GroupId", FarmStaysSeasons.GroupId)).ExecuteNonQuery();


                        DateTime _BookingDate = Convert.ToDateTime(FarmStaysSeasons.StartDate);

                        Guid _NewGuid = Guid.NewGuid();

                        for (int i = 0; i <= differenceInDays; i++)
                        {
                            FarmStaysSeasons.Id = _DbManager.SetCommand(_InsertSqlQuery,
                                       _DbManager.Parameter("@RoomId", FarmStaysSeasons.RoomId),
                                       _DbManager.Parameter("@BookingDate ", _BookingDate),
                                       _DbManager.Parameter("@Price", FarmStaysSeasons.Price),
                                       _DbManager.Parameter("@GroupId", _NewGuid)).ExecuteScalar<Int32>();

                            _BookingDate = _BookingDate.AddDays(1);
                        }



                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
                return 0;
            }

        }

        public List<FarmStaysSeasons> GetSeasonByRoomId(int Id)
        {
            List<FarmStaysSeasons> FarmStays = new List<FarmStaysSeasons>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {

                    String _SeasonsSqlQuery = @"SELECT CAST(min(BookingDate) as DATE) as StartDate,CAST(max(BookingDate) as DATE) as EndDate,
                                                 GroupId ,Price
                                                 from FarmStaysSeasons where RoomId=@Id group by GroupId,Price order by StartDate";
                    FarmStays = _DbManager.SetCommand(_SeasonsSqlQuery,
                       _DbManager.Parameter("@Id", Id)).ExecuteList<FarmStaysSeasons>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return FarmStays;
        }

        public int DeleteFarmStaySeason(Guid GroupId)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _DeleteSqlQuery = @"Delete FarmStaysSeasons where GroupId = @GroupId";

                    return _DbManager.SetCommand(_DeleteSqlQuery, _DbManager.Parameter("@GroupId", GroupId)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
                return 0;
            }

        }
        #endregion

        public List<DiscountMaster> GetDiscountByFarmStayId(int Id)
        {
            List<DiscountMaster> FarmStays = new List<DiscountMaster>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {

                    String _SeasonsSqlQuery = @" select DM.* from DiscountMaster DM
                                              inner join [DiscountHistory] DH on DH.DiscountId=DM.Id and DH.FarmstaysId=@Id
                                              where DM.IsEBO=1 order by DM.StartDate";
                    FarmStays = _DbManager.SetCommand(_SeasonsSqlQuery,
                       _DbManager.Parameter("@Id", Id)).ExecuteList<DiscountMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return FarmStays;
        }

        public List<FarmStays> GetFarmStaysForDropDown()
        {
            List<FarmStays> ListFarmStays = new List<FarmStays>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT Id,Name FROM [dbo].[FarmStays] WHERE IsActive = 1 ";
                    ListFarmStays = _DbManager.SetCommand(_SqlQuery).ExecuteList<FarmStays>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return ListFarmStays;
        }

        public SeasonListResponse GetSeasonByBookingDateRoomId(SeasonRequest _SeasonRequest)
        {
            List<FarmStaysSeasons> FarmStays = new List<FarmStaysSeasons>();
            SeasonListResponse _SeasonListResponse = new SeasonListResponse();
            RatePlan _RatePlan = new RatePlan();
            List<RatePlan> _ListRatePlan = new List<RatePlan>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {

                    String _SeasonsSqlQuery = @"SELECT RoomId,BookingDate,RatePlanId,Price,[Double],
                                                Triple,ExtraBed,ExtraChild,ExtraAdult
                                                from FarmStaysSeasons where RoomId=@RoomId and @SeasonStartDate <= BookingDate and BookingDate<=@SeasonEndDate
                                                Order By BookingDate DESC";
                    FarmStays = _DbManager.SetCommand(_SeasonsSqlQuery,
                       _DbManager.Parameter("@RoomId", _SeasonRequest.RoomId),
                       _DbManager.Parameter("@SeasonStartDate", _SeasonRequest.SeasonStartDate),
                       _DbManager.Parameter("@SeasonEndDate", _SeasonRequest.SeasonEndDate)).ExecuteList<FarmStaysSeasons>();
                    FarmStays.Reverse();

                    List<DateTime> allDates = new List<DateTime>();
                    for (DateTime date = _SeasonRequest.SeasonStartDate; date <= _SeasonRequest.SeasonEndDate; date = date.AddDays(1))
                        allDates.Add(date);
                    //allDates.OrderByDescending(x => x);
                    //allDates.Reverse();

                    //if (FarmStays.Count() > 0)
                    //{
                    foreach (RatePlanEnum rpitem in Enum.GetValues(typeof(RatePlanEnum)))
                    {
                        List<Plans> _ListPlan = new List<Plans>();
                        foreach (PlanEnum pitem in Enum.GetValues(typeof(PlanEnum)))
                        {
                            List<PriceWithDates> _PriceDate = new List<PriceWithDates>();
                            foreach (DateTime _Date in allDates)
                            {
                                int count = FarmStays.Where(x => x.BookingDate == _Date && x.RatePlanId == Convert.ToInt32(rpitem)).Count();
                                if (count > 0)
                                {
                                    foreach (FarmStaysSeasons _FSSeason in FarmStays.Where(x => x.BookingDate == _Date && x.RatePlanId == Convert.ToInt32(rpitem)))
                                    {

                                        #region Prices
                                        if (Convert.ToInt32(PlanEnum.Price) == Convert.ToInt32(pitem))
                                        {
                                            PriceWithDates _pd = new PriceWithDates();
                                            _pd.Price = _FSSeason.Price;
                                            _pd.Date = _FSSeason.BookingDate.ToString();
                                            _PriceDate.Add(_pd);
                                        }
                                        else if (Convert.ToInt32(PlanEnum.Double) == Convert.ToInt32(pitem))
                                        {
                                            PriceWithDates _pd = new PriceWithDates();
                                            _pd.Price = _FSSeason.Double;
                                            _pd.Date = _FSSeason.BookingDate.ToString();
                                            _PriceDate.Add(_pd);
                                        }
                                        else if (Convert.ToInt32(PlanEnum.Triple) == Convert.ToInt32(pitem))
                                        {
                                            PriceWithDates _pd = new PriceWithDates();
                                            _pd.Price = _FSSeason.Triple;
                                            _pd.Date = _FSSeason.BookingDate.ToString();
                                            _PriceDate.Add(_pd);
                                        }
                                        else if (Convert.ToInt32(PlanEnum.ExtraBed) == Convert.ToInt32(pitem))
                                        {
                                            PriceWithDates _pd = new PriceWithDates();
                                            _pd.Price = _FSSeason.ExtraBed;
                                            _pd.Date = _FSSeason.BookingDate.ToString();
                                            _PriceDate.Add(_pd);
                                        }
                                        else if (Convert.ToInt32(PlanEnum.ExtraChild) == Convert.ToInt32(pitem))
                                        {
                                            PriceWithDates _pd = new PriceWithDates();
                                            _pd.Price = _FSSeason.ExtraChild;
                                            _pd.Date = _FSSeason.BookingDate.ToString();
                                            _PriceDate.Add(_pd);
                                        }
                                        else if (Convert.ToInt32(PlanEnum.ExtraAdult) == Convert.ToInt32(pitem))
                                        {
                                            PriceWithDates _pd = new PriceWithDates();
                                            _pd.Price = _FSSeason.ExtraAdult;
                                            _pd.Date = _FSSeason.BookingDate.ToString();
                                            _PriceDate.Add(_pd);
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(PlanEnum.Price) == Convert.ToInt32(pitem))
                                    {
                                        PriceWithDates _pd = new PriceWithDates();
                                        _pd.Price = 0;
                                        _pd.Date = _Date.ToString();
                                        _PriceDate.Add(_pd);
                                    }
                                    else if (Convert.ToInt32(PlanEnum.Double) == Convert.ToInt32(pitem))
                                    {
                                        PriceWithDates _pd = new PriceWithDates();
                                        _pd.Price = 0;
                                        _pd.Date = _Date.ToString();
                                        _PriceDate.Add(_pd);
                                    }
                                    else if (Convert.ToInt32(PlanEnum.Triple) == Convert.ToInt32(pitem))
                                    {
                                        PriceWithDates _pd = new PriceWithDates();
                                        _pd.Price = 0;
                                        _pd.Date = _Date.ToString();
                                        _PriceDate.Add(_pd);
                                    }
                                    else if (Convert.ToInt32(PlanEnum.ExtraBed) == Convert.ToInt32(pitem))
                                    {
                                        PriceWithDates _pd = new PriceWithDates();
                                        _pd.Price = 0;
                                        _pd.Date = _Date.ToString();
                                        _PriceDate.Add(_pd);
                                    }
                                    else if (Convert.ToInt32(PlanEnum.ExtraChild) == Convert.ToInt32(pitem))
                                    {
                                        PriceWithDates _pd = new PriceWithDates();
                                        _pd.Price = 0;
                                        _pd.Date = _Date.ToString();
                                        _PriceDate.Add(_pd);
                                    }
                                    else if (Convert.ToInt32(PlanEnum.ExtraAdult) == Convert.ToInt32(pitem))
                                    {
                                        PriceWithDates _pd = new PriceWithDates();
                                        _pd.Price = 0;
                                        _pd.Date = _Date.ToString();
                                        _PriceDate.Add(_pd);
                                    }
                                }
                            }
                            _ListPlan.Add(new Plans() { PlanName = Helper.GetEnumDescription(pitem), ListOfPriceDates = _PriceDate });
                            /////
                            #region Old Code
                            //List<PriceWithDates> _PriceDate = new List<PriceWithDates>();
                            //foreach (FarmStaysSeasons _FSSeason in FarmStays)
                            //{
                            //    foreach(DateTime _Date in allDates){

                            //        if (_FSSeason.RatePlanId == Convert.ToInt32(rpitem) && _Date == _FSSeason.BookingDate)
                            //        {

                            //            if (Convert.ToInt32(PlanEnum.Single) == Convert.ToInt32(pitem))
                            //            {
                            //                PriceWithDates _pd = new PriceWithDates();
                            //                _pd.Price = _FSSeason.Single;
                            //                _pd.Date = _FSSeason.BookingDate;
                            //                _PriceDate.Add(_pd);
                            //            }
                            //            if (Convert.ToInt32(PlanEnum.Double) == Convert.ToInt32(pitem))
                            //            {
                            //                PriceWithDates _pd = new PriceWithDates();
                            //                _pd.Price = _FSSeason.Double;
                            //                _pd.Date = _FSSeason.BookingDate;
                            //                _PriceDate.Add(_pd);
                            //            }
                            //            if (Convert.ToInt32(PlanEnum.Triple) == Convert.ToInt32(pitem))
                            //            {
                            //                PriceWithDates _pd = new PriceWithDates();
                            //                _pd.Price = _FSSeason.Triple;
                            //                _pd.Date = _FSSeason.BookingDate;
                            //                _PriceDate.Add(_pd);
                            //            }
                            //            if (Convert.ToInt32(PlanEnum.ExtraBed) == Convert.ToInt32(pitem))
                            //            {
                            //                PriceWithDates _pd = new PriceWithDates();
                            //                _pd.Price = _FSSeason.ExtraBed;
                            //                _pd.Date = _FSSeason.BookingDate;
                            //                _PriceDate.Add(_pd);
                            //            }
                            //            if (Convert.ToInt32(PlanEnum.ExtraChild) == Convert.ToInt32(pitem))
                            //            {
                            //                PriceWithDates _pd = new PriceWithDates();
                            //                _pd.Price = _FSSeason.ExtraChild;
                            //                _pd.Date = _FSSeason.BookingDate;
                            //                _PriceDate.Add(_pd);
                            //            }
                            //            if (Convert.ToInt32(PlanEnum.ExtraAdult) == Convert.ToInt32(pitem))
                            //            {
                            //                PriceWithDates _pd = new PriceWithDates();
                            //                _pd.Price = _FSSeason.ExtraAdult;
                            //                _pd.Date = _FSSeason.BookingDate;
                            //                _PriceDate.Add(_pd);
                            //            }
                            //        }
                            //        else{

                            //        }
                            //    }

                            //}
                            #endregion

                        }

                        _ListRatePlan.Add(new RatePlan() { RatePlanName = Helper.GetEnumDescription(rpitem), ListOfPlans = _ListPlan });
                    }
                    _SeasonListResponse.ListOfRatePlan = _ListRatePlan;
                    //}
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _SeasonListResponse;
        }

        public int UpdateSeason(SeasonListResponse p_SeasonListResponse)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string accessKey = @System.Configuration.ConfigurationManager.AppSettings["AccessKey"].ToString();
                    string channelId = @System.Configuration.ConfigurationManager.AppSettings["ChannelId"].ToString();

                    #region Get Hotel Id from Room Id

                    string _GetSqlQuery = @"Select FarmStaysId,IsSolo from FarmStaysRooms WHERE Id = @RoomId";
                    FarmStaysRooms _FRoom = _DbManager.SetCommand(_GetSqlQuery, _DbManager.Parameter("@RoomId", p_SeasonListResponse.RoomId)).ExecuteObject<FarmStaysRooms>();
                    #endregion

                    List<DateTime> allDates = new List<DateTime>();
                    for (DateTime date = p_SeasonListResponse.SeasonStartDate; date <= p_SeasonListResponse.SeasonEndDate; date = date.AddDays(1))
                        allDates.Add(date);
                    if (allDates.Count() > 0)
                    {
                        string _DeleteSqlQuery = @"Delete FarmStaysSeasons where RoomId = @RoomId and BookingDate >= @StartDate and BookingDate <= @EndDate";
                        _DbManager.SetCommand(_DeleteSqlQuery, _DbManager.Parameter("@RoomId", p_SeasonListResponse.RoomId),
                                                               _DbManager.Parameter("@StartDate", p_SeasonListResponse.SeasonStartDate),
                                                               _DbManager.Parameter("@EndDate", p_SeasonListResponse.SeasonEndDate)).ExecuteNonQuery();


                        string _InsertSqlQuery = @"Insert into FarmStaysSeasons(RoomId,BookingDate,RatePlanId,Price,[Double],Triple,ExtraBed,ExtraChild,ExtraAdult) Output Inserted.Id 
                                                values(@RoomId,@BookingDate,@RatePlanId,@Price,@Double,@Triple,@ExtraBed,@ExtraChild,@ExtraAdult)";

                        AxisRoom _AxisRoom = new AxisRoom();
                        _AxisRoom.accessKey = accessKey;
                        _AxisRoom.channelId = channelId;

                        Hotel _Hotel = new Hotel();
                        _Hotel.hotelId = _FRoom.FarmStaysId.ToString();

                        Room _Room = new Room();
                        _Room.roomId = p_SeasonListResponse.RoomId.ToString();

                        List<AxisRatePlan> RatePlanList = new List<AxisRatePlan>();


                        List<FarmStaysSeasons> _ListObj = new List<FarmStaysSeasons>();

                        foreach (RatePlanEnum rpitem in Enum.GetValues(typeof(RatePlanEnum)))
                        {
                            AxisRatePlan _AxisRatePlan = new AxisRatePlan();
                            var ratePlanIdinstring = Helper.GetValueFromDescription<RatePlanEnum>(Helper.GetEnumDescription(rpitem));
                            _AxisRatePlan.rateplanId = Convert.ToString(Convert.ToInt32(ratePlanIdinstring));

                            List<PriceDetails> PriceDetailsList = new List<PriceDetails>();

                            foreach (DateTime _Date in allDates)
                            {
                                PriceDetails _PriceDetails = new PriceDetails();
                                _PriceDetails.date = _Date.ToString("yyyy-MM-dd");

                                PlanValue _PlanValue = new PlanValue();
                                Price _axisPrice = new Price();

                                RatePlan _ratePlan = p_SeasonListResponse.ListOfRatePlan.Where(x => x.RatePlanName == Helper.GetEnumDescription(rpitem)).FirstOrDefault();


                                #region Pricing
                                foreach (Plans _plan in _ratePlan.ListOfPlans)
                                {
                                    if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.Price) && _plan.ListOfPriceDates.Any(x => x.Date == _Date.ToString()))
                                    {
                                        _PlanValue.Price = _plan.ListOfPriceDates.Where(x => x.Date == _Date.ToString()).FirstOrDefault().Price;
                                        _axisPrice.Single = _PlanValue.Price;
                                    }
                                    else if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.Double) && _plan.ListOfPriceDates.Any(x => x.Date == _Date.ToString()))
                                    {
                                        _PlanValue.Double = _plan.ListOfPriceDates.Where(x => x.Date == _Date.ToString()).FirstOrDefault().Price;
                                        _axisPrice.Double = _PlanValue.Double;
                                    }
                                    else if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.Triple) && _plan.ListOfPriceDates.Any(x => x.Date == _Date.ToString()))
                                    {
                                        _PlanValue.Triple = _plan.ListOfPriceDates.Where(x => x.Date == _Date.ToString()).FirstOrDefault().Price;
                                        _axisPrice.Triple = _PlanValue.Triple;
                                    }
                                    else if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.ExtraBed) && _plan.ListOfPriceDates.Any(x => x.Date == _Date.ToString()))
                                    {
                                        _PlanValue.ExtraBed = _plan.ListOfPriceDates.Where(x => x.Date == _Date.ToString()).FirstOrDefault().Price;
                                        _axisPrice.ExtraBed = _PlanValue.ExtraBed;
                                    }
                                    else if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.ExtraChild) && _plan.ListOfPriceDates.Any(x => x.Date == _Date.ToString()))
                                    {
                                        _PlanValue.ExtraChild = _plan.ListOfPriceDates.Where(x => x.Date == _Date.ToString()).FirstOrDefault().Price;
                                        _axisPrice.ExtraChild = _PlanValue.ExtraChild;
                                    }
                                    else if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.ExtraAdult) && _plan.ListOfPriceDates.Any(x => x.Date == _Date.ToString()))
                                    {
                                        _PlanValue.ExtraAdult = _plan.ListOfPriceDates.Where(x => x.Date == _Date.ToString()).FirstOrDefault().Price;
                                        _axisPrice.ExtraAdult = _PlanValue.ExtraAdult;
                                    }

                                }
                                #endregion
                                _PriceDetails.price = _axisPrice;
                                if (_axisPrice.Single > 0 && _axisPrice.Double > 0 && _axisPrice.ExtraBed > 0)///////if value for any plans then only add to the rateplan Array
                                    PriceDetailsList.Add(_PriceDetails);

                                int _update = _DbManager.SetCommand(_InsertSqlQuery,
                                  _DbManager.Parameter("@RoomId", p_SeasonListResponse.RoomId),
                                  _DbManager.Parameter("@BookingDate ", _Date),
                                  _DbManager.Parameter("@RatePlanId ", rpitem),
                                  _DbManager.Parameter("@Price", _PlanValue.Price),
                                  _DbManager.Parameter("@Double", _PlanValue.Double),
                                  _DbManager.Parameter("@Triple ", _PlanValue.Triple),
                                  _DbManager.Parameter("@ExtraBed", _PlanValue.ExtraBed),
                                  _DbManager.Parameter("@ExtraChild", _PlanValue.ExtraChild),
                                  _DbManager.Parameter("@ExtraAdult", _PlanValue.ExtraAdult)).ExecuteScalar<Int32>();

                            }
                            _AxisRatePlan.priceDetails = PriceDetailsList;
                            if (_AxisRatePlan.priceDetails.Count > 0)
                                RatePlanList.Add(_AxisRatePlan);
                        }

                        _AxisRoom.hotels = new List<Hotel>();
                        _AxisRoom.hotels.Add(_Hotel);
                        _AxisRoom.hotels[0].rooms = new List<Room>();
                        _AxisRoom.hotels[0].rooms.Add(_Room);



                        //   RatePlanList = RatePlanList.Where(x => x.priceDetails.Exists(x1 => x1.price.ExtraBed > 0 && x1.price.Single > 0 && x1.price.Double > 0)).ToList();
                        _AxisRoom.hotels[0].rooms[0].rateplans = RatePlanList;


                        if(_FRoom.IsSolo == false)
                        {
                            HttpClient client = new HttpClient();

                            var p = new JavaScriptSerializer().Serialize(_AxisRoom);

                            p = p.Replace("ExtraBed", "Extra Bed");
                            p = p.Replace("ExtraChild", "Extra Child");
                            p = p.Replace("ExtraAdult", "Extra Adult");


                            Object _hotelnew = JsonConvert.DeserializeObject<Object>(p);


                            var response = client.PostAsJsonAsync("https://sandbox-pms.axisrooms.com/api/daywisePrice", _hotelnew).Result;
                            //var response = client.PostAsJsonAsync("https://test.axisrooms.com/api/daywisePrice", _AxisRoom).Result;
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                return 1;
                            }
                            else
                            {
                                return 3;
                            }
                        }
                        else
                        {
                            return 4;
                        }

                        
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            catch (Exception _Exception)
            {
                return 0;
            }

        }

        public int AddSeason(AddSeasonResponse p_AddSeasonResponse)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string accessKey = @System.Configuration.ConfigurationManager.AppSettings["AccessKey"].ToString();
                    string channelId = @System.Configuration.ConfigurationManager.AppSettings["ChannelId"].ToString();

                    #region Get Hotel Id from Room Id

                    string _GetSqlQuery = @"Select FarmStaysId,IsSolo from FarmStaysRooms WHERE Id = @RoomId";
                    FarmStaysRooms _FRoom = _DbManager.SetCommand(_GetSqlQuery, _DbManager.Parameter("@RoomId", p_AddSeasonResponse.RoomId)).ExecuteObject<FarmStaysRooms>();
                    #endregion

                    List<DateTime> allDates = new List<DateTime>();
                    for (DateTime date = p_AddSeasonResponse.SeasonStartDate; date <= p_AddSeasonResponse.SeasonEndDate; date = date.AddDays(1))
                        allDates.Add(date);

                    if (allDates.Count() > 0)
                    {
                        string _DeleteSqlQuery = @"Delete FarmStaysSeasons where RoomId = @RoomId and BookingDate >= @StartDate and BookingDate <= @EndDate";
                        _DbManager.SetCommand(_DeleteSqlQuery, _DbManager.Parameter("@RoomId", p_AddSeasonResponse.RoomId),
                                                               _DbManager.Parameter("@StartDate", p_AddSeasonResponse.SeasonStartDate),
                                                               _DbManager.Parameter("@EndDate", p_AddSeasonResponse.SeasonEndDate)).ExecuteNonQuery();


                        string _InsertSqlQuery = @"Insert into FarmStaysSeasons(RoomId,BookingDate,RatePlanId,Price,[Double],Triple,ExtraBed,ExtraChild,ExtraAdult) Output Inserted.Id 
                                                values(@RoomId,@BookingDate,@RatePlanId,@Price,@Double,@Triple,@ExtraBed,@ExtraChild,@ExtraAdult)";

                        AxisRoom _AxisRoom = new AxisRoom();
                        _AxisRoom.accessKey = accessKey;
                        _AxisRoom.channelId = channelId;

                        Hotel _Hotel = new Hotel();
                        _Hotel.hotelId = _FRoom.FarmStaysId.ToString();

                        Room _Room = new Room();
                        _Room.roomId = p_AddSeasonResponse.RoomId.ToString();

                        List<AxisRatePlan> RatePlanList = new List<AxisRatePlan>();

                        List<FarmStaysSeasons> _ListObj = new List<FarmStaysSeasons>();


                        int _axisCount = 1;
                        foreach (DateTime _Date in allDates)
                        {
                            foreach (RatePlanEnum rpitem in Enum.GetValues(typeof(RatePlanEnum)))
                            {
                                //if (_axisCount == 1)
                                //{
                                PriceDetails _PriceDetails = new PriceDetails();
                                _PriceDetails.startDate = p_AddSeasonResponse.SeasonStartDate.ToString("yyyy-MM-dd");
                                _PriceDetails.endDate = p_AddSeasonResponse.SeasonEndDate.ToString("yyyy-MM-dd");
                                AxisRatePlan _AxisRatePlan = new AxisRatePlan();
                                var ratePlanIdinstring = Helper.GetValueFromDescription<RatePlanEnum>(Helper.GetEnumDescription(rpitem));
                                _AxisRatePlan.rateplanId = Convert.ToString(Convert.ToInt32(ratePlanIdinstring));

                                List<PriceDetails> PriceDetailsList = new List<PriceDetails>();
                                Price _axisPrice = new Price();
                                //}




                                RatePlan _ratePlan = p_AddSeasonResponse.ListOfRatePlan.Where(x => x.RatePlanName == Helper.GetEnumDescription(rpitem)).FirstOrDefault();

                                PlanValue _PlanValue = new PlanValue();
                                foreach (Plans _plan in _ratePlan.ListOfPlans)
                                {
                                    if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.Price))
                                    {
                                        _PlanValue.Price = _plan.Price;
                                        _axisPrice.Single = _PlanValue.Price;
                                    }
                                    else if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.Double))
                                    {
                                        _PlanValue.Double = _plan.Price;
                                        _axisPrice.Double = _PlanValue.Double;
                                    }
                                    else if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.Triple))
                                    {
                                        _PlanValue.Triple = _plan.Price;
                                        _axisPrice.Triple = _PlanValue.Triple;
                                    }
                                    else if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.ExtraBed))
                                    {
                                        _PlanValue.ExtraBed = _plan.Price;
                                        _axisPrice.ExtraBed = _PlanValue.ExtraBed;
                                    }
                                    else if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.ExtraChild))
                                    {
                                        _PlanValue.ExtraChild = _plan.Price;
                                        _axisPrice.ExtraChild = _PlanValue.ExtraChild;
                                    }
                                    else if (_plan.PlanName == Helper.GetEnumDescription(PlanEnum.ExtraAdult))
                                    {
                                        _PlanValue.ExtraAdult = _plan.Price;
                                        _axisPrice.ExtraAdult = _PlanValue.ExtraAdult;
                                    }

                                }
                                _PriceDetails.price = _axisPrice;

                                if (_axisPrice.Single > 0 && _axisPrice.Double > 0 && _axisPrice.ExtraBed > 0)///////if value for any plans then only add to the rateplan Array
                                    PriceDetailsList.Add(_PriceDetails);

                                int _update = _DbManager.SetCommand(_InsertSqlQuery,
                                  _DbManager.Parameter("@RoomId", p_AddSeasonResponse.RoomId),
                                  _DbManager.Parameter("@BookingDate ", _Date),
                                  _DbManager.Parameter("@RatePlanId ", rpitem),
                                  _DbManager.Parameter("@Price", _PlanValue.Price),
                                  _DbManager.Parameter("@Double", _PlanValue.Double),
                                  _DbManager.Parameter("@Triple ", _PlanValue.Triple),
                                  _DbManager.Parameter("@ExtraBed", _PlanValue.ExtraBed),
                                  _DbManager.Parameter("@ExtraChild", _PlanValue.ExtraChild),
                                  _DbManager.Parameter("@ExtraAdult", _PlanValue.ExtraAdult)).ExecuteScalar<Int32>();

                                _AxisRatePlan.priceDetails = PriceDetailsList;
                                if (_AxisRatePlan.priceDetails.Count > 0)
                                    RatePlanList.Add(_AxisRatePlan);
                            }

                        }

                        List<AxisRatePlan> _ListOfratePlanIds = new List<AxisRatePlan>();
                        foreach (AxisRatePlan _axisRP in RatePlanList)
                        {
                            if (_ListOfratePlanIds.Where(x => x.rateplanId == _axisRP.rateplanId).Count() == 0)
                            {
                                _ListOfratePlanIds.Add(_axisRP);
                            }
                        }

                        _AxisRoom.hotels = new List<Hotel>();
                        _AxisRoom.hotels.Add(_Hotel);
                        _AxisRoom.hotels[0].rooms = new List<Room>();
                        _AxisRoom.hotels[0].rooms.Add(_Room);
                        //_AxisRoom.Hotel.Room.RatePlanList = RatePlanList;
                        _AxisRoom.hotels[0].rooms[0].rateplans = _ListOfratePlanIds;

                        if(_FRoom.IsSolo == false)
                        {
                            HttpClient client = new HttpClient();

                            string p = new JavaScriptSerializer().Serialize(_AxisRoom);

                            p = p.Replace("ExtraBed", "Extra Bed");
                            p = p.Replace("ExtraChild", "Extra Child");
                            p = p.Replace("ExtraAdult", "Extra Adult");


                            Object _hotelnew = JsonConvert.DeserializeObject<Object>(p);

                            var response = client.PostAsJsonAsync("http://sandbox-pms.axisrooms.com/api/bulkPriceUpdate", _hotelnew).Result;
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                return 1;
                            }
                            else
                            {
                                return 3;
                            }
                        }
                        else
                        {
                            return 4;
                        }
                        

                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            catch (Exception _Exception)
            {
                return 0;
            }

        }
    }
}

