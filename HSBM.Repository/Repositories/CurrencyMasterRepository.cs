using HSBM.Repository.Contracts;
using HSBM.EntityModel.CurrencyMaster;
using System.Collections.Generic;
using HSBM.Common.Utils;
using BLToolkit.Data;
using System;
using System.Linq;
using System.Web.Mvc;
using HSBM.EntityModel.Common;
using System.Data;

namespace HSBM.Repository.Repositories
{
    public class CurrencyMasterRepository
    {
        public List<CurrencyMasterResponse> GetAllCurrencyMasterBySearchRequest(GridParams p_GridParams, CurrencyMasterRequest p_SearchRequest)
        {
            List<CurrencyMasterResponse> objList = new List<CurrencyMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT *,RecordsTotal = COUNT(*) OVER() from [dbo].[CurrencyMaster]";

                    bool IsWhereClauseEmpty = true;

                    //if (!p_SearchRequest.IncludeIsDeleted)
                    //{
                    //    if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                    //    IsWhereClauseEmpty = false;
                    //    _SqlQuery += " [IsActive] = 1 ";
                    //}
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<CurrencyMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public List<CurrencyMasterResponse> GetAllCurrencyMaster()
        {
            List<CurrencyMasterResponse> objList = new List<CurrencyMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT *,RecordsTotal = COUNT(*) OVER() from [dbo].[CurrencyMaster]";
                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<CurrencyMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public int AddorUpdateCurrency(CurrencyMaster currencyMaster)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExistCurName = CheckNameForDuplicate(_DbManager, currencyMaster);

                    if (_IsExistCurName)
                    {
                        return 2;
                    }
                    else
                    {
                        if (currencyMaster.Id != 0)
                        {
                            _SqlQuery = @"update CurrencyMaster set CurrencyName=@CurrencyName
                                                                       ,CurrencyCode = @CurrencyCode
                                                                       ,Value = @Value
                                                                       ,IsActive = @IsActive
                                                                       ,UpdatedBy = @UpdatedBy
                                                                       ,UpdateDate = @UpdateDate
                                                                        where (Id = @Id)";
                        }
                        else
                        {
                            _SqlQuery = @"insert into CurrencyMaster
                                                        (CurrencyName
                                                        ,CurrencyCode
                                                        ,Value
                                                        ,IsActive
                                                        ,CreatedBy
                                                        ,CreatedDate
                                                        ,UpdatedBy
                                                        ,UpdateDate)
                                                   values
                                                        (@CurrencyName
                                                        ,@CurrencyCode
                                                        ,@Value
                                                        ,@IsActive
                                                        ,@CreatedBy
                                                        ,@CreatedDate
                                                        ,@UpdatedBy
                                                        ,@UpdateDate)";
                        }
                        int Affected = _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@CurrencyName", currencyMaster.CurrencyName),
                            _DbManager.Parameter("@CurrencyCode", currencyMaster.CurrencyCode),
                            _DbManager.Parameter("@Value", currencyMaster.Value),
                            _DbManager.Parameter("@IsActive", currencyMaster.IsActive),
                            _DbManager.Parameter("@CreatedBy", currencyMaster.CreatedBy),
                            _DbManager.Parameter("@CreatedDate", currencyMaster.CreatedDate),
                            _DbManager.Parameter("@UpdatedBy", currencyMaster.UpdatedBy),
                            _DbManager.Parameter("@UpdateDate", currencyMaster.UpdateDate),
                            _DbManager.Parameter("@Id", currencyMaster.Id)).ExecuteNonQuery();

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
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public bool CheckNameForDuplicate(DbManager _DbManager, CurrencyMaster currencyMaster)
        {
            try
            {
                String _SqlQuery = @"Select Count(Id) from CurrencyMaster where Id!=@Id and CurrencyName=@CurrencyName";

                int Affected = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", currencyMaster.Id),
                    _DbManager.Parameter("@CurrencyName", currencyMaster.CurrencyName)).ExecuteScalar<int>();

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

        public CurrencyMaster GetCurrencyRecordById(long Id)
        {
            CurrencyMaster _CurrencyMasterModel = new CurrencyMaster();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[CurrencyName],[CurrencyCode],[Value],[IsActive],CreatedDate,CreatedBy, RecordsTotal = COUNT(*) OVER() from [dbo].[CurrencyMaster] where Id = @Id";

                    _CurrencyMasterModel = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<CurrencyMaster>().FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _CurrencyMasterModel;
        }

        public void ActiveAndInactiveSwitchUpdateForCur(CurrencyMaster currencyMaster)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"Update CurrencyMaster set IsActive = @IsActive where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                       _DbManager.Parameter("@Id", currencyMaster.Id),
                       _DbManager.Parameter("@IsActive", currencyMaster.IsActive)).ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCurrencyById(long Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"Delete CurrencyMaster where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> GetAllCurrencyForDropDown()
        {
            List<SelectListItem> _ListOfCurrency = new List<SelectListItem>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"
SELECT 
    [Id] AS Value,
    [CurrencyCode] AS Text,
    [IsBaseCurrency] AS Selected
from [dbo].[CurrencyMaster] where IsActive = 1";

                    _ListOfCurrency = _DbManager.SetCommand(_SqlQuery).ExecuteList<SelectListItem>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _ListOfCurrency;
        }

        public int UpdateCurrencyRates(Dictionary<string, decimal> UpdatedRates, long UpdatedBy)
        {
            using (DbManager _DbManager = new DbManager())
            {
                try
                {
                    String _SqlQuery;

                    _DbManager.BeginTransaction(IsolationLevel.ReadCommitted);
                    

                    foreach (var item in UpdatedRates)
                    {
                        _SqlQuery = @"update CurrencyMaster set Value = @Value,UpdatedBy = @UpdatedBy,UpdateDate = @UpdateDate where (CurrencyCode = @CurrencyCode)";

                        _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@CurrencyCode", item.Key),
                        _DbManager.Parameter("@Value", item.Value),
                        _DbManager.Parameter("@UpdatedBy", UpdatedBy),
                        _DbManager.Parameter("@UpdateDate", DateTime.Now)
                        ).ExecuteNonQuery();
                    }

                    _DbManager.CommitTransaction();

                }
                catch (Exception _Exception)
                {
                    _DbManager.RollbackTransaction();
                    throw _Exception;
                }
            }
            return 1;
        }

    }
}