using HSBM.Repository.Contracts;
using HSBM.EntityModel.CityMaster;
using HSBM.Common.Utils;
using System.Collections.Generic;
using BLToolkit.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using HSBM.Common.Enums;
using System.Data;
using HSBM.EntityModel.Common;

namespace HSBM.Repository.Repositories
{
    public class CityMasterRepository : Repository<CityMaster>, ICityMasterRepository
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string Region_ID = "region id";
        public const string Region_Name = "region name";
        public const string State = "state";
        public const string Country_ID = "country id";
        public const int Name_Length = 50;

        public List<CityMasterResponse> GetAllCityBySearchRequest(GridParams p_GridParams, CityMasterRequest p_SearchRequest)
        {
            List<CityMasterResponse> objList = new List<CityMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"
SELECT 
City.[Id],City.[CityName], Region.[RegionName], 
Country.[CountryName], City.[Code],City.[IsActive], City.ImageUrl, 
IsNull(City.[IsTopDestination], 'false') AS IsTopDestination, RecordsTotal = COUNT(*) OVER(), TotalTopDestination = COUNT(City.IsTopDestination) OVER() 
FROM [dbo].[CityMaster] as City 
left join [RegionMaster] as Region on City.RegionMasterId = Region.Id 
left join [CountryMaster] as Country on Country.Id = Region.CountryMasterId
";

                    bool IsWhereClauseEmpty = true;
                    if (!string.IsNullOrEmpty(p_SearchRequest.CountryName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " Country.[CountryName] like '%" + p_SearchRequest.CountryName + "%' ";
                    }
                    if (!string.IsNullOrEmpty(p_SearchRequest.RegionName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " Region.[RegionName] like '%" + p_SearchRequest.RegionName + "%' ";
                    }
                    if (!string.IsNullOrEmpty(p_SearchRequest.CityName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " City.[CityName] like '%" + p_SearchRequest.CityName + "%' ";
                    }

                    if (!string.IsNullOrEmpty(p_SearchRequest.Code))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " City.[Code] like '%" + p_SearchRequest.Code + "%' ";
                    }

                    //if (!p_SearchRequest.IncludeIsDeleted)
                    //{
                    //    if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                    //    IsWhereClauseEmpty = false;
                    //    _SqlQuery += " City.[IsActive] = 1 ";
                    //}
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @"
ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<CityMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

       
        private bool CheckCityNameForDuplicate(DbManager _DbManager, CityMaster _CityMaster)
        {
            String _SqlQuery = @" select * from CityMaster where CityName= @CityName and CountryMasterId=@CountryMasterId";
            CityMaster _City = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@CityName", _CityMaster.CityName),
                                                             _DbManager.Parameter("@CountryMasterId", _CityMaster.CountryMasterId)).ExecuteList<CityMaster>().FirstOrDefault();

            if (_City != null)
            {
                if (_City.RoomXMLRegionId == null || _City.RoomXMLRegionId == 0)
                {
                    String _CityQuery = @"update CityMaster set RoomXMLRegionId=@RoomXMLRegionId ,IsActive=1  output Inserted.Id  where CityName=@CityName and CountryMasterId=@CountryMasterId";
                    int _CityId = _DbManager.SetCommand(_CityQuery,
                                                   _DbManager.Parameter("@CityName", _CityMaster.CityName),
                                                   _DbManager.Parameter("@RoomXMLRegionId", _CityMaster.RoomXMLRegionId),
                                                   _DbManager.Parameter("@CountryMasterId", _CityMaster.CountryMasterId)).ExecuteNonQuery();
                }
                return true;

            }


            return false;
        }

        #region Get All City
        /// <summary>
        /// Get All City
        /// </summary>
        /// <returns></returns>
        public List<CityMaster> GetAllCity()
        {
            List<CityMaster> listOfCityMaster = new List<CityMaster>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"
select CTM.Id, CTM.CityName,RM.RegionName,CM.CountryName,CTM.Code,ISNULL(CTM.IsTopDestination, 'false') AS IsTopDestination, CTM.ImageUrl, CTM.IsActive, CTM.HotelBedsCode,
CTM.Latitude, CTM.Longitude
from CityMaster CTM 
Left join RegionMaster RM on CTM.RegionMasterId=RM.Id
Left join CountryMaster CM on CM.Id=CTM.CountryMasterId";

                    listOfCityMaster = _DbManager.SetCommand(_SqlQuery).ExecuteList<CityMaster>();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return listOfCityMaster;
        }
        #endregion

        #region Get City ById
        /// <summary>
        /// Get City By Id
        /// </summary>
        /// <param name="p_Id"></param>
        /// <returns></returns>
        public CityMaster GetCityById(long p_Id)
        {
            CityMaster cityMaster = new CityMaster();

            try
            {
                using (DbManager _DBManager = new DbManager())
                {
                    String _SqlQuery = @"Select Id, CityName, Code, ImageUrl, Latitude, Longitude, RegionMasterId, CountryMasterId, RoomXMLRegionId, IsActive
                                                From CityMaster Where Id = @Id";

                    cityMaster = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@Id", p_Id)).ExecuteObject<CityMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return cityMaster;
        }
        #endregion

        #region Add Update CityMaster
        /// <summary>
        /// AddUpdate CityMaster
        /// </summary>
        /// <param name="p_CityMaster">Object of CityMaster</param>
        /// <returns></returns>
        public bool AddUpdateCityMaster(CityMaster p_CityMaster)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    if (p_CityMaster != null)
                    {
                        String _SqlQuery = string.Empty;

                        bool _IsExistCityName = CheckNameForDuplicate(_DbManager, p_CityMaster);

                        if (_IsExistCityName)
                        {
                            return false;
                        }
                        else
                        {
                            if (p_CityMaster.Id > 0)
                            {

                                _SqlQuery = @" Update CityMaster set CountryMasterId = @CountryMasterId, 
                                                                RegionMasterId = @RegionMasterId, 
                                                                CityName = @CityName,
                                                                Code = @Code,
                                                                ImageUrl = @ImageUrl,
                                                                Latitude = @Latitude,
                                                                Longitude = @Longitude,
                                                                IsActive = @IsActive Where Id = @Id";
                            }
                            else
                            {
                                _SqlQuery = @" Insert into CityMaster (CountryMasterId, RegionMasterId, CityName, Code, ImageUrl, Latitude, Longitude, IsActive) 
                                                values (@CountryMasterId, @RegionMasterId, @CityName, @Code, @ImageUrl, @Latitude, @Longitude, @IsActive)";
                            }

                            int result = _DbManager.SetCommand(_SqlQuery,
                                                                _DbManager.Parameter("@CountryMasterId", p_CityMaster.CountryMasterId)
                                                                , _DbManager.Parameter("@RegionMasterId", p_CityMaster.RegionMasterId)
                                                                , _DbManager.Parameter("@CityName", p_CityMaster.CityName)
                                                                , _DbManager.Parameter("@Code", p_CityMaster.Code)
                                                                , _DbManager.Parameter("@ImageUrl", p_CityMaster.ImageUrl)
                                                                , _DbManager.Parameter("@Latitude", p_CityMaster.Latitude)
                                                                , _DbManager.Parameter("@Longitude", p_CityMaster.Longitude)
                                                                , _DbManager.Parameter("@IsActive", p_CityMaster.IsActive)
                                                                , _DbManager.Parameter("@Id", p_CityMaster.Id)).ExecuteNonQuery();
                            if (result > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return false;
        }

        private bool CheckNameForDuplicate(DbManager _DbManager, CityMaster p_CityMaster)
        {
            try
            {
                String _SqlQuery = @"Select Count(Id) from CityMaster where Id != @Id and CityName = @CityName";

                int Affected = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", p_CityMaster.Id),
                    _DbManager.Parameter("@CityName", p_CityMaster.CityName)).ExecuteScalar<int>();

                if (Affected > 0)
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


        public bool ImportCityForHotelBeds(CityMaster cityMaster)
        {
            try
            {
                long res = 0;
                using (DbManager _DbManager = new DbManager())
                {
                    if (cityMaster != null)
                    {
                        String _SqlQuery = string.Empty;

                        _SqlQuery = @"SELECT [Id],[CountryMasterId],[CityName],[IsActive],[HotelBedsCode] FROM [dbo].[CityMaster] where CityName= @CityName and CountryMasterId=@CountryMasterId";
                        CityMaster cm = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@CityName", cityMaster.CityName),
                                                                    _DbManager.Parameter("@CountryMasterId", cityMaster.CountryMasterId)).ExecuteList<CityMaster>().FirstOrDefault();

                        List<IDbDataParameter> paramList = new List<IDbDataParameter>();                          
                           paramList.Add(_DbManager.Parameter("@CountryMasterId",cityMaster.CountryMasterId));
                           paramList.Add(_DbManager.Parameter("@CityName",cityMaster.CityName));
                           paramList.Add(_DbManager.Parameter("@IsActive",1));
                           paramList.Add(_DbManager.Parameter("@HotelBedsCode", cityMaster.HotelBedsCode));

                        if (cm != null && cm.Id > 0)
                        {
                            _SqlQuery = @"UPDATE [dbo].[CityMaster] SET [CountryMasterId] = @CountryMasterId,[CityName] = @CityName,[IsActive] = @IsActive,
                                              [HotelBedsCode] = @HotelBedsCode WHERE Id = " + cm.Id;
                            res = _DbManager.SetCommand(_SqlQuery, paramList.ToArray()).ExecuteNonQuery();
                            res = cm.Id;
                            _ILogger.Error("HotelBeds=>LocationService(AddDestinationMaster): DestinationAlreadyExist-[" + cm.CityName + "]");
                        }
                        else {
                            _SqlQuery = @"INSERT INTO [dbo].[CityMaster]([CountryMasterId],[CityName],[IsActive],[HotelBedsCode]) 
                                            VALUES (@CountryMasterId,@CityName,@IsActive,@HotelBedsCode) select SCOPE_IDENTITY() as int";
                            res = _DbManager.SetCommand(_SqlQuery, paramList.ToArray()).ExecuteScalar<int>();
                        }                       
                    }
                }
                return res > 0 ? true : false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Delete City ById
        /// <summary>
        /// Delete City ById
        /// </summary>
        /// <param name="p_Id"></param>
        /// <returns></returns>
        public bool DeleteCityById(long p_Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = "Delete from CityMaster where Id= @Id";
                    int result = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@Id", p_Id)).ExecuteNonQuery();

                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return false;
        }
        #endregion

        #region Active Inactive City ById
        /// <summary>
        /// Active Inactive City ById
        /// </summary>
        /// <param name="p_CityMaster"></param>
        /// <returns></returns>
        public bool ActiveAndInactiveSwitchForCityUpdate(CityMaster p_CityMaster)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = "Update CityMaster set IsActive = @IsActive Where Id = @Id";

                    int _Result = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@IsActive", p_CityMaster.IsActive),
                         _DbManager.Parameter("@Id", p_CityMaster.Id)).ExecuteNonQuery();

                    if (_Result > 0)
                    {
                        return true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return false;
        }
        #endregion

        #region Active Inactive City ById
        /// <summary>
        /// Active Inactive City ById
        /// </summary>
        /// <param name="p_CityMaster"></param>
        /// <returns></returns>
        public bool TopDestinationSwitchForCityUpdate(CityMaster p_CityMaster)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = "Update CityMaster set IsTopDestination = @IsTop Where Id = @Id";

                    int _Result = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@IsTop", p_CityMaster.IsTopDestination),
                         _DbManager.Parameter("@Id", p_CityMaster.Id)).ExecuteNonQuery();

                    if (_Result > 0)
                    {
                        return true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return false;
        }
        #endregion

        #region Get All City By RegionMasterId
        /// <summary>
        /// Get All City By RegionMasterId
        /// </summary>
        /// <param name="p_Id">Region Id</param>
        /// <returns></returns>
        public List<CityMaster> GetAllCityByRegionMasterId(long p_Id)
        {
            List<CityMaster> _ListOfCityMaster = new List<CityMaster>();

            try
            {
                using (DbManager _DBManager = new DbManager())
                {
                    String _SqlQuery = @"Select Id, CityName, ImageUrl, Code, Latitude, Longitude, RegionMasterId, CountryMasterId, IsActive
                                                From CityMaster Where RegionMasterId = @Id and IsActive = 1";

                    _ListOfCityMaster = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@Id", p_Id)).ExecuteList<CityMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfCityMaster;
        }
        #endregion


        #region Get All City By CountryId
        /// <summary>
        /// Get All City By CountryId
        /// </summary>
        /// <param name="Country Id">Country Id</param>
        /// <returns></returns>
        public List<CityMaster> GetAllCityByCountryId(long p_CountryId)
        {
            List<CityMaster> _ListOfCityMaster = new List<CityMaster>();

            try
            {
                using (DbManager _DBManager = new DbManager())
                {
                    String _SqlQuery = @"Select Id, CityName, Code, ImageUrl, Latitude, Longitude, RegionMasterId, CountryMasterId, IsActive
                                                From CityMaster Where CountryMasterId = @Id and IsActive = 1";

                    _ListOfCityMaster = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@Id", p_CountryId)).ExecuteList<CityMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfCityMaster;
        }
        #endregion

        #region Get All Top Destination Cities
        /// <summary>
        /// Get All Top Destination Cities
        /// </summary>
        /// <returns>Top Cities Set As Top Destination</returns>
        public List<CityMaster> GetAllTopDestinationCity()
        {
            List<CityMaster> objList = new List<CityMaster>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"
SELECT TOP 9
    CT.Id,
    CT.CityName,
    CT.ImageUrl,
    RG.RegionName AS RegionName,
    CN.CountryName AS CountryName
FROM CityMaster CT
LEFT JOIN RegionMaster RG ON CT.RegionMasterId = RG.Id
LEFT JOIN CountryMaster CN ON RG.CountryMasterId = CN.Id
WHERE CT.IsTopDestination = 'true'
";

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<CityMaster>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        #endregion

        public List<KeyValueDto> GetCityNamesForDropDown(string search)
        {
            List<KeyValueDto> _List = new List<KeyValueDto>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"select a.Id as Value, a.CityName + isnull(', ' + b.regionname,'') + isnull(', ' + c.CountryName,'') as [Key]
                                        from citymaster a
                                        left join RegionMaster b on a.RegionMasterId=b.Id
                                        left join CountryMaster c on a.CountryMasterId=c.Id
                                        where a.IsActive=1 and a.cityname like '" + search + @"%'
                                        order by a.CityName + isnull(', ' + b.regionname,'') + isnull(', ' + c.CountryName,'')";

                    _List = _DbManager.SetCommand(_SqlQuery).ExecuteList<KeyValueDto>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _List;
        }
    }
}

