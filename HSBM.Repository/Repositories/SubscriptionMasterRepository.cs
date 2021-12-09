using HSBM.Repository.Contracts;
using HSBM.EntityModel.SubscriptionMaster;
using System.Collections.Generic;
using HSBM.Common.Utils;
using BLToolkit.Data;
using System;
using System.Globalization;
namespace HSBM.Repository.Repositories
{
    public class SubscriptionMasterRepository
    {
        public List<SubscriptionMasterResponse> GetAllSubscriptionMasterBySearchRequest(GridParams p_GridParams, SubscriptionMasterRequest p_SearchRequest)
        {
            List<SubscriptionMasterResponse> objList = new List<SubscriptionMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT *,RecordsTotal = COUNT(*) OVER() from [dbo].[SubscriptionMaster]";

                    bool IsWhereClauseEmpty = true;

                    //if (!p_SearchRequest.IncludeIsDeleted)
                    //{
                    //    if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                    //    IsWhereClauseEmpty = false;
                    //    _SqlQuery += " [IsActive] = 1 ";
                    //}                     
                    DateTime CreatedDateFrom = DateTime.MinValue;
                    DateTime CreatedDateTo = DateTime.MinValue;
                    if (!string.IsNullOrEmpty(p_SearchRequest.CreatedDateFrom))
                    {
                        CreatedDateFrom = DateTime.ParseExact(p_SearchRequest.CreatedDateFrom, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(p_SearchRequest.CreatedDateTo))
                    {
                        CreatedDateTo = DateTime.ParseExact(p_SearchRequest.CreatedDateTo, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                    }
                    if (Convert.ToDateTime(CreatedDateFrom) != DateTime.MinValue)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " [CreatedDate] >= '" + CreatedDateFrom.ToString("dd/MMM/yyyy") + "' ";
                    }
                    if (Convert.ToDateTime(CreatedDateTo) != DateTime.MinValue)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += "  [CreatedDate] <= '" + CreatedDateTo.ToString("dd/MMM/yyyy") + "'";
                    }

                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<SubscriptionMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public int AddUpdateSubscription(SubscriptionMaster subscriptionMaster)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExistEmail = CheckEmailForDuplicate(_DbManager, subscriptionMaster);

                    if (_IsExistEmail)
                    {
                        return 2;
                    }
                    else
                    {
                        _SqlQuery = @"insert into SubscriptionMaster
                                                        (Email
                                                        ,FirstName
                                                        ,LastName
                                                        ,IsActive
                                                        ,CreatedDate
                                                        ,UpdateDate)
                                                   values
                                                        (@Email
                                                        ,@FirstName
                                                        ,@LastName
                                                        ,@IsActive
                                                        ,@CreatedDate
                                                        ,@UpdateDate)";
                    }
                    int Affected = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Email", subscriptionMaster.Email),
                        _DbManager.Parameter("@FirstName", subscriptionMaster.FirstName),
                        _DbManager.Parameter("@LastName", subscriptionMaster.LastName),
                        _DbManager.Parameter("@IsActive", subscriptionMaster.IsActive),
                        _DbManager.Parameter("@CreatedDate", subscriptionMaster.CreatedDate),
                        _DbManager.Parameter("@UpdateDate", subscriptionMaster.UpdateDate),
                        _DbManager.Parameter("@Id", subscriptionMaster.Id)).ExecuteNonQuery();

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

        private bool CheckEmailForDuplicate(DbManager _DbManager, SubscriptionMaster subscriptionMaster)
        {
            try
            {
                String _SqlQuery = @"Select Count(Id) from SubscriptionMaster where Id!=@Id and Email=@Email";

                int Affected = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", subscriptionMaster.Id),
                    _DbManager.Parameter("@Email", subscriptionMaster.Email)).ExecuteScalar<int>();

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

        public void ActiveAndInactiveSwitchUpdateForSC(SubscriptionMaster subscriptionMaster)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Update SubscriptionMaster set IsActive = @IsActive where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", subscriptionMaster.Id),
                        _DbManager.Parameter("@IsActive", subscriptionMaster.IsActive)).ExecuteScalar<int>();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void DeleteSubscriptionById(long Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Delete SubscriptionMaster where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteScalar<int>();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void UnsubscribeUser(long Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Update SubscriptionMaster set IsActive = @IsActive , UpdateDate = @UpdateDate where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id),
                        _DbManager.Parameter("@IsActive", false),
                        _DbManager.Parameter("@UpdateDate", DateTime.Now)).ExecuteScalar<int>();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<SubscriptionMaster> GetAllSubscription()
        {

            List<SubscriptionMaster> _SubscriptionMasterList = new List<SubscriptionMaster>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Select Email,FirstName,LastName,IsActive,CreatedDate,UpdateDate from SubscriptionMaster";

                    _SubscriptionMasterList = _DbManager.SetCommand(_SqlQuery).ExecuteList<SubscriptionMaster>();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _SubscriptionMasterList;
        }
    }
}