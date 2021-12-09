
using HSBM.Repository.Contracts;
using HSBM.EntityModel.RegionMaster;
using HSBM.Common.Utils;
using System.Collections.Generic;
using BLToolkit.Data;
using System;
namespace HSBM.Repository.Repositories
{
    public class RegionMasterRepository : Repository<RegionMaster>, IRegionMasterRepository
    {
        public List<RegionMasterResponse> GetAllRegionBySearchRequest(GridParams p_GridParams, RegionMasterRequest p_SearchRequest)
        {
            List<RegionMasterResponse> objList = new List<RegionMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT Region.[Id],Region.[RegionName],Region.[Code],Region.[IsActive],RecordsTotal = COUNT(*) OVER(), Country.[CountryName] FROM [dbo].[RegionMaster] as Region left join [CountryMaster] as Country on Region.CountryMasterId = Country.Id";

                    bool IsWhereClauseEmpty = true;
                    if (!string.IsNullOrEmpty(p_SearchRequest.CountryName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " Region.CountryName like '%" + p_SearchRequest.CountryName + "%' ";
                    }
                    if (!string.IsNullOrEmpty(p_SearchRequest.RegionName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " Region.RegionName like '%" + p_SearchRequest.RegionName + "%' ";
                    }

                    if (!string.IsNullOrEmpty(p_SearchRequest.Code))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " Region.Code like '%" + p_SearchRequest.Code + "%' ";
                    }

                    //if (!p_SearchRequest.IncludeIsDeleted)
                    //{
                    //    if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                    //    IsWhereClauseEmpty = false;
                    //    _SqlQuery += " Region.IsActive = 1 ";
                    //}
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<RegionMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        #region Get All Region
        /// <summary>
        /// Get All Region
        /// </summary>
        /// <returns></returns>
        public List<RegionMaster> GetAllRegion()
        {
            List<RegionMaster> listOfRegionMaster = new List<RegionMaster>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT Region.[Id],Region.[RegionName],Region.[Code],Region.[IsActive], Country.[CountryName] 
                                            FROM [dbo].[RegionMaster] as Region Inner Join [CountryMaster] as Country on Region.CountryMasterId = Country.Id";

                    listOfRegionMaster = _DbManager.SetCommand(_SqlQuery).ExecuteList<RegionMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return listOfRegionMaster;
        }
        #endregion

        #region Active Inactive Region ById
        /// <summary>
        /// Active Inactive Region ById
        /// </summary>
        /// <param name="p_CountryMaster">Object of RegionMaster</param>
        /// <returns></returns>
        public bool ActiveAndInactiveSwitchForRegionUpdate(RegionMaster p_RegionMaster)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = "Update RegionMaster set IsActive = @IsActive Where Id = @Id";

                    int _Result = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@IsActive", p_RegionMaster.IsActive),
                         _DbManager.Parameter("@Id", p_RegionMaster.Id)).ExecuteNonQuery();

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

        #region AddUpdate RegionMaster
        /// <summary>
        /// Add Update Region
        /// </summary>
        /// <param name="p_RegionMaster">Object Of Region Master</param>
        /// <returns></returns>
        public bool AddUpdateRegion(RegionMaster p_RegionMaster)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = string.Empty;

                    bool _IsExistRegionName = CheckNameForDuplicate(_DbManager, p_RegionMaster);

                    if (_IsExistRegionName)
                    {
                        return false;
                    }
                    else
                    {
                        if (p_RegionMaster.Id > 0)
                        {
                            _SqlQuery = @"Update RegionMaster set RegionName = @RegionName, 
                                                Code = @Code, 
                                                Latitude = @Latitude, 
                                                Longitude = @Longitude,
                                                CountryMasterId = @CountryMasterId,
                                                IsActive = @IsActive
                                                Where Id = @Id";
                        }
                        else
                        {
                            _SqlQuery = @"Insert into RegionMaster (RegionName, Code, IsActive, Latitude, Longitude, CountryMasterId)
                                            Values (@RegionName, @Code, @IsActive, @Latitude, @Longitude, @CountryMasterId)";
                        }

                        int _Result = _DbManager.SetCommand(_SqlQuery,
                             _DbManager.Parameter("@RegionName", p_RegionMaster.RegionName)
                            , _DbManager.Parameter("@CountryMasterId", p_RegionMaster.CountryMasterId)
                            , _DbManager.Parameter("@Code", p_RegionMaster.Code)
                            , _DbManager.Parameter("@IsActive", p_RegionMaster.IsActive)
                            , _DbManager.Parameter("@Latitude", p_RegionMaster.Latitude)
                            , _DbManager.Parameter("@Longitude", p_RegionMaster.Longitude)
                            , _DbManager.Parameter("@Id", p_RegionMaster.Id)).ExecuteNonQuery();

                        if (_Result > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return false;
        }

        private bool CheckNameForDuplicate(DbManager _DbManager, RegionMaster p_RegionMaster)
        {
            try
            {
                String _SqlQuery = @"Select Count(Id) from RegionMaster where Id != @Id and RegionName = @RegionName";

                int Affected = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", p_RegionMaster.Id),
                    _DbManager.Parameter("@RegionName", p_RegionMaster.RegionName)).ExecuteScalar<int>();

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
        #endregion

        #region Get Region ById
        /// <summary>
        /// Get Region By Id
        /// </summary>
        /// <param name="p_Id">Region Id</param>
        /// <returns></returns>
        public RegionMaster GetRegionById(long p_Id)
        {
            RegionMaster regionMaster = new RegionMaster();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = "Select Id, RegionName, Code, Latitude, Longitude, CountryMasterId, IsActive from RegionMaster Where Id= @Id";

                    regionMaster = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@Id", p_Id)).ExecuteObject<RegionMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return regionMaster;
        }
        #endregion

        #region Delete Region ById
        /// <summary>
        /// Delete Region ById
        /// </summary>
        /// <param name="p_Id">RegionId</param>
        /// <returns></returns>
        public bool DeleteRegionById(long p_Id)
        {
            try
            {
                using (DbManager _DBManager = new DbManager())
                {
                    String _SqlQuery = "Delete From RegionMaster Where Id = @Id";
                    int _Result = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@Id", p_Id)).ExecuteNonQuery();

                    if (_Result > 0)
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
        }
        #endregion

        #region Get RegionBy Country Id
        /// <summary>
        /// Get RegionBy Country Id
        /// </summary>
        /// <param name="p_Id">Country Id</param>
        /// <returns></returns>
        public List<RegionMaster> GetRegionByCountryId(long p_Id)
        {
            List<RegionMaster> listOfRegionMaster = new List<RegionMaster>();
            try
            {
                using (DbManager _DBManager = new DbManager())
                {
                    String _SqlQuery = @"Select Id, RegionName, CountryMasterId, IsActive from RegionMaster Where CountryMasterId = @Id";

                    listOfRegionMaster = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@Id", p_Id)).ExecuteList<RegionMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return listOfRegionMaster;
        }
        #endregion


        #region CheckRegionExist
        public bool CheckRegionExists(string regionName, long countryId)
        {
            RegionMaster regionMaster = new RegionMaster();
            try
            {
                int regionCount = 0;
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = "Select COUNT(Id) from [dbo].[RegionMaster] where RegionName=@RegionName and CountryMasterId=@CountryMasterId";
                    regionCount = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@RegionName", regionName), _DbManager.Parameter("@CountryMasterId", countryId)).ExecuteScalar<int>();

                    if (regionCount > 0)
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
        }
        #endregion

        #region GetRegion
        public List<RegionMaster> GetRegion(List<Tuple<string, string>> param)
        {
            List<RegionMaster> regionMaster = new List<RegionMaster>();
            try
            {
                string strWHERE = string.Empty;
                String _SqlQuery = string.Empty;

                using (DbManager _DbManager = new DbManager())
                {
                    if (param != null && param.Count > 0)
                    {
                        strWHERE = " where " + param[0].Item1 + "='" + param[0].Item2 + "'";
                        for (int i = 1; i < param.Count; i++)
                            strWHERE = strWHERE + "and " + param[i].Item1 + "='" + param[i].Item2 + "'";

                        _SqlQuery = "Select * from RegionMaster " + strWHERE;
                    }
                    else
                    {
                        _SqlQuery = "Select * from RegionMaster";
                    }
                    regionMaster = _DbManager.SetCommand(_SqlQuery).ExecuteList<RegionMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return regionMaster;
        }
        #endregion

    }
}